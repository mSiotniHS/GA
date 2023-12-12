using System.Numerics;
using GA.Core;

namespace GA.Upper;

public record GaModules<TNumber>(
	IPopulationGenerator PopulationGenerator,
	IPairSelector PairSelector,
	ICrossover Crossover,
	IMutation Mutation,
	ISelection<TNumber> Selection)
	where TNumber : INumber<TNumber>
{
	public IPopulationGenerator PopulationGenerator { get; } = PopulationGenerator;
	public IPairSelector PairSelector { get; } = PairSelector;
	public ICrossover Crossover { get; } = Crossover;
	public IMutation Mutation { get; } = Mutation;
	public ISelection<TNumber> Selection { get; } = Selection;
}
