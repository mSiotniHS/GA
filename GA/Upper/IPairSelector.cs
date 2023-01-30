using System.Collections.Generic;
using GA.Core;

namespace GA.Upper;

public interface IPairSelector
{
	public IEnumerable<(Genotype, Genotype)> Select(IEnumerable<Genotype> population);
}
