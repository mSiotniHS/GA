using System.Collections.Generic;

namespace GA.Core;

public interface ICrossover
{
	public List<Genotype> Perform(Genotype parent1, Genotype parent2);
}
