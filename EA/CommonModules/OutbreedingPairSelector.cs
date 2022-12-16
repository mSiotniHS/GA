using System.Collections.Generic;
using System.Linq;
using Common;
using EA.Core;
using EA.Upper;

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
		Logger.Begin(nameof(OutbreedingPairSelector), nameof(Select));

		var populationList = population.ToList();
		Logger.Log("Получили популяцию:\n" + string.Join('\n', populationList));

		var pairs = new List<(Genotype, Genotype)>();

		while (populationList.Count != 0)
		{
			const int firstIdx = 0;
			var firstParent = populationList[firstIdx];

			Logger.Log($"Ищем пару для {firstParent} среди популяции:\n" + string.Join('\n', populationList));

			var furthestIdx = -1;
			var furthestDistance = -1;

			for (var i = firstIdx + 1; i < populationList.Count; i++)
			{
				Logger.Log($"Наибольшее расстояние: {furthestDistance}");

				var candidate = populationList[i];
				Logger.Log($"Кандидат: {candidate}");

				var candidateDistance = firstParent.DistanceTo(candidate);
				Logger.Log($"Расстояние до кандидата: {candidateDistance}");

				if (candidateDistance > furthestDistance)
				{
					Logger.Log("Расстояние до кандидата больше известного, сохраняем");
					furthestIdx = i;
					furthestDistance = candidateDistance;

					if (furthestDistance >= _minDifference)
					{
						Logger.Log("Расстояние до кандитата больше, чем минимальное. Выходим из цикла");
						break;
					}
				}
			}

			pairs.Add((firstParent, populationList[furthestIdx]));

			Logger.Log($"Добавили пару: {firstParent} и {populationList[furthestIdx]}");
			Logger.Log($"Расстояние: {furthestDistance}");

			if (firstIdx > furthestIdx)
			{
				populationList.RemoveAt(firstIdx);
				populationList.RemoveAt(furthestIdx);
			}
			else
			{
				populationList.RemoveAt(furthestIdx);
				populationList.RemoveAt(firstIdx);
			}
		}

		Logger.Log($"Получили пары:\n{string.Join('\n', pairs)}");
		Logger.End();

		return pairs;
	}
}
