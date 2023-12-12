using System.Collections.Generic;
using GA.BaseProblem;
using GA.Core;
using GA.Upper;

namespace GA.CommonModules.PopulationGenerators;

public sealed class RandomValidPopulationGenerator<TBaseType, TBaseProblem> : IPopulationGenerator
	where TBaseProblem : IGaOptimizationProblem<TBaseType>, IRandomSolutionGenerator<TBaseType>
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
