namespace EA;

public interface IEvaluationStrategy<TBase>
{
	public bool ShouldWork(GaManager<TBase> state);
}