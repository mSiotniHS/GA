using System.Collections.Generic;
using GA.Core;

namespace GA.Upper;

public interface IPopulationGenerator
{
	public IEnumerable<Genotype> Generate(int count);
}
