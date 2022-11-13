namespace EA.BaseProblem;

public interface IProblem<in TBase>
{
	public ICriterion<TBase> Criterion { get; }
}