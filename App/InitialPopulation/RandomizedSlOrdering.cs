using Common;
using GraphColoring.Math;
using GraphColoring.Solvers;

namespace App.InitialPopulation;

internal class RandomizedSlOrdering(IRng rng) : IOrderingStrategy
{
	public IEnumerable<Vertex> OrderVertices(IReadonlyGraph graph)
	{
		var ordering = new List<Vertex>();
		var vertices = graph.VertexList().ToList();

		while (ordering.Count != graph.VertexCount)
		{
			var weights = vertices
				.Select(vertex => (double) graph.VertexDegree(vertex, vertices))
				.Select(degree => 1 / degree)
				.ToList();
			var nextVertex = Roulette.Spin(rng, vertices, weights);

			ordering.Add(nextVertex);
			vertices.Remove(nextVertex);
		}

		ordering.Reverse();
		return ordering;
	}
}
