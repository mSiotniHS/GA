using System.Collections.Generic;
using EA.Core;

namespace EA;

public interface IPopulationGenerator
{
	public IEnumerable<Genotype> Generate(int count);
}