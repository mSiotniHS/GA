using System.Numerics;

namespace GA.Core;

public delegate T PhenotypeCalculator<out T>(Genotype genotype)
	where T : INumber<T>;
