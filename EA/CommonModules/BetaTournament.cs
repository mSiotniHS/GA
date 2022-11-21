using System;
using System.Collections.Generic;
using Common;
using EA.Core;
using EA.Helpers;

namespace EA.CommonModules;

public sealed class BetaTournament : ISelection
{
	private readonly uint _beta;

	public BetaTournament(uint beta)
	{
		if (beta < 2)
		{
			throw new ArgumentOutOfRangeException(
				nameof(beta), $"[{nameof(BetaTournament)}/cons] Параметр Beta должен быть больше единицы");
		}
		_beta = beta;
	}

	public IEnumerable<Genotype> Perform(List<Genotype> fund, Func<Genotype, int> phenotype, uint count)
	{
		for (var selectedCount = 0; selectedCount < count; selectedCount++)
		{
			yield return Round(fund, phenotype);
		}
	}

	private Genotype Round(IReadOnlyList<Genotype> fund, Func<Genotype, int> phenotype)
	{
		var fighters = new List<Genotype>();
		// не нужно ли добавить && i < fund.Count?
		for (var i = 0; i < _beta; i++)
		{
			fighters.Add(fund[Randomness.GetInt(fund.Count)]);
		}

		return Services.FindBest(fighters, phenotype);
	}
}
