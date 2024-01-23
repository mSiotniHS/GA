using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using GA.Core;

namespace GA.Helpers;

public static class Services
{
	public static Genotype FindBest<TNumber>(IEnumerable<Genotype> group, PhenotypeCalculator<TNumber> fitnessFunction)
		where TNumber : INumber<TNumber> =>
		FindBest(group, fitnessFunction, out _);

	public static Genotype FindBest<TNumber>(IEnumerable<Genotype> group, PhenotypeCalculator<TNumber> fitnessFunction, out TNumber fitness)
		where TNumber : INumber<TNumber>
	{
		Genotype? best = null;
		var bestFitness = TNumber.Zero;

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
