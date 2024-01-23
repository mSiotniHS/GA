using Common;
using GA.Core;

namespace App.Mutations;

internal class SwappingMutation(IRng rng) : IMutation
{
	public Genotype Perform(Genotype genotype)
	{
		var mutant = new Genotype(genotype);

		var first = rng.GetInt(genotype.Length);
		var second = rng.GetInt(genotype.Length - 1);

		if (second < first)
		{
			(mutant[first], mutant[second]) = (mutant[second], mutant[first]);
		}
		else
		{
			(mutant[first], mutant[second + 1]) = (mutant[second + 1], mutant[first]);
		}

		return mutant;
	}
}
