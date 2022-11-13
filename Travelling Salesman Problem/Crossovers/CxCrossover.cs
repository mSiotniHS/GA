using System;
using System.Collections.Generic;
using System.Linq;
using EA.Core;

namespace Travelling_Salesman_Problem.Crossovers;

public sealed class CxCrossover : ICrossover
{
	public List<Genotype> Perform(Genotype parent1, Genotype parent2)
	{
		var (numberedCycles, cycleCount)
			= NumberCycles(parent1.ToFilledArray(), parent2.ToFilledArray());

		if (cycleCount <= 1)
		{
			return new List<Genotype>();
		}

		// TODO если можно подобные родителям, то убрать -2; в цикле начинать с 0
		var totalChildAmount = Convert.ToInt32(Math.Pow(2, cycleCount)) - 2;

		var children = new List<Genotype>(totalChildAmount);

		for (var state = 1; state <= totalChildAmount; state++)
		{
			var child = new Genotype(parent1.Length);

			for (var i = 0; i < parent1.Length; i++)
			{
				var cycleNumber = numberedCycles[i];
				if (cycleNumber != -2)
				{
					var useSecondParent = GetNthBit(state, cycleNumber);
					child[i] = useSecondParent ? parent2[i] : parent1[i];
				}
				else
				{
					child[i] = parent1[i];
				}
			}

			children.Add(child);
		}

		return children;
	}

	public bool GuaranteesValidGenotype => true;

	private static (int[], int) NumberCycles(int[] preimages, IReadOnlyList<int> images)
	{
		var numberedCycles = Enumerable.Repeat(-1, preimages.Length).ToArray();

		var cycleCount = 0;
		while (true)
		{
			var startIdx = Array.FindIndex(numberedCycles, x => x == -1);
			if (startIdx == -1) break;

			bool? is1Cycle = null;

			var iterationIdx = startIdx;
			do
			{
				var nextIdx = Array.IndexOf(preimages, images[iterationIdx]);

				// определяем, является ли цикл длины 1
				if (is1Cycle is null)
				{
					if (nextIdx != iterationIdx)
					{
						is1Cycle = false;
					}
					else
					{
						is1Cycle = true;
						numberedCycles[iterationIdx] = -2;
						break;
					}
				}

				numberedCycles[iterationIdx] = cycleCount;
				iterationIdx = nextIdx;
			} while (iterationIdx != startIdx);

			if (!is1Cycle.Value)
			{
				cycleCount++;
			}
		}

		return (numberedCycles, cycleCount);
	}

	// n-й справа бит
	private static bool GetNthBit(int number, int n) => (number & (1 << n)) != 0;
}