using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using Common;
using GA.BaseProblem;
using GA.Core;
using GA.Helpers;

namespace GA.Upper;

public sealed class GaManager<TBase, TNumber, TBaseProblem> : IProblemSolver<TBase, TNumber, TBaseProblem>
	where TNumber : INumber<TNumber>
	where TBaseProblem : IOptimizationProblem<TBase, TNumber>
{
	private readonly ICoder<TBase, Genotype> _coder;

	private readonly IGaIteration _core;
	private readonly IPairSelector _pairSelector;
	private readonly IPopulationGenerator _populationGenerator;

	public StatisticsCommittee Statistics { get; }
	private readonly IEvaluationStrategy<TBaseType> _evaluationStrategy;

	public GaManager(
		IRng rng,
		ICoder<TBase, Genotype> coder,
		GaParameters parameters,
		GaModules<TNumber> modules,
		IEvaluationStrategy<TBaseType> evaluationStrategy,
		StatisticsCommittee? statistics = null)
	{
		if (parameters.PopulationSize % 2 != 0)
			throw new ArgumentException($"[{nameof(GaManager<TBaseType>)}/cons] Размер популяции должен быть чётным числом");

		_core = new GaCore<TNumber>(rng, parameters, modules.Crossover, modules.Mutation, modules.Selection);
		_coder = coder;
		_evaluationStrategy = evaluationStrategy;
		_pairSelector = modules.PairSelector;
		_populationGenerator = modules.PopulationGenerator;
		Statistics = statistics ?? new StatisticsCommittee();
	}

	public (TBase, TNumber) FindSolution(TBaseProblem problem)
	{
		var genotype = FindBestGenotype(problem);
		var phenotype = Phenotype(problem)(genotype);

		return (_coder.Decode(genotype), phenotype);
	}

	private Genotype FindBestGenotype(TBaseProblem problem)
	{
		Logger.Begin(nameof(GaManager<TBase, TNumber, TBaseProblem>), nameof(FindBestGenotype));

		Statistics.Reset();
		_evaluationStrategy.Reset();
		Logger.Log("Сбросили статистику");


		var population = _populationGenerator.Generate(_core.Parameters.PopulationSize).ToList();
		Logger.Log($"Начальная популяция:\n{string.Join('\n', population)}");


		while (_evaluationStrategy.ShouldWork(this))
		{
			// Report(population);

			Logger.Log("Продолжаем работу");
			Logger.Log("Сейчас имеем:\n" + string.Join(
				'\n',
				population.Zip(population.Select(Phenotype(problem)), (genotype, phenotype) => $"*) {genotype} - {phenotype}"))
			);

			Logger.Log("Формируем пары");
			var pairs = _pairSelector.Select(population).ToList();

			Logger.Log("Проводим итерацию");
			population = _core.PerformIteration(population, pairs, Phenotype(problem));

			if (population.Count != _core.Parameters.PopulationSize)
			{
				throw new UnreachableException("Популяция должна быть неизменной");
			}

			Statistics.Save(population);
		}

		// Report(population, true);

		// Logger.Log("Итого:\n" + string.Join(
		// 	'\n',
		// 	population.Zip(population.Select(Phenotype(problem), (genotype, phenotype) => $"*) {genotype} - {phenotype}"))
		// );

		Logger.End();

		return Services.FindBest(population, Phenotype(problem));
	}

	private Func<Genotype, TNumber> Phenotype(TBaseProblem problem) =>
		genotype => problem.Criterion(_coder.Decode(genotype));

	// private void Report(List<Genotype> population, bool isEnd = false)
	// {
	// 	Console.WriteLine(isEnd
	// 		? $"\nФинальное поколение №{Statistics.TotalGenerationCount}"
	// 		: $"\nПоколение №{Statistics.TotalGenerationCount}");
	//
	// 	var tmpBest = population.MinBy(Phenotype());
	// 	Console.WriteLine($"> Лучшая особь: {tmpBest} ({Phenotype(tmpBest!)})");
	// 	Console.WriteLine("> Особи в поколении:");
	// 	foreach (var genotype in population)
	// 	{
	// 		Console.WriteLine($"> *) {genotype} ({Phenotype(genotype)})");
	// 	}
	// }
}
