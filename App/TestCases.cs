using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EA;
using EA.CommonModules;
using EA.Core;
using EA.Helpers;
using Travelling_Salesman_Problem;
using Travelling_Salesman_Problem.Crossovers;
using Travelling_Salesman_Problem.Mutations;
using Travelling_Salesman_Problem.Solvers;

namespace App;

using Route = List<int>;

public class TestCases
{
	private readonly Tsp _tsp;

	private const int POPULATION_SIZE = 6;
	private const double CROSSOVER_RATE = 0.95;
	private const double MUTATION_RATE = 0.05;
	private const double GENERATIONAL_OVERLAP_RATIO = 0.5;
	private const bool DONT_USE_ELITISM = false;

	public TestCases()
	{
		var (cityCount, rawDistances) = FromFile();
		var distances = new DistanceMatrix(cityCount, rawDistances.ToArray());

		_tsp = new Tsp(distances);
	}

	/// Настройка 1:
	/// - Параметы:
	/// -- Размер популяции              : 6
	/// -- Шанс кроссовера               : 95%
	/// -- Шанс мутации                  : 5%
	/// -- Процент замен при селекции    : 50%
	/// -- Элитарная стратегия           : false
	///
	/// - Модули
	/// -- Генерация начальной популяции : на основе рандомизированного жадного алгоритма
	/// -- Выбор пар родителей           : случайно
	/// -- Кроссовер                     : ox
	/// -- Мутация                       : точечная
	/// -- Селекция                      : β-турнир с параметром 2
	///
	/// - Завершение работы              : при достижении 10 поколения
	/// - Статистика                     : каждый, макс. 5
	public void Test1(int runCount)
	{
		var ga = new GaManager<Route>(
			_tsp,
			new GaParameters(
				PopulationSize: POPULATION_SIZE,
				CrossoverRate: CROSSOVER_RATE,
				MutationRate: MUTATION_RATE,
				GenerationalOverlapRatio: GENERATIONAL_OVERLAP_RATIO,
				UseElitistStrategy: DONT_USE_ELITISM),
			new GaModules(
				PopulationGenerator: new ProblemSolverPopulationGenerator<Route, Tsp>(
					_tsp,
					new RandomizedClosestNeighbourMethod()),
				PairSelector: new RandomPairSelector(),
				Crossover: new OxCrossover(),
				Mutation: new PointMutation(),
				Selection: new BetaTournament(2)),
			new GenerationCountEvaluator<Route>(10),
			new StatisticsCommittee(1, 5)
		);

		var bests = new List<Genotype>();
		for (var i = 0; i < runCount; i++)
		{
			bests.Add(ga.FindBestGenotype());
		}

		Console.WriteLine($"Статистика теста 1 за {runCount} запусков:");
		Console.WriteLine($"*) Лучший из лучших: {Services.FindBest(bests, ga.Phenotype)}");
		Console.WriteLine($"*) Среднее значение приспособленности: {bests.Average(g => ga.Phenotype(g))}");
		Console.WriteLine($"*) Лучшая приспособленность: {bests.Min(g => ga.Phenotype(g))}");
		Console.WriteLine($"*) Худшая приспособленность: {bests.Max(g => ga.Phenotype(g))}");
	}

	/// Настройка 2:
	/// - Параметы:
	/// -- Размер популяции              : 6
	/// -- Шанс кроссовера               : 95%
	/// -- Шанс мутации                  : 5%
	/// -- Процент замен при селекции    : 50%
	/// -- Элитарная стратегия           : true
	///
	/// - Модули
	/// -- Генерация начальной популяции : на основе рандомизированного жадного алгоритма
	/// -- Выбор пар родителей           : случайно
	/// -- Кроссовер                     : ox
	/// -- Мутация                       : точечная
	/// -- Селекция                      : β-турнир с параметром 2
	///
	/// - Завершение работы              : при достижении 10 поколения
	/// - Статистика                     : каждый, макс. 5
	public void Test2(uint runCount)
	{
		var ga = new GaManager<Route>(
			_tsp,
			new GaParameters(
				PopulationSize: POPULATION_SIZE,
				CrossoverRate: CROSSOVER_RATE,
				MutationRate: MUTATION_RATE,
				GenerationalOverlapRatio: GENERATIONAL_OVERLAP_RATIO,
				UseElitistStrategy: true),
			new GaModules(
				new ProblemSolverPopulationGenerator<Route, Tsp>(
					_tsp,
					new RandomizedClosestNeighbourMethod()),
				new RandomPairSelector(),
				new OxCrossover(),
				new PointMutation(),
				new BetaTournament(2)),
			new GenerationCountEvaluator<Route>(10),
			new StatisticsCommittee(1, 5)
		);

		var bests = new List<Genotype>();
		for (var i = 0; i < runCount; i++)
		{
			bests.Add(ga.FindBestGenotype());
		}

		Console.WriteLine($"Статистика теста 2 за {runCount} запусков:");
		Console.WriteLine($"*) Лучший из лучших: {Services.FindBest(bests, ga.Phenotype)}");
		Console.WriteLine($"*) Среднее значение приспособленности: {bests.Average(g => ga.Phenotype(g))}");
		Console.WriteLine($"*) Лучшая приспособленность: {bests.Min(g => ga.Phenotype(g))}");
		Console.WriteLine($"*) Худшая приспособленность: {bests.Max(g => ga.Phenotype(g))}");
	}

