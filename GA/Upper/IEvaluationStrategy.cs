using System.Numerics;
using GA.BaseProblem;

namespace GA.Upper;

public interface IEvaluationStrategy<TBase, TNumber, TBaseProblem>
	where TNumber : INumber<TNumber>
	where TBaseProblem : IOptimizationProblem<TBase, TNumber>
{
	public bool ShouldWork(GaManager<TBase, TNumber, TBaseProblem> state);
	public void Reset();
}
