using Common;
using EA.Core;

namespace Travelling_Salesman_Problem.Mutations;

public sealed class Inversion : IMutation
{
	public Genotype Perform(Genotype genotype)
	{
		// [startIdx, endIdx)
		var startIdx = Randomness.GetInt(genotype.Length);
		var endIdx = Randomness.GetInt(startIdx + 1, genotype.Length + 1);

		var newGenotype = new Genotype(genotype);

		for (var i = startIdx; i < startIdx + (endIdx - startIdx) / 2; i++)
		{
			(newGenotype[i], newGenotype[endIdx - i]) = (newGenotype[endIdx - i], newGenotype[i]);
		}

		return newGenotype;
	}

	public bool GuaranteesValidGenotype => true;
}