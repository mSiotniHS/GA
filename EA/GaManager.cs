using System;
using System.Linq;
using Common;
using EA.BaseProblem;
using EA.Core;
using EA.Helpers;

namespace EA;

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
		Statistics = statistics switch
		{
			{ } => statistics,
			null => new StatisticsCommittee()
		};
	}

	public TBaseType FindSolution()
	{
		return _baseProblem.Coder.Decode(FindBestGenotype());
	}

	public Genotype FindBestGenotype()
	{
#if DEBUG
		Logger.Begin(nameof(GaManager<TBaseType>), nameof(FindBestGenotype));
#endif

		Statistics.Reset();
		_evaluationStrategy.Reset();

#if DEBUG
		Logger.Log("Сбросили статистику");
#endif

		var population = _populationGenerator.Generate(_core.Parameters.PopulationSize).ToList();

#if DEBUG
		Logger.Log($"Начальная популяция:\n{string.Join('\n', population)}");
#endif

		while (_evaluationStrategy.ShouldWork(this))
		{
#if DEBUG
			Logger.Log("Продолжаем работу");
			Logger.Log("Сейчас имеем:");
			Logger.Log(string.Join(
				'\n',
				population.Zip(population.Select(Phenotype), (genotype, phenotype) => $"*) {genotype} - {phenotype}"))
			);
			Logger.Log("Формируем пары");
#endif

			var pairs = _pairSelector.Select(population).ToList();

#if DEBUG
			Logger.Log("Проводим итерацию");
#endif

			population = _core.PerformIteration(population, pairs, Phenotype);

			Statistics.Save(population);
		}

#if DEBUG
		Logger.End();
#endif

		return Services.FindBest(population, Phenotype);
	}

	public int Phenotype(Genotype genotype)
	{
		return _baseProblem.Criterion.Calculate(_baseProblem.Coder.Decode(genotype));
	}
}
