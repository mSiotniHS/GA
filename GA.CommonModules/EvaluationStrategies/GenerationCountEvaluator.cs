using System.Numerics;
using OptimizationProblemsFramework;
using GA.Upper;

namespace GA.CommonModules.EvaluationStrategies;

/// <summary>
/// Завершает работу ЭГА при достижении
/// определённого поколения.
/// </summary>
/// <typeparam name="TBase"></typeparam>
/// <typeparam name="TNumber"></typeparam>
/// <typeparam name="TBaseProblem"></typeparam>
public sealed class GenerationCountEvaluator<TBaseProblem, TBase, TNumber> : IEvaluationStrategy<TBaseProblem, TBase, TNumber>
	where TNumber : INumber<TNumber>
	where TBaseProblem : IOptimizationProblem<TBase, TNumber>
{
	private readonly int _maxGenerations;

	public GenerationCountEvaluator(int maxGenerations)
	{
		_maxGenerations = maxGenerations;
	}

	public bool ShouldWork(GaManager<TBaseProblem, TBase, TNumber> state)
	{
		return _maxGenerations != state.Statistics.TotalGenerationCount;
	}

	public void Reset() {}
}
