using System;
using System.Linq;
using Common;
using EA.BaseProblem;
using EA.Core;
using EA.Helpers;

namespace EA.Upper;

public sealed class GaManager<TBaseType>
{
	private readonly IGaProblem<TBaseType> _baseProblem;

	private readonly GaCore _core;
	private readonly IPairSelector _pairSelector;
	private readonly IPopulationGenerator _populationGenerator;

	public StatisticsCommittee Statistics { get; }
	private readonly IEvaluationStrategy<TBaseType> _evaluationStrategy;

	public GaManager(
		IRng rng,
		IGaProblem<TBaseType> baseProblem,
		GaParameters parameters,
		GaModules modules,
		IEvaluationStrategy<TBaseType> evaluationStrategy,
		StatisticsCommittee? statistics = null)
	{
		if (parameters.PopulationSize % 2 != 0)
			throw new ArgumentException($"[{nameof(GaManager<TBaseType>)}/cons] Размер популяции должен быть чётным числом");

		_core = new GaCore(rng, parameters, modules.Crossover, modules.Mutation, modules.Selection);
		_baseProblem = baseProblem;
		_evaluationStrategy = evaluationStrategy;
		_pairSelector = modules.PairSelector;
		_populationGenerator = modules.PopulationGenerator;
		Statistics = statistics ?? new StatisticsCommittee();
	}

	public TBaseType FindSolution()
	{
		return _baseProblem.Coder.Decode(FindBestGenotype());
	}

	public Genotype FindBestGenotype()
	{
		Logger.Begin(nameof(GaManager<TBaseType>), nameof(FindBestGenotype));

		Statistics.Reset();
		_evaluationStrategy.Reset();
		Logger.Log("Сбросили статистику");


		var population = _populationGenerator.Generate(_core.Parameters.PopulationSize).ToList();
		Logger.Log($"Начальная популяция:\n{string.Join('\n', population)}");


		while (_evaluationStrategy.ShouldWork(this))
		{
			Logger.Log("Продолжаем работу");
			Logger.Log("Сейчас имеем:\n" + string.Join(
				'\n',
				population.Zip(population.Select(Phenotype), (genotype, phenotype) => $"*) {genotype} - {phenotype}"))
			);

			Logger.Log("Формируем пары");
			var pairs = _pairSelector.Select(population).ToList();

			Logger.Log("Проводим итерацию");
			population = _core.PerformIteration(population, pairs, Phenotype);

			Statistics.Save(population);
		}

		Logger.End();

		return Services.FindBest(population, Phenotype);
	}

	public int Phenotype(Genotype genotype)
	{
		return _baseProblem.Criterion.Calculate(_baseProblem.Coder.Decode(genotype));
	}
}
