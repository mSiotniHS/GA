using GraphColoring.Math;
using System.Collections.Generic;
using System.Linq;

namespace GraphColoring.Solvers;

public sealed class SlOrdering : IOrderingStrategy
{
    public IEnumerable<Vertex> OrderVertices(IReadonlyGraph graph)
    {
        var ordering = new List<Vertex>();
        var vertices = graph.VertexList().ToList();

        while (ordering.Count != graph.VertexCount)
        {
            var smallestDegreeVertex = vertices.MinBy(vertex => graph.VertexDegree(vertex, vertices));
            ordering.Add(smallestDegreeVertex);
            vertices.Remove(smallestDegreeVertex);
        }

        ordering.Reverse();
        return ordering;
    }
}
