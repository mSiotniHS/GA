using System.Collections.Generic;
using Common;
using EA.Core;

namespace Travelling_Salesman_Problem.Crossovers;

public sealed class OxCrossover : ICrossover
{
	public List<Genotype> Perform(Genotype parent1, Genotype parent2)
	{
		// [point1, point2)
		var point1 = Randomness.GetInt(parent1.Length); // in [0, parent1.Length - 1]
		var point2 = Randomness.GetInt(point1 + 1, parent1.Length + 1); // in [point1 + 1, parent1.Length]

		var child1 = Base(parent1, parent2, point1, point2);
		var child2 = Base(parent2, parent1, point1, point2);

		return new List<Genotype> {child1, child2};
	}

	// interval [point1, point2)
	private static Genotype Base(Genotype parent1, Genotype parent2, int point1, int point2)
	{
		var child = parent1.Copy(point1, point2);

		var childIdx = point2 % parent1.Length;
		var parent2Idx = point2 % parent1.Length;

		int Inc(int idx) => (idx + 1) % parent1.Length;

		while (childIdx != point1)
		{
			var gene = parent2.GetNonNull(parent2Idx);

			if (!child.ContainsIn(gene, point1, point2))
			{
				child[childIdx] = gene;
				childIdx = Inc(childIdx);
			}

			parent2Idx = Inc(parent2Idx);
		}

		return child;
	}

	public bool GuaranteesValidGenotype => true;
}