using System.Collections.Generic;
using EA.BaseProblem;
using EA.Core;

namespace EA.CommonModules;

public sealed class ProblemSolverPopulationGenerator<TBaseType, TBaseProblem> : IPopulationGenerator
	where TBaseProblem : IGaProblem<TBaseType>
{
	private TBaseProblem BaseProblem { get; }
	private IProblemSolver<TBaseType, TBaseProblem> ProblemSolver { get; }

	public ProblemSolverPopulationGenerator(TBaseProblem baseProblem, IProblemSolver<TBaseType, TBaseProblem> problemSolver)
	{
		BaseProblem = baseProblem;
		ProblemSolver = problemSolver;
	}

	public IEnumerable<Genotype> Generate(int count)
	{
		for (var i = 0; i < count; i++)
		{
			yield return BaseProblem.Coder.Encode(ProblemSolver.FindSolution(BaseProblem));
		}
	}
}
