using System.Collections.Generic;
using EA.BaseProblem;
using EA.Core;

namespace EA.CommonModules;

public sealed class RandomValidPopulationGenerator<TBaseType, TBaseProblem> : IPopulationGenerator
	where TBaseProblem : IGaProblem<TBaseType>, IRandomSolutionGenerator<TBaseType>
{
	private readonly TBaseProblem _baseProblem;

	public RandomValidPopulationGenerator(TBaseProblem baseProblem)
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
