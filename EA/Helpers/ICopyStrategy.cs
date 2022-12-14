using System.Collections.Generic;
using EA.Core;

namespace EA;

public interface ICopyStrategy
{
	public IEnumerable<Genotype> Copy(IList<Genotype> fund, IList<double> weights, uint count);
}
