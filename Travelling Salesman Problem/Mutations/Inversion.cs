using Common;
using EA.Core;

namespace Travelling_Salesman_Problem.Mutations;

public sealed class Inversion : IMutation
{
	public Genotype Perform(Genotype genotype)
	{
		// [startIdx, endIdx)
		var startIdx = Randomness.GetInt(genotype.Length - 1); // -1, чтобы хотя бы 2 гена было
		var endIdx = Randomness.GetInt(startIdx + 1, genotype.Length + 1);

		var newGenotype = new Genotype(genotype);

		var length = endIdx - startIdx;
		var halfLength = length / 2;
		for (var i = 0; i < halfLength; i++)
		{
			var left = i + startIdx;
			var right = endIdx - i - 1;
			(newGenotype[left], newGenotype[right]) = (newGenotype[right], newGenotype[left]);
		}

		return newGenotype;
	}

	public bool GuaranteesValidGenotype => true;
}
