using GA.Core;
using System.Numerics;

namespace App;

internal class ModificationInfusedGaIteration<T>(IGaIteration iteration, IGenotypeModificator modificator, uint gap = 0) : IGaIteration
	where T : INumber<T>
{
	public GaParameters Parameters => iteration.Parameters;
	private uint _iterationsAfterApplication = 0;

	public List<Genotype> PerformIteration(List<Genotype> population, List<(Genotype, Genotype)> parents)
	{
		var @base = iteration.PerformIteration(population, parents);

		if (_iterationsAfterApplication == gap)
		{
			_iterationsAfterApplication = 0;
			return @base.Select(modificator.Modify).ToList();
		}
		else
		{
			_iterationsAfterApplication++;
			return @base;	
		}
	}
}
