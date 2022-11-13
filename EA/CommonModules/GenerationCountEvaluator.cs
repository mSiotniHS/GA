namespace EA.CommonModules;

public sealed class GenerationCountEvaluator<TBase> : IEvaluationStrategy<TBase>
{
	private readonly int _maxGenerations;

	public GenerationCountEvaluator(int maxGenerations)
	{
		_maxGenerations = maxGenerations;
	}

	public bool ShouldWork(GaManager<TBase> state)
	{
		return _maxGenerations != state.Statistics.TotalGenerationCount;
	}
}