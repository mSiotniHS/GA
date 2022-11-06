using System;
using System.Linq;
using EA.BaseProblem;
using EA.Core;
using EA.Helpers;

namespace EA;

public sealed class GaManager<TBase>
{
	private readonly IGaProblem<TBase> _baseProblem;

	private readonly GaCore _core;
	private readonly IPairSelector _pairSelector;
	private readonly IPopulationGenerator _populationGenerator;

	public StatisticsCommittee Statistics { get; }
	private readonly IEvaluationStrategy<TBase> _evaluationStrategy;

	public GaManager(
		IGaProblem<TBase> baseProblem,
		GaParameters parameters,
		GaModules modules,
		IEvaluationStrategy<TBase> evaluationStrategy,
		StatisticsCommittee? statistics = null)
	{
		if (parameters.PopulationSize % 2 != 0) throw new ArgumentException("Population size better be even");

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

	public TBase FindSolution()
	{
		var population = _populationGenerator.Generate(_core.Parameters.PopulationSize).ToList();

		while (_evaluationStrategy.ShouldWork(this))
		{
			var pairs = _pairSelector.Select(population).ToList();
			population = _core.PerformIteration(population, pairs, Phenotype);

			Statistics.Save(population);
		}

		return _baseProblem.Coder.Decode(Services.FindBest(population, Phenotype));
	}

	public int Phenotype(Genotype genotype)
	{
		return _baseProblem.Criterion.Calculate(_baseProblem.Coder.Decode(genotype));
	}
}
