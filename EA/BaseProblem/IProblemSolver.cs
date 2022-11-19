namespace EA.BaseProblem;

public interface IProblemSolver<out TBaseType, in TBaseProblem> where TBaseProblem : IProblem<TBaseType>
{
	public TBaseType FindSolution(TBaseProblem problem);
}
