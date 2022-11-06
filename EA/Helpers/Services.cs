using System;
using System.Collections.Generic;
using EA.Core;

namespace EA.Helpers;

internal static class Services
{
	public static Genotype FindBest(IEnumerable<Genotype> group, Func<Genotype, int> fitness)
	{
		Genotype best = null;
		foreach (var genotype in group)
		{
			if (best is null || fitness(genotype) > fitness(best))
			{
				best = genotype;
			}
		}

		return best ?? throw new Exception("can't be right");
	}
}
