using Common;
using GraphColoring.Math;
using GraphColoring.Solvers;

namespace App.InitialPopulation;

/// <summary>
/// В классическом LfOrdering вершины упорядочиваются в порядке невозрастания степени
/// вершины. Здесь рандомно, то тем выше шанс быть следующей вершиной, чем выше
/// её степень.
/// </summary>
/// <param name="rng"></param>
internal class RandomizedLfOrdering(IRng rng) : IOrderingStrategy
{
	public IEnumerable<Vertex> OrderVertices(IReadonlyGraph graph)
	{
		var vertices = graph.VertexList().ToList();
		var degrees = vertices.Select(graph.VertexDegree).Select(degree => (double) degree).ToList();

		while (vertices.Count > 0)
		{
			yield return Roulette.Spin(rng, vertices, degrees, out var idx);

			vertices.RemoveAt(idx);
			degrees.RemoveAt(idx);
		}
	}
}
