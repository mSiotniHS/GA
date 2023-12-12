using System.Numerics;
using GA.BaseProblem;
using GA.Upper;

namespace GA.CommonModules.EvaluationStrategies;

/// <summary>
/// Завершает работу ЭГА при достижении
/// определённого поколения.
/// </summary>
/// <typeparam name="TBase"></typeparam>
/// <typeparam name="TNumber"></typeparam>
/// <typeparam name="TBaseProblem"></typeparam>
public sealed class GenerationCountEvaluator<TBase, TNumber, TBaseProblem> : IEvaluationStrategy<TBase, TNumber, TBaseProblem>
	where TNumber : INumber<TNumber>
	where TBaseProblem : IOptimizationProblem<TBase, TNumber>
{
	private readonly int _maxGenerations;

	public GenerationCountEvaluator(int maxGenerations)
	{
		_maxGenerations = maxGenerations;
	}

	public bool ShouldWork(GaManager<TBase, TNumber, TBaseProblem> state)
	{
		return _maxGenerations != state.Statistics.TotalGenerationCount;
	}

	public void Reset() {}
}
