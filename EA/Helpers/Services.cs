using System;
using System.Collections.Generic;
using System.Diagnostics;
using EA.Core;

namespace EA.Helpers;

public static class Services
{
	public static Genotype FindBest(IEnumerable<Genotype> group, Func<Genotype, int> fitnessFunction)
	{
		return FindBest(group, fitnessFunction, out _);
	}

	public static Genotype FindBest(IEnumerable<Genotype> group, Func<Genotype, int> fitnessFunction, out int fitness)
	{
		Genotype? best = null;
		var bestFitness = int.MaxValue;

		foreach (var genotype in group)
		{
			var currentFitness = fitnessFunction(genotype);
			if (best is null || currentFitness < bestFitness)
			{
				best = genotype;
				bestFitness = currentFitness;
			}
		}

		fitness = bestFitness;

		return best ?? throw new UnreachableException($"[{nameof(Services)}/{nameof(FindBest)}] Не удалось найти лучшего");
	}
}
