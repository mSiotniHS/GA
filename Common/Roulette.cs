using System;
using System.Collections.Generic;

namespace Common;

public static class Roulette
{
	public static T Spin<T>(IList<T> items, IList<double> weights)
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

		var randomNum = Randomness.GetDouble() * totalWeight;
		var currentSector = 0.0;

		for (var i = 0; i < items.Count; i++)
		{
			currentSector += weights[i];
			if (randomNum <= currentSector)
			{
				return items[i];
			}
		}

		throw new Exception($"[{nameof(Roulette)}/{nameof(Spin)}] Случайное число оказалось вне отрезка");
	}
}
