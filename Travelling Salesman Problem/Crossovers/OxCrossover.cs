using System.Collections.Generic;
using Common;
using EA.Core;

namespace Travelling_Salesman_Problem.Crossovers;

public sealed class OxCrossover : ICrossover
{
	private readonly IRng _rng;

	public OxCrossover(IRng rng)
	{
		_rng = rng;
	}

	public List<Genotype> Perform(Genotype parent1, Genotype parent2)
	{
		Logger.Begin(nameof(OxCrossover), nameof(Perform));
		Logger.Log($"Получили родителей:\n{parent1} и {parent2}");

		// [point1, point2)
		var point1 = _rng.GetInt(parent1.Length); // in [0, parent1.Length - 1]
		var point2 = _rng.GetInt(point1 + 1, parent1.Length + 1); // in [point1 + 1, parent1.Length]

		Logger.Log($"Точки кроссовера: {point1} и {point2}");

		var child1 = Base(parent1, parent2, point1, point2);
		var child2 = Base(parent2, parent1, point1, point2);

		Logger.Log($"Получили детей:\n{child1} и {child2}");
		Logger.End();

		return new List<Genotype> {child1, child2};
	}

	// interval [point1, point2)
	private static Genotype Base(Genotype parent1, Genotype parent2, int point1, int point2)
	{
		Logger.Begin(nameof(OxCrossover), nameof(Base));
		Logger.Log($"Получили родителей:\n{parent1} (родитель 1)\n{parent2} (родитель 2)");
		Logger.Log($"Точки кроссовера: {point1} и {point2}");

		var child = parent1.Extract(point1, point2);
		Logger.Log($"Извлекаем участок из родителя 1:\n{child}");

		var childIdx = point2 % parent1.Length;
		var parent2Idx = point2 % parent1.Length;

		int Inc(int idx) => (idx + 1) % parent1.Length;

		while (childIdx != point1)
		{
			Logger.Log($"childIdx = {childIdx}, parent2Idx = {parent2Idx}");

			var gene = parent2.GetNonNull(parent2Idx);
			Logger.Log($"Ген родителя 2 под индексом {parent2Idx}: {gene}");

			if (!child.ContainsIn(gene, point1, point2))
			{
				Logger.Log($"Такой ген не содержится в скопированном участке. Записываем в индекс {childIdx}");
				child[childIdx] = gene;
				childIdx = Inc(childIdx);
			}

			parent2Idx = Inc(parent2Idx);
		}

		Logger.Log($"Получили ребёнка: {child}");
		Logger.End();

		return child;
	}
}
