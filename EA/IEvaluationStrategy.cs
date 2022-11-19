namespace EA;

public interface IEvaluationStrategy<TBaseType>
{
	public bool ShouldWork(GaManager<TBaseType> state);
	public void Reset();
}
