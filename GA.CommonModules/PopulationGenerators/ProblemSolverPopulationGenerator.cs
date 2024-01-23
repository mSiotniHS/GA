using System.Collections.Generic;
using OptimizationProblemsFramework;
using GA.Core;
using GA.Upper;
using System.Numerics;
using GA.BaseProblem;

namespace GA.CommonModules.PopulationGenerators;

public sealed class ProblemSolverPopulationGenerator<TBaseProblem, TBaseType, TNumber>(
        TBaseProblem baseProblem,
        IProblemSolver<TBaseProblem, TBaseType, TNumber> problemSolver,
        IGaEncoder<TBaseType> encoder
    ) : IPopulationGenerator
	where TBaseProblem : IOptimizationProblem<TBaseType, TNumber>
	where TNumber : INumber<TNumber>
{
    public IEnumerable<Genotype> Generate(int count)
	{
		for (var i = 0; i < count; i++)
		{
			yield return encoder.Encode(problemSolver.FindSolution(baseProblem));
		}
	}
}
