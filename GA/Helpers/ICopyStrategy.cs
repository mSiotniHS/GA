using System.Collections.Generic;
using GA.Core;

namespace GA;

public interface ICopyStrategy
{
	public IEnumerable<Genotype> Copy(IList<Genotype> fund, IList<double> weights, uint count);
}
