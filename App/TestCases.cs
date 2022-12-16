using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;
using EA.CommonModules;
using EA.Core;
using EA.Helpers;
using EA.Upper;
using Travelling_Salesman_Problem;
using Travelling_Salesman_Problem.Crossovers;
using Travelling_Salesman_Problem.Mutations;
using Travelling_Salesman_Problem.Solvers;

namespace App;

using Route = List<int>;

public sealed class TestCases
{
	private readonly Tsp _tsp;

	private const int POPULATION_SIZE = 6;
	private const double CROSSOVER_RATE = 0.95;
	private const double MUTATION_RATE = 0.05;
	private const double GENERATIONAL_OVERLAP_RATIO = 0.5;
	private const bool DONT_USE_ELITISM = false;
	private static readonly IEvaluationStrategy<Route> EVALUATION_STRATEGY = new GenerationCountEvaluator<Route>(5);
	// private static readonly IEvaluationStrategy<Route> EVALUATION_STRATEGY = new NoProgressEvaluator<Route>(5);

	private static readonly Rng Rng = new();

	public TestCases()
	{
		var (cityCount, rawDistances) = FromFile();
		var distances = new DistanceMatrix(cityCount, rawDistances.ToArray());

		_tsp = new Tsp(Rng, distances);
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
	/// - Завершение работы              : при достижении 5 поколения
	/// - Статистика                     : каждый, макс. 5
	public void Test1(int runCount)
	{
		var ga = new GaManager<Route>(
			Rng,
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
					new RandomizedClosestNeighbourMethod(Rng)),
				PairSelector: new RandomPairSelector(Rng),
				Crossover: new OxCrossover(Rng),
				Mutation: new PointMutation(Rng),
				Selection: new BetaTournament(Rng, 2)),
			EVALUATION_STRATEGY,
			new StatisticsCommittee(1, 5)
		);

		RunTest(ga, runCount, "Эталонный");
	}

	public void Test2(int runCount)
	{
		var ga = new GaManager<Route>(
			Rng,
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
					new RandomizedClosestNeighbourMethod(Rng)),
				PairSelector: new OutbreedingPairSelector(10),
				Crossover: new OxCrossover(Rng),
				Mutation: new PointMutation(Rng),
				Selection: new BetaTournament(Rng, 2)),
			EVALUATION_STRATEGY,
			new StatisticsCommittee(1, 5)
		);

		RunTest(ga, runCount, "Выбор пар");
	}

	public void Test3(int runCount)
	{
		var ga = new GaManager<Route>(
			Rng,
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
					new RandomizedClosestNeighbourMethod(Rng)),
				PairSelector: new RandomPairSelector(Rng),
				Crossover: new CxCrossover(),
				Mutation: new PointMutation(Rng),
				Selection: new BetaTournament(Rng, 2)),
			EVALUATION_STRATEGY,
			new StatisticsCommittee(1, 5)
		);

		RunTest(ga, runCount, "Кроссовер");
	}

	public void Test4(int runCount)
	{
		var ga = new GaManager<Route>(
			Rng,
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
					new RandomizedClosestNeighbourMethod(Rng)),
				PairSelector: new RandomPairSelector(Rng),
				Crossover: new OxCrossover(Rng),
				Mutation: new Inversion(Rng),
				Selection: new BetaTournament(Rng, 2)),
			EVALUATION_STRATEGY,
			new StatisticsCommittee(1, 5)
		);

		RunTest(ga, runCount, "Мутация");
	}

	public void Test5(int runCount)
	{
		var ga = new GaManager<Route>(
			Rng,
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
					new RandomizedClosestNeighbourMethod(Rng)),
				PairSelector: new RandomPairSelector(Rng),
				Crossover: new OxCrossover(Rng),
				Mutation: new PointMutation(Rng),
				Selection: new LinearRankScheme(Rng, new WithoutReturnCopy(Rng))),
			EVALUATION_STRATEGY,
			new StatisticsCommittee(1, 5)
		);

		RunTest(ga, runCount, "Селекция");
	}

	public void Test6(int runCount)
	{
		var ga = new GaManager<Route>(
			Rng,
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
					new RandomizedClosestNeighbourMethod(Rng)),
				PairSelector: new RandomPairSelector(Rng),
				Crossover: new OxCrossover(Rng),
				Mutation: new Inversion(Rng),
				Selection: new LinearRankScheme(Rng, new WithoutReturnCopy(Rng))),
			EVALUATION_STRATEGY,
			new StatisticsCommittee(1, 5)
		);

		RunTest(ga, runCount, "Мутация + Селекция");
	}

	public void Test7(int runCount)
	{
		var ga = new GaManager<Route>(
			Rng,
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
					new RandomizedClosestNeighbourMethod(Rng)),
				PairSelector: new RandomPairSelector(Rng),
				Crossover: new CxCrossover(),
				Mutation: new Inversion(Rng),
				Selection: new BetaTournament(Rng, 2)),
			EVALUATION_STRATEGY,
			new StatisticsCommittee(1, 5)
		);

		RunTest(ga, runCount, "Кроссовер + Мутация");
	}

	private static void RunTest(GaManager<Route> ga, int runCount, string testName)
	{
		var bests = new Genotype[runCount];
		var generations = new int[runCount];

		for (var i = 0; i < runCount; i++)
		{
			bests[i] = ga.FindBestGenotype();
			generations[i] = (int) ga.Statistics.TotalGenerationCount;
		}

		Console.WriteLine($"Статистика теста {testName} за {runCount} запусков:");
		Console.WriteLine($"*) Лучший из лучших: {Services.FindBest(bests, ga.Phenotype)}");
		Console.WriteLine($"*) Среднее значение приспособленности: {bests.Average(ga.Phenotype)}");
		Console.WriteLine($"*) Лучшая приспособленность: {bests.Min(ga.Phenotype)}");
		Console.WriteLine($"*) Худшая приспособленность: {bests.Max(ga.Phenotype)}");
		Console.WriteLine($"*) Среднее количество поколений: {generations.Average()}");

		Console.WriteLine("\n");
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
				if (!successfulParse) throw new Exception($"[{nameof(TestCases)}/{nameof(FromFile)}] Не удалось отпарсить расстояние");
				distances.Add(distance);
			}

			i++;
		}

		return (i, distances);
	}
}
