using GA.Core;

namespace App.Mutations;

internal class FullMutation : IMutation
{
	public Genotype Perform(Genotype genotype)
	{
		var mutant = new Genotype(genotype);

		for (var i = 0; i < mutant.Length; i++)
		{
			mutant[i] = (mutant.Length - 1) - mutant[i];
		}

		return mutant;
	}
}
