using System.Collections.Generic;

namespace GA.Core;

public interface IGaIteration
{
	public List<Genotype> PerformIteration(List<Genotype> population, List<(Genotype, Genotype)> parents);
	public GaParameters Parameters { get; }
}
