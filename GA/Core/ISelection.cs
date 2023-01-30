using System;
using System.Collections.Generic;

namespace GA.Core;

public interface ISelection
{
	public IEnumerable<Genotype> Perform(List<Genotype> fund, Func<Genotype, int> phenotype, uint count);
}
