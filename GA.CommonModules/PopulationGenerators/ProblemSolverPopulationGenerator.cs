using System.Collections.Generic;
using GA.BaseProblem;
using GA.Core;
using GA.Upper;

namespace GA.CommonModules.PopulationGenerators;

public sealed class ProblemSolverPopulationGenerator<TBaseType, TBaseProblem> : IPopulationGenerator
	where TBaseProblem : IGaOptimizationProblem<TBaseType>
{
	private readonly TBaseProblem _baseProblem;
	private readonly IProblemSolver<TBaseType, TBaseProblem> _problemSolver;

	public ProblemSolverPopulationGenerator(TBaseProblem baseProblem, IProblemSolver<TBaseType, TBaseProblem> problemSolver)
	{
		_baseProblem = baseProblem;
		_problemSolver = problemSolver;
	}

	public IEnumerable<Genotype> Generate(int count)
	{
		for (var i = 0; i < count; i++)
		{
			yield return _baseProblem.Coder.Encode(_problemSolver.FindSolution(_baseProblem));
		}
	}
}
