namespace EA.BaseProblem;

public interface IProblemSolver<out TBase, in TProblem> where TProblem: IProblem<TBase>
{
	public TBase FindSolution(TProblem problem);
}
