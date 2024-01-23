using GraphColoring.Math;
using OptimizationProblemsFramework;
using System.Linq;

namespace GraphColoring.Problem;

using GraphColoring = Math.GraphColoring;

public class GraphColoringProblem(IReadonlyGraph graph) : IOptimizationProblem<GraphColoring, uint>
{
    public Extremum ExtremumOption { get; } = Extremum.Minimum;

    public ISet<GraphColoring> Set { get; } = new GraphColoringProblemSet(graph);

    public uint Criterion(GraphColoring coloring)
    {
        var validityPenalty = 0u;

        var vertices = graph.VertexList().ToList();
        foreach (var first in vertices)
        {
            foreach (var second in vertices)
            {
                if (!graph.AreAdjacent(first, second)) continue;

                var firstColor = coloring[first];
                var secondColor = coloring[second];

                if (firstColor == secondColor)
                {
                    validityPenalty++;
                }
            }
        }

        var colorCount = coloring.ColorCount;

        var criterion = colorCount + validityPenalty * 2;

        return criterion;
    }

    public IReadonlyGraph Graph => graph;
}

internal sealed class GraphColoringProblemSet(IReadonlyGraph graph) : ISet<GraphColoring>
{
    public bool Has(GraphColoring coloring)
    {
        var vertices = graph.VertexList().ToList();

        foreach (var first in vertices)
        {
            foreach (var second in vertices)
            {
                if (!graph.AreAdjacent(first, second)) continue;

                var firstColor = coloring[first];
                var secondColor = coloring[second];

                if (firstColor is null || secondColor is null)
                    return false;

                if (firstColor.Value == secondColor.Value)
                    return false;
            }
        }

        return true;
    }
}
