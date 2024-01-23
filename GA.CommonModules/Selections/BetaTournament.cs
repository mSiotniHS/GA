using System;
using System.Collections.Generic;
using System.Numerics;
using Common;
using GA.Core;
using GA.Helpers;

namespace GA.CommonModules.Selections;

public sealed class BetaTournament<TNumber> : ISelection<TNumber>
	where TNumber : INumber<TNumber>
{
	private readonly uint _beta;
	private readonly IRng _rng;

	public BetaTournament(IRng rng, uint beta)
	{
		if (beta < 2)
		{
			throw new ArgumentOutOfRangeException(
				nameof(beta), $"[{nameof(BetaTournament<TNumber>)}/cons] Параметр Beta должен быть больше единицы");
		}

		_rng = rng;
		_beta = beta;
	}

	public IEnumerable<Genotype> Perform(List<Genotype> fund, PhenotypeCalculator<TNumber> phenotype, uint count)
	{
		for (var selectedCount = 0; selectedCount < count; selectedCount++)
		{
			yield return Round(fund, phenotype);
		}
	}

	private Genotype Round(IReadOnlyList<Genotype> fund, PhenotypeCalculator<TNumber> phenotype)
	{
		var fighters = new List<Genotype>();
		// не нужно ли добавить && i < fund.Count?
		for (var i = 0; i < _beta; i++)
		{
			fighters.Add(fund[_rng.GetInt(fund.Count)]);
		}

		return Services.FindBest(fighters, phenotype);
	}
}
