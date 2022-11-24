using Common;
using EA.Core;

namespace Travelling_Salesman_Problem.Mutations;

public sealed class Saltation : IMutation
{
	private readonly IRng _rng;

	public Saltation(IRng rng)
	{
		_rng = rng;
	}

	public Genotype Perform(Genotype genotype)
	{
		var startIdx = _rng.GetInt(genotype.Length - 1);
		var endIdx = _rng.GetInt(startIdx + 1, genotype.Length);

		var newGenotype = new Genotype(genotype);
		(newGenotype[startIdx], newGenotype[endIdx]) = (newGenotype[endIdx], newGenotype[startIdx]);

		return newGenotype;
	}

	public bool GuaranteesValidGenotype => true;
}
