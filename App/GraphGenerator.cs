using Common;
using GraphColoring.Math;

namespace App;

public static class GraphGenerator
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="vertexCount"></param>
	/// <param name="density">значение из отрезка [0,1];
	///	вероятность, что между двумя вершинами будет ребро</param>
	/// <returns></returns>
	public static Graph GenerateGenericGraph(int vertexCount, double density)
	{
		var data = GenerateData(vertexCount, density);
		var matrix = new SymmetricMatrix<bool>(vertexCount, data, false);
		return new Graph(matrix);
	}

	public static Graph GenerateTree(int vertexCount)
	{
		var encoding = Utilities
			.Generate(
				vertexCount - 2,
				() => new Vertex(Random.Shared.Next(0, vertexCount)))
			.ToList();

		return DecodePruferSequence(encoding);
	}

	private static Graph DecodePruferSequence(IList<Vertex> encoding)
	{
		var vertexCount = encoding.Count + 2;
		var graph = Graph.Empty(vertexCount);
		var vertices = graph.VertexList().ToList();

		while (encoding.Count != 0)
		{
			var missingVertex = FindFirstMissingVertex(vertices, encoding);
			graph.AddEdge(missingVertex, encoding[0]);

			vertices.Remove(missingVertex);
			encoding.RemoveAt(0);
		}

		graph.AddEdge(vertices[0], vertices[1]);

		return graph;
	}

	private static Vertex FindFirstMissingVertex(List<Vertex> pool, ICollection<Vertex> encoding)
	{
		foreach (var vertex in pool)
		{
			if (encoding.Contains(vertex)) continue;

			return vertex;
		}

		throw new ArgumentException("Bad data");
	}

	public static bool[] GenerateData(int vertexCount, double density) =>
		Enumerable
			.Range(0, vertexCount * (vertexCount - 1) / 2)
			.Select(_ => Random.Shared.NextDouble() < density)
			.ToArray();
}
