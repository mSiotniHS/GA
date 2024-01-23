using GraphColoring.Math;
using GraphColoring.Problem;
using OptimizationProblemsFramework;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphColoring.Solvers;

using GraphColoring = Math.GraphColoring;

public class GreedySequentialColoring : IProblemSolver<GraphColoringProblem, GraphColoring, uint>
{
    protected readonly IOrderingStrategy OrderingStrategy;

    public GreedySequentialColoring(IOrderingStrategy orderingStrategy)
    {
        OrderingStrategy = orderingStrategy;
    }

    public virtual GraphColoring FindSolution(GraphColoringProblem problem)
    {
        var vertices = OrderingStrategy.OrderVertices(problem.Graph);
        var coloring = new GraphColoring();

        foreach (var vertex in vertices)
        {
            coloring[vertex] = SmallestPossibleColor(problem.Graph, vertex, coloring);
        }

        return coloring;
    }

    protected static Color SmallestPossibleColor(IReadonlyGraph graph, Vertex vertex, GraphColoring coloring)
    {
        var neighbourColors =
            graph
                .VertexNeighbours(vertex)
                .Select(neighbour => coloring[neighbour])
                .Distinct()
                .Where(maybeColor => maybeColor.HasValue)
                .OrderBy(color => color!.Value.Id);

        var smallestColor = Color.Smallest();

        foreach (var color in neighbourColors)
        {
            if (smallestColor != color) break;
            smallestColor = smallestColor.Next();
        }

        return smallestColor;
    }
}
