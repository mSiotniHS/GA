using Common;
using GraphColoring.Problem;
using GraphColoring.Math;
using OptimizationProblemsFramework;

namespace App;

using GraphColoring = GraphColoring.Math.GraphColoring;

class RandomColoringGenerator(IRng rng) : IProblemSolver<GraphColoringProblem, GraphColoring, uint>
{
    public GraphColoring FindSolution(GraphColoringProblem problem)
    {
        var coloring = new GraphColoring();

        foreach (var vertex in problem.Graph.VertexList())
        {
            coloring[vertex] = new Color(rng.GetInt(problem.Graph.VertexCount));
        }

        return coloring;
    }
}
