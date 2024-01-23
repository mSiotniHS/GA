using GraphColoring.Math;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphColoring.Solvers;

/// <summary>
/// Сортирует вершины в порядке невозрастания степени вершины
/// </summary>
public sealed class LfOrdering : IOrderingStrategy
{
    public IEnumerable<Vertex> OrderVertices(IReadonlyGraph graph) =>
        graph
            .VertexList()
            .OrderBy(graph.VertexDegree)
            .Reverse();
}
