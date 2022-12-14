using System.Collections.Generic;
using EA.Core;

namespace EA.Upper;

public interface IPopulationGenerator
{
	public IEnumerable<Genotype> Generate(int count);
}
