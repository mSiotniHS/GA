namespace EA.BaseProblem;

public interface IProblem<in TBaseType>
{
	public ICriterion<TBaseType> Criterion { get; }
}
