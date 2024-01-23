using System.Collections.Generic;

namespace GraphColoring.Math;

public readonly record struct Vertex(int Id);

public interface IReadonlyGraph
{
    int VertexCount { get; }
    IEnumerable<Vertex> VertexList();
    bool AreAdjacent(Vertex first, Vertex second);
    int VertexDegree(Vertex v);
    int VertexDegree(Vertex v, IEnumerable<Vertex> among);
    IEnumerable<Vertex> VertexNeighbours(Vertex v);
    IEnumerable<Vertex> VertexNeighbours(Vertex v, IEnumerable<Vertex> among);
}

public interface IGraph : IReadonlyGraph
{
    void AddEdge(Vertex first, Vertex second);
    void RemoveEdge(Vertex first, Vertex second);
}
