using System;
using System.Collections.Generic;
using System.Numerics;

namespace GA.Core;

public interface ISelection<in TNumber>
	where TNumber : INumber<TNumber>
{
	public IEnumerable<Genotype> Perform(List<Genotype> fund, PhenotypeCalculator<TNumber> phenotype, uint count);
}
