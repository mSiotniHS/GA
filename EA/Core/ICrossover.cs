using System.Collections.Generic;

namespace EA.Core;

public interface ICrossover
{
	public List<Genotype> Perform(Genotype parent1, Genotype parent2);
	public bool GuaranteesValidGenotype { get; }
}