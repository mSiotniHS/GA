namespace EA.CommonModules;

/// <summary>
/// Завершает работу ЭГА при достижении
/// определённого поколения.
/// </summary>
/// <typeparam name="TBase"></typeparam>
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

	public void Reset() {}
}
