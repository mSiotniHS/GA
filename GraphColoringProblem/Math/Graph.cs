using System.Collections.Generic;
using System.Linq;

namespace GraphColoring.Math;

public sealed class Graph : IGraph
{
    private readonly SymmetricMatrix<bool> _adjacencyMatrix;

    public Graph(bool[,] adjacencyMatrix)
    {
        var rowCount = (int)System.Math.Round(System.Math.Sqrt(adjacencyMatrix.Length));
        var data = new List<bool>();

        for (var i = 0; i < rowCount; i++)
        {
            for (var j = i + 1; j < rowCount; j++)
            {
                data.Add(adjacencyMatrix[i, j]);
            }
        }

        _adjacencyMatrix = new SymmetricMatrix<bool>(rowCount, data.ToArray(), false);
    }

    public Graph(SymmetricMatrix<bool> adjacencyMatrix)
    {
        _adjacencyMatrix = adjacencyMatrix;
    }

    public static Graph Empty(int vertexCount)
    {
        var matrix = new bool[vertexCount, vertexCount];
        return new Graph(matrix);
    }

    public int VertexCount => _adjacencyMatrix.RowCount;

    public IEnumerable<Vertex> VertexList() =>
        Enumerable.Range(0, VertexCount).Select(idx => new Vertex(idx));

    public bool AreAdjacent(Vertex first, Vertex second) => _adjacencyMatrix[first.Id, second.Id];

    public int VertexDegree(Vertex v) =>
        VertexList().Count(vertex => AreAdjacent(v, vertex));

    public int VertexDegree(Vertex v, IEnumerable<Vertex> among) =>
        among.Count(vertex => AreAdjacent(v, vertex));

    public IEnumerable<Vertex> VertexNeighbours(Vertex v) =>
        VertexList().Where(vertex => AreAdjacent(vertex, v));

    public IEnumerable<Vertex> VertexNeighbours(Vertex v, IEnumerable<Vertex> among) =>
        among.Where(vertex => AreAdjacent(vertex, v));

    public void AddEdge(Vertex first, Vertex second) =>
        _adjacencyMatrix[first.Id, second.Id] = true;

    public void RemoveEdge(Vertex first, Vertex second) =>
        _adjacencyMatrix[first.Id, second.Id] = false;

    public override string ToString() =>
        string.Join(' ', _adjacencyMatrix.RawData.Select(x => x ? '1' : '0'));
}

