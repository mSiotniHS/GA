using System.Collections.Generic;
using System.Linq;
using EA.Core;

namespace EA.CommonModules;

public sealed class OutbreedingPairSelector : IPairSelector
{
	private readonly uint _minDifference;

	public OutbreedingPairSelector(uint minDifference)
	{
		_minDifference = minDifference;
	}

	public IEnumerable<(Genotype, Genotype)> Select(IEnumerable<Genotype> population)
	{
		var populationList = population.ToList();
		var pairs = new List<(Genotype, Genotype)>();

		while (populationList.Count != 0)
		{
			const int firstIdx = 0;
			var firstParent = populationList[firstIdx];

			var furthestIdx = -1;
			var furthestDistance = -1;

			for (var i = firstIdx + 1; i < populationList.Count; i++)
			{
				var candidate = populationList[i];
				var candidateDistance = firstParent.DistanceTo(candidate);

				if (candidateDistance > furthestDistance)
				{
					furthestIdx = i;
					furthestDistance = candidateDistance;

					if (furthestDistance >= _minDifference)
					{
						break;
					}
				}
			}

			pairs.Add((firstParent, populationList[furthestIdx]));
			populationList.RemoveAt(firstIdx);
			populationList.RemoveAt(furthestIdx);
		}

		return pairs;
	}
}
