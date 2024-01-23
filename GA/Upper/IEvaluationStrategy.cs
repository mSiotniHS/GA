using System.Numerics;
using OptimizationProblemsFramework;

namespace GA.Upper;

public interface IEvaluationStrategy<TBaseProblem, TBase, TNumber>
	where TNumber : INumber<TNumber>
	where TBaseProblem : IOptimizationProblem<TBase, TNumber>
{
	bool ShouldWork(GaManager<TBaseProblem, TBase, TNumber> state);
	void Reset();
}
