using EA.Upper;

namespace EA.CommonModules;

/// <summary>
/// Завершает работу ЭГА при достижении
/// определённого поколения.
/// </summary>
/// <typeparam name="TBaseType"></typeparam>
public sealed class GenerationCountEvaluator<TBaseType> : IEvaluationStrategy<TBaseType>
{
	private readonly int _maxGenerations;

	public GenerationCountEvaluator(int maxGenerations)
	{
		_maxGenerations = maxGenerations;
	}

	public bool ShouldWork(GaManager<TBaseType> state)
	{
		return _maxGenerations != state.Statistics.TotalGenerationCount;
	}

	public void Reset() {}
}
