using Common;
using EA.Core;

namespace Travelling_Salesman_Problem.Mutations;

public sealed class PointMutation : IMutation
{
	public Genotype Perform(Genotype genotype)
	{
		var startIdx = Randomness.GetInt(genotype.Length - 1);

		var newGenotype = new Genotype(genotype);
		(newGenotype[startIdx], newGenotype[startIdx + 1]) = (newGenotype[startIdx + 1], newGenotype[startIdx]);

		return newGenotype;
	}

	public bool GuaranteesValidGenotype => true;
}