using System.Collections.Generic;
using EA.BaseProblem;
using EA.Core;

namespace EA.CommonModules;

public sealed class ProblemSolverPopulationGenerator<TBase, TGaProblem> : IPopulationGenerator
	where TGaProblem : IGaProblem<TBase>
{
	private TGaProblem BaseProblem { get; }
	private IProblemSolver<TBase, TGaProblem> ProblemSolver { get; }

	public ProblemSolverPopulationGenerator(TGaProblem baseProblem, IProblemSolver<TBase, TGaProblem> problemSolver)
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
