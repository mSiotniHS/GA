using GA.BaseProblem;
using GA.Core;
using GraphColoring.Math;
using GraphColoring.Problem;

namespace App;

using GraphColoring = GraphColoring.Math.GraphColoring;

/// <summary>
/// Исправляет <c>Genotype</c> таким образом, чтобы соответствующий генотипу
/// <c>GraphColoring</c> была валидна для данной задачи
/// </summary>
internal class GraphColoringDomainModificator(
        GraphColoringProblem problem,
        IGaCoder<GraphColoring> coder
    ) : IGenotypeModificator
{
    public Genotype Modify(Genotype genotype)
    {
        var coloring = coder.Decode(genotype);

        if (problem.Set.Has(coloring))
        {
            return genotype;
        }

        var graph = problem.Graph;
        var newColoring = coloring.DeepClone();

        while (true)
        {
            var maybeProblematicVertex = FindTheMostProblematicVertex(graph, newColoring);
            /* Vertex? maybeProblematicVertex = null;
            List<Vertex>? itsNeighbours = null;
            var maxConflictCount = 0;

            foreach (var vertex in graph.VertexList())
            {
                var conflictCount = 0;

                var neighbours = graph.VertexNeighbours(vertex).ToList();
				foreach (var neighbour in neighbours)
                {
                    if (coloring[vertex] == coloring[neighbour])
                    {
                        conflictCount++;
                    }
                }

                if (conflictCount > maxConflictCount)
                {
                    maybeProblematicVertex = vertex;
                    itsNeighbours = neighbours;
					maxConflictCount = conflictCount;
                }
            }*/

            if (maybeProblematicVertex is null) break;

            var problematicVertex = maybeProblematicVertex.Value;
            // newColoring[problematicVertex] = SmallestPossibleColor(graph, problematicVertex, itsNeighbours!, newColoring);
			newColoring[problematicVertex] = SmallestPossibleColor(graph, problematicVertex, newColoring);
        }

        return coder.Encode(newColoring);
    }

    /// <summary>
    /// Находит вершину, у которой наиболее число конфликтов в цветах с соседями
    /// </summary>
    /// <param name="graph"></param>
    /// <param name="coloring"></param>
    /// <returns></returns>
    private static Vertex? FindTheMostProblematicVertex(IReadonlyGraph graph, GraphColoring coloring)
    {
        Vertex? problematicVertex = null;
        var maxConflictCount = 0u;

        foreach (var vertex in graph.VertexList())
        {
            var currentConflictCount = GetConflictCount(graph, vertex, coloring);
            if (currentConflictCount == 0) continue;

            if (currentConflictCount > maxConflictCount)
            {
                problematicVertex = vertex;
                maxConflictCount = currentConflictCount;
            }
        }

        return problematicVertex;
    }

    private static uint GetConflictCount(IReadonlyGraph graph, Vertex vertex, GraphColoring coloring) =>
        (uint)graph
            .VertexNeighbours(vertex)
            .Select(neighbour => coloring[neighbour])
            .Distinct()
            .Where(maybeColor => maybeColor.HasValue)
            .Where(color => color!.Value == coloring[vertex])
            .Count();

    //   private static Color SmallestPossibleColor(IReadonlyGraph graph, Vertex vertex, List<Vertex> neighbours, GraphColoring coloring)
    //{
    //       var neighbourColors = neighbours.Select(neighbour => coloring[neighbour]).Distinct();
    //	var smallestColor = Color.Smallest();

    //	foreach (var color in neighbourColors)
    //	{
    //		if (smallestColor != color) break;
    //		smallestColor = smallestColor.Next();
    //	}

    //	return smallestColor;
    //}

    private static Color SmallestPossibleColor(IReadonlyGraph graph, Vertex vertex, GraphColoring coloring)
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
