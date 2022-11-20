using System.Collections.Generic;
using System.Linq;
using Common;
using EA.Core;

namespace EA.CommonModules;

public sealed class RandomPairSelector : IPairSelector
{
	public IEnumerable<(Genotype, Genotype)> Select(IEnumerable<Genotype> population)
	{
#if DEBUG
		Logger.Begin(nameof(RandomPairSelector), nameof(Select));
#endif

		var populationList = population.ToList();
		var pairs = new List<(Genotype, Genotype)>();

#if DEBUG
		Logger.Log($"Получили популяцию:\n{string.Join('\n', populationList)}");
#endif

		while (populationList.Count != 0)
		{
			var firstIdx = Randomness.GetInt(populationList.Count);
			var first = populationList[firstIdx];
			populationList.RemoveAt(firstIdx);

			var secondIdx = Randomness.GetInt(populationList.Count);
			var second = populationList[secondIdx];
			populationList.RemoveAt(secondIdx);

#if DEBUG
			Logger.Log($"Сформировали пару: {first} и {second}");
#endif

			pairs.Add((first, second));
		}

#if DEBUG
		Logger.End();
#endif

		return pairs;
	}
}
