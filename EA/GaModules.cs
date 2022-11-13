using EA.Core;

namespace EA;

public record GaModules(IPopulationGenerator PopulationGenerator, IPairSelector PairSelector, ICrossover Crossover,
	IMutation Mutation, ISelection Selection)
{
	public IPopulationGenerator PopulationGenerator { get; } = PopulationGenerator;
	public IPairSelector PairSelector { get; } = PairSelector;
	public ICrossover Crossover { get; } = Crossover;
	public IMutation Mutation { get; } = Mutation;
	public ISelection Selection { get; } = Selection;
}