using Common;
using EA.Core;

namespace Travelling_Salesman_Problem.Mutations;

public sealed class Saltation : IMutation
{
	public Genotype Perform(Genotype genotype)
	{
		var startIdx = Randomness.GetInt(genotype.Length - 1);
		var endIdx = Randomness.GetInt(startIdx + 1, genotype.Length);

		var newGenotype = new Genotype(genotype);
		(newGenotype[startIdx], newGenotype[endIdx]) = (newGenotype[endIdx], newGenotype[startIdx]);

		return newGenotype;
	}

	public bool GuaranteesValidGenotype => true;
}