using GraphColoring.Math;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphColoring.Solvers;

/// <summary>
/// Сортирует вершины случайным образом.
/// </summary>
public sealed class RsOrdering : IOrderingStrategy
{
    public IEnumerable<Vertex> OrderVertices(IReadonlyGraph graph) =>
        graph
            .VertexList()
            .OrderBy(_ => Random.Shared.Next());
}
