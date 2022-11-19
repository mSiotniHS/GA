using System.Collections.Generic;
using EA.BaseProblem;
using EA.Core;

namespace EA.CommonModules;

public sealed class RandomValidPopulationGenerator<TBase, TGaProblem> : IPopulationGenerator
	where TGaProblem : IGaProblem<TBase>, IRandomSolutionGenerator<TBase>
{
	private readonly TGaProblem _baseProblem;

	public RandomValidPopulationGenerator(TGaProblem baseProblem)
	{
		_baseProblem = baseProblem;
	}

	public IEnumerable<Genotype> Generate(int count)
	{
		for (var i = 0; i < count; i++)
		{
			yield return _baseProblem.Coder.Encode(_baseProblem.PickRandom());
		}
	}
}