	/// Настройка 3:
	/// - Параметы:
	/// -- Размер популяции              : 6
	/// -- Шанс кроссовера               : 50%
	/// -- Шанс мутации                  : 50%
	/// -- Процент замен при селекции    : 50%
	/// -- Элитарная стратегия           : false
	///
	/// - Модули
	/// -- Генерация начальной популяции : на основе рандомизированного жадного алгоритма
	/// -- Выбор пар родителей           : случайно
	/// -- Кроссовер                     : ox
	/// -- Мутация                       : точечная
	/// -- Селекция                      : β-турнир с параметром 2
	///
	/// - Завершение работы              : при достижении 10 поколения
	/// - Статистика                     : каждый, макс. 5
	public void Test3(uint runCount)
	{
		var ga = new GaManager<Route>(
			_tsp,
			new GaParameters(
				PopulationSize: POPULATION_SIZE,
				CrossoverRate: .5,
				MutationRate: .5,
				GenerationalOverlapRatio: GENERATIONAL_OVERLAP_RATIO,
				UseElitistStrategy: DONT_USE_ELITISM),
			new GaModules(
				new ProblemSolverPopulationGenerator<Route, Tsp>(
					_tsp,
					new RandomizedClosestNeighbourMethod()),
				new RandomPairSelector(),
				new OxCrossover(),
				new PointMutation(),
				new BetaTournament(2)),
			new GenerationCountEvaluator<Route>(10),
			new StatisticsCommittee(1, 5)
		);

		var bests = new List<Genotype>();
		for (var i = 0; i < runCount; i++)
		{
			bests.Add(ga.FindBestGenotype());
		}

		Console.WriteLine($"Статистика теста 3 за {runCount} запусков:");
		Console.WriteLine($"*) Лучший из лучших: {Services.FindBest(bests, ga.Phenotype)}");
		Console.WriteLine($"*) Среднее значение приспособленности: {bests.Average(g => ga.Phenotype(g))}");
		Console.WriteLine($"*) Лучшая приспособленность: {bests.Min(g => ga.Phenotype(g))}");
		Console.WriteLine($"*) Худшая приспособленность: {bests.Max(g => ga.Phenotype(g))}");
	}

	/// Настройка 4:
	/// - Параметы:
	/// -- Размер популяции              : 6
	/// -- Шанс кроссовера               : 50%
	/// -- Шанс мутации                  : 50%
	/// -- Процент замен при селекции    : 50%
	/// -- Элитарная стратегия           : false
	///
	/// - Модули
	/// -- Генерация начальной популяции : на основе рандомизированного жадного алгоритма
	/// -- Выбор пар родителей           : случайно
	/// -- Кроссовер                     : ox
	/// -- Мутация                       : точечная
	/// -- Селекция                      : β-турнир с параметром 2
	///
	/// - Завершение работы              : при достижении 10 поколения
	/// - Статистика                     : каждый, макс. 5
	public void Test4(uint runCount)
	{
		var ga = new GaManager<Route>(
			_tsp,
			new GaParameters(
				PopulationSize: POPULATION_SIZE,
				CrossoverRate: CROSSOVER_RATE,
				MutationRate: MUTATION_RATE,
				GenerationalOverlapRatio: GENERATIONAL_OVERLAP_RATIO,
				UseElitistStrategy: DONT_USE_ELITISM),
			new GaModules(
				new ProblemSolverPopulationGenerator<Route, Tsp>(
					_tsp,
					new RandomizedClosestNeighbourMethod()),
				new RandomPairSelector(),
				new CxCrossover(),
				new Inversion(),
				new BetaTournament(2)),
			new GenerationCountEvaluator<Route>(10),
			new StatisticsCommittee(0, 5)
		);

		var bests = new List<Genotype>();
		for (var i = 0; i < runCount; i++)
		{
			bests.Add(ga.FindBestGenotype());
		}

		Console.WriteLine($"Статистика теста 4 за {runCount} запусков:");
		Console.WriteLine($"*) Лучший из лучших: {Services.FindBest(bests, ga.Phenotype)}");
		Console.WriteLine($"*) Среднее значение приспособленности: {bests.Average(g => ga.Phenotype(g))}");
		Console.WriteLine($"*) Лучшая приспособленность: {bests.Min(g => ga.Phenotype(g))}");
		Console.WriteLine($"*) Худшая приспособленность: {bests.Max(g => ga.Phenotype(g))}");
	}

	private static (int, IEnumerable<int>) FromFile()
	{
		var lines = File.ReadLines("matrix.txt");

		var i = 0;
		var distances = new List<int>();
		foreach (var line in lines)
		{
			var split = line.Split(' ');
			for (var j = i + 1; j < split.Length; j++)
			{
				var successfulParse = int.TryParse(split[j], out var distance);
				if (!successfulParse) throw new Exception("Failed to parse distance");
				distances.Add(distance);
			}

			i++;
		}

		return (i, distances);
	}
}
