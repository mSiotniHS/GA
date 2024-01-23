using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Common;
using GA.Helpers;

namespace GA.Core;

public sealed class GaCore<TNumber> : IGaIteration
	where TNumber : INumber<TNumber>
{
	private static readonly bool[] Items = [true, false];
	private readonly double[] _mutationWeights;
	private readonly double[] _crossoverWeights;
	private readonly uint _newcomerCount;

	private readonly IRng _rng;

	private readonly ICrossover _crossover;
	private readonly IMutation _mutation;
	private readonly ISelection<TNumber> _selection;

	private readonly PhenotypeCalculator<TNumber> _phenotype;

	public GaParameters Parameters { get; }

	public GaCore(
		IRng rng,
		GaParameters parameters,
		ICrossover crossover,
		IMutation mutation,
		ISelection<TNumber> selection,
		PhenotypeCalculator<TNumber> phenotype)
	{
		_rng = rng;

		Parameters = parameters;
		_crossover = crossover;
		_mutation = mutation;
		_selection = selection;
		_phenotype = phenotype;

		_mutationWeights = [Parameters.MutationRate, 1 - Parameters.MutationRate];
		_crossoverWeights = [Parameters.CrossoverRate, 1 - Parameters.CrossoverRate];

		_newcomerCount = (uint) (Parameters.PopulationSize * Parameters.GenerationalOverlapRatio);
	}

	public List<Genotype> PerformIteration(List<Genotype> population, List<(Genotype, Genotype)> parents)
	{
		// Logger.Begin(nameof(GaCore), nameof(PerformIteration));
		Logger.Log($"Получили популяцию:\n{string.Join('\n', population)}");
		Logger.Log($"Получили пары родителей:\n{string.Join('\n', parents)}");

		Logger.Log("Совершаем кроссовер");
		var children = PerformCrossover(parents);
		Logger.Log($"Получили детей:\n{string.Join('\n', children)}");

		if (children.Count == 0)
		{
			Logger.Log("Детей не оказалось");
			return population;
		}

		Logger.Log("Проводим мутацию");
		var mutants = Mutate(children);
		Logger.Log($"Получили мутантов:\n{string.Join('\n', mutants)}");

		var reproductionSet = new List<Genotype>(children.Count + mutants.Count);
		reproductionSet.AddRange(children);
		reproductionSet.AddRange(mutants);

		Logger.Log($"Получили репродукционное множество:\n{string.Join('\n', reproductionSet)}");
		Logger.Log("Переход к селекции");
		Logger.End();

		return SelectAndSwap(population, reproductionSet);
	}

	private List<Genotype> PerformCrossover(List<(Genotype, Genotype)> parents)
	{
		// Logger.Begin(nameof(GaCore), nameof(PerformCrossover));
		Logger.Log($"Получили пары родителей:\n{string.Join('\n', parents)}");

		// var children = new List<Genotype>();
		//foreach (var parentPair in parents)
		//{
		//	var toGiveBirth = Roulette.Spin(_rng, Items, _crossoverWeights);
		//	Logger.Log($"Будет ли ребенок? {toGiveBirth}");
		//	if (!toGiveBirth) continue;

		//	var (parent1, parent2) = parentPair;
		//	Logger.Log("Проводим кроссовер");
		//	children.AddRange(_crossover.Perform(parent1, parent2));
		//}

		var children = new ConcurrentBag<Genotype>();
		Parallel.ForEach(parents, pair =>
		{
			var toGiveBirth = Roulette.Spin(_rng, Items, _crossoverWeights);
			Logger.Log($"Будет ли ребенок? {toGiveBirth}");
			if (!toGiveBirth) return;

			var (parent1, parent2) = pair;
			Logger.Log("Проводим кроссовер");
			foreach (var child in _crossover.Perform(parent1, parent2))
			{
				children.Add(child);
			}
		});

		Logger.Log($"Провели кроссовер, получили детей:\n{string.Join('\n', children)}");
		Logger.End();

		return [.. children];
	}

	private List<Genotype> Mutate(List<Genotype> children)
	{
		// Logger.Begin(nameof(GaCore), nameof(Mutate));
		Logger.Log($"Получили детей:\n{string.Join('\n', children)}");

		//var mutants = new List<Genotype>();
		//foreach (var child in children)
		//{
		//	var toMutate = Roulette.Spin(_rng, Items, _mutationWeights);
		//	Logger.Log($"Произойдёт ли мутация? {toMutate}");
		//	if (!toMutate) continue;

		//	Logger.Log("Проводим мутацию");
		//	mutants.Add(_mutation.Perform(child));
		//}

		var mutants = new ConcurrentBag<Genotype>();
		Parallel.ForEach(children, child =>
		{
			var toMutate = Roulette.Spin(_rng, Items, _mutationWeights);
			if (!toMutate) return;

			mutants.Add(_mutation.Perform(child));
		});

		Logger.Log($"Провели мутацию, получили мутантов:\n{string.Join('\n', mutants)}");
		Logger.End();

		return [.. mutants];
	}

	private List<Genotype> SelectAndSwap(List<Genotype> population, List<Genotype> fund)
	{
		// Logger.Begin(nameof(GaCore), nameof(SelectAndSwap));

		//Logger.Log($"Получили популяцию:\n{
		//	string.Join('\n', population.Zip(population.Select(_phenotype), (genotype, phen) => $"*) {genotype} - {phen}"))
		//}");
		//Logger.Log($"Получили фонд:\n{
		//	string.Join('\n', fund.Zip(fund.Select(_phenotype), (genotype, phen) => $"*) {genotype} - {phen}"))
		//}");

		if (fund.Count == 0)
		{
			return population;
		}

		var newPopulation = new List<Genotype>(population.Count);
		var toBeSaved = population.Select(x => new Genotype(x)).ToList();

		var newcomerCount = (uint) Math.Min(_newcomerCount, fund.Count);

		List<Genotype> survivors;
		if (Parameters.UseElitistStrategy)
		{
			Logger.Log("Используем элитарную стратегию");

			var bestParent = Services.FindBest(toBeSaved, _phenotype);
			var bestChild = Services.FindBest(fund, _phenotype);

			Logger.Log($"Лучший родитель: {bestParent}");
			Logger.Log($"Лучший потомок: {bestChild}");

			if (_phenotype(bestChild) <= _phenotype(bestParent))
			{
				Logger.Log("Приспособленность ребёнка лучше, чем у родителя");

				newPopulation.Add(bestChild);
				fund.Remove(bestChild);

				Logger.Log($"Проводим селекцию, всего должно быть {newcomerCount - 1} выживших");
				survivors = _selection.Perform(fund, _phenotype, newcomerCount - 1).ToList();
			}
			else
			{
				Logger.Log("Приспособленность родителя лучше, чем у ребёнка");

				newPopulation.Add(bestParent);
				toBeSaved.Remove(bestParent);

				Logger.Log($"Проводим селекцию, всего должно быть {newcomerCount} выживших");
				survivors = _selection.Perform(fund, _phenotype, newcomerCount).ToList();
			}
		}
		else
		{
			Logger.Log("Не используем элитарную стратегию");
			Logger.Log("Проводим селекцию");

			survivors = _selection.Perform(fund, _phenotype, newcomerCount).ToList();
		}
		Logger.Log($"Получили выживших:\n{string.Join('\n', survivors)}");

		Logger.Log("Рандомно убираем родителей");
		for (var i = 0; i < newcomerCount; i++)
		{
			toBeSaved.RemoveAt(_rng.GetInt(toBeSaved.Count));
		}

		newPopulation.AddRange(toBeSaved);
		newPopulation.AddRange(survivors);

		Logger.Log($"Получили новую популяцию:\n{string.Join('\n', newPopulation)}");
		Logger.End();

		return newPopulation;
	}
}
