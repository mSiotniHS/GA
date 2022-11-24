using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using EA.Core;

namespace EA.CommonModules;

public sealed class LinearRankScheme : ISelection
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

	public IEnumerable<Genotype> Perform(List<Genotype> fund, Func<Genotype, int> phenotype, uint count)
	{
		Logger.Begin(nameof(LinearRankScheme), nameof(Perform));
		Logger.Log($"Нужно выбрать {count} особей");
		Logger.Log($"Фонд:\n{string.Join('\n', fund)}");

		// первый --- наиболее приспособленный
		var sorted = fund.OrderBy(phenotype).ToList();
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
