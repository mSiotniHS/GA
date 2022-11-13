using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using EA.Helpers;

namespace EA.Core;

internal sealed class GaCore
{
	private static readonly bool[] Items = {true, false};
	private readonly double[] _mutationWeights;
	private readonly double[] _crossoverWeights;
	private readonly uint _newcomerCount;

	private ICrossover Crossover { get; }
	private IMutation Mutation { get; }
	private ISelection Selection { get; }
	public GaParameters Parameters { get; }

	public GaCore(GaParameters parameters, ICrossover crossover, IMutation mutation, ISelection selection)
	{
		Parameters = parameters;
		Crossover = crossover;
		Mutation = mutation;
		Selection = selection;

		_mutationWeights = new[] {Parameters.MutationRate, 1 - Parameters.MutationRate};
		_crossoverWeights = new[] {Parameters.CrossoverRate, 1 - Parameters.CrossoverRate};

		_newcomerCount = Convert.ToUInt32(Parameters.PopulationSize * Parameters.GenerationalOverlapRatio);
	}

	public List<Genotype> PerformIteration(List<Genotype> population, List<(Genotype, Genotype)> parents,
		Func<Genotype, int> phenotype)
	{
		var children = PerformCrossover(parents);
		var mutants = Mutate(children);

		if (children.Count == 0)
		{
			return population;
		}

		var reproductionSet = new List<Genotype>(children.Count + mutants.Count);
		reproductionSet.AddRange(children);
		reproductionSet.AddRange(mutants);

		return SelectAndSwap(population, reproductionSet, phenotype);
	}

	private List<Genotype> PerformCrossover(List<(Genotype, Genotype)> parents)
	{
		var children = new List<Genotype>();
		foreach (var parentPair in parents)
		{
			var toGiveBirth = Roulette.Spin(Items, _crossoverWeights);
			if (!toGiveBirth) continue;

			var (parent1, parent2) = parentPair;
			children.AddRange(Crossover.Perform(parent1, parent2));
		}

		return children;
	}

	private List<Genotype> Mutate(List<Genotype> children)
	{
		var mutants = new List<Genotype>();
		foreach (var child in children)
		{
			var toMutate = Roulette.Spin(Items, _mutationWeights);
			if (!toMutate) continue;

			mutants.Add(Mutation.Perform(child));
		}

		return mutants;
	}

	private List<Genotype> SelectAndSwap(IReadOnlyCollection<Genotype> population, List<Genotype> fund,
		Func<Genotype, int> phenotype)
	{
		var newPopulation = new List<Genotype>(population.Count);

		List<Genotype> survivors;
		if (Parameters.UseElitistStrategy)
		{
			var best = Services.FindBest(fund, phenotype);
			newPopulation.Add(best);
			fund.Remove(best);

			survivors = Selection.Perform(fund, phenotype, _newcomerCount - 1).ToList();
		}
		else
		{
			survivors = Selection.Perform(fund, phenotype, _newcomerCount).ToList();
		}

		var toBeSaved = population.Select(x => new Genotype(x)).ToList();
		for (var i = 0; i < _newcomerCount; i++)
		{
			toBeSaved.RemoveAt(Randomness.GetInt(toBeSaved.Count));
		}

		newPopulation.AddRange(toBeSaved);
		newPopulation.AddRange(survivors);

		return newPopulation;
	}
}