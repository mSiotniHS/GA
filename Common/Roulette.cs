using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Common;

public static class Roulette
{
	public static T Spin<T>(IRng rng, IList<T> items, IList<double> weights, out int idx)
	{
		if (items.Count != weights.Count)
		{
			throw new ArgumentException($"[{nameof(Roulette)}/{nameof(Spin)}] Числа предметов и весов должны совпадать");
		}

		var totalWeight = 0.0;
		for (var i = 0; i < weights.Count; i++)
		{
			totalWeight += weights[i];
		}

		var randomNum = rng.GetDouble() * totalWeight;
		var currentSector = 0.0;

		for (var i = 0; i < items.Count; i++)
		{
			currentSector += weights[i];
			if (randomNum <= currentSector)
			{
				idx = i;
				return items[i];
			}
		}

		throw new UnreachableException($"[{nameof(Roulette)}/{nameof(Spin)}] Случайное число оказалось вне отрезка");
	}

	public static T Spin<T>(IRng rng, IList<T> items, IList<double> weights)
	{
		return Spin(rng, items, weights, out _);
	}
}
