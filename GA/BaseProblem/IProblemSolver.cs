using System.Numerics;

namespace GA.BaseProblem;

public interface IProblemSolver<TBase, TNumber, in TBaseProblem>
	where TNumber : INumber<TNumber>
	where TBaseProblem : IOptimizationProblem<TBase, TNumber>
{
	public (TBase, TNumber) FindSolution(TBaseProblem problem);
}
