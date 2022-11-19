using System;
using System.Linq;
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
		IGaProblem<TBaseType> baseProblem,
		GaParameters parameters,
		GaModules modules,
		IEvaluationStrategy<TBaseType> evaluationStrategy,
		StatisticsCommittee? statistics = null)
	{
		if (parameters.PopulationSize % 2 != 0)
			throw new ArgumentException($"[{nameof(GaManager<TBaseType>)}/cons] Размер популяции должен быть чётным числом");

		_core = new GaCore(parameters, modules.Crossover, modules.Mutation, modules.Selection);
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
		Statistics.Reset();
		_evaluationStrategy.Reset();

		var population = _populationGenerator.Generate(_core.Parameters.PopulationSize).ToList();

		while (_evaluationStrategy.ShouldWork(this))
		{
			var pairs = _pairSelector.Select(population).ToList();
			population = _core.PerformIteration(population, pairs, Phenotype);

			Statistics.Save(population);
		}

		return Services.FindBest(population, Phenotype);
	}

	public int Phenotype(Genotype genotype)
	{
		return _baseProblem.Criterion.Calculate(_baseProblem.Coder.Decode(genotype));
	}
}
