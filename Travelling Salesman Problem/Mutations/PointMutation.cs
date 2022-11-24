using Common;
using EA.Core;

namespace Travelling_Salesman_Problem.Mutations;

public sealed class PointMutation : IMutation
{
	private readonly IRng _rng;

	public PointMutation(IRng rng)
	{
		_rng = rng;
	}

	public Genotype Perform(Genotype genotype)
	{
		var startIdx = _rng.GetInt(genotype.Length - 1);

		var newGenotype = new Genotype(genotype);
		(newGenotype[startIdx], newGenotype[startIdx + 1]) = (newGenotype[startIdx + 1], newGenotype[startIdx]);

		return newGenotype;
	}

	public bool GuaranteesValidGenotype => true;
}
