using System.Collections.Generic;
using System.Linq;
using Common;
using EA.Core;

namespace EA.CommonModules;

public sealed class RandomPairSelector : IPairSelector
{
	public IEnumerable<(Genotype, Genotype)> Select(IEnumerable<Genotype> population)
	{
		var populationList = population.ToList();
		var pairs = new List<(Genotype, Genotype)>();

		while (populationList.Count != 0)
		{
			var firstIdx = Randomness.GetInt(populationList.Count);
			populationList.RemoveAt(firstIdx);
			var secondIdx = Randomness.GetInt(populationList.Count);
			populationList.RemoveAt(secondIdx);

			pairs.Add((populationList[firstIdx], populationList[secondIdx]));
		}

		return pairs;
	}
}
