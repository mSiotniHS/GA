using System.Collections.Generic;
using EA.Core;

namespace EA;

public interface IPairSelector
{
	public IEnumerable<(Genotype, Genotype)> Select(IEnumerable<Genotype> population);
}
