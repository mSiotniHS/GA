using System.Collections.Generic;

namespace GA.Core;

public interface ICrossover
{
	public IEnumerable<Genotype> Perform(Genotype parent1, Genotype parent2);
}
