using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Common;
using GA.Core;

namespace GA.CommonModules.Selections;

public sealed class LinearRankScheme<TNumber> : ISelection<TNumber>
	where TNumber : INumber<TNumber>
{
	private readonly double _upperBound;
	private readonly double _lowerBound;
	private readonly ICopyStrategy _copyStrategy;

	public LinearRankScheme(IRng rng, ICopyStrategy copyStrategy)
	{
		// in (1, 2]
		_upperBound = 2 - rng.GetDouble();
		_lowerBound = 2 - _upperBound;

		_copyStrategy = copyStrategy;
	}

	public IEnumerable<Genotype> Perform(List<Genotype> fund, PhenotypeCalculator<TNumber> phenotype, uint count)
	{
		Logger.Begin(nameof(LinearRankScheme<TNumber>), nameof(Perform));
		Logger.Log($"Нужно выбрать {count} особей");
		Logger.Log($"Фонд:\n{string.Join('\n', fund)}");

		// первый --- наиболее приспособленный
		var sorted = fund.OrderBy(phenotype.Invoke).ToList();
		var weights = Enumerable
			.Range(0, fund.Count)
			.Select(i => ExpectedCopyCount(i, fund.Count))
			.ToList();

		Logger.End();

		return _copyStrategy.Copy(sorted, weights, count);
	}

	private static int Rank(int idx) => idx + 1;

	private double ExpectedCopyCount(int idx, int count)
		=> _lowerBound + (_upperBound - _lowerBound) * Rank(idx - 1) / (count - 1);
}
