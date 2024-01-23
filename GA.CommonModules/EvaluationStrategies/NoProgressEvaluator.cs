using System;
using System.Linq;
using System.Numerics;
using GA.Helpers;
using GA.Upper;
using OptimizationProblemsFramework;

namespace GA.CommonModules.EvaluationStrategies;

/// <summary>
/// Завершает работу ЭГА, если приспособленность лучших особей
/// "n" поколений не улучшается
/// </summary>
/// <typeparam name="TBaseType"></typeparam>
public sealed class NoProgressEvaluator<TBaseProblem, TBase, TNumber> : IEvaluationStrategy<TBaseProblem, TBase, TNumber>
    where TNumber : struct, INumber<TNumber>
    where TBaseProblem : IOptimizationProblem<TBase, TNumber>
{
	private readonly uint _maxNoProgressCount;
	private uint _noProgressCount;
	private TNumber? _lastBestFitness;

	public NoProgressEvaluator(uint maxNoProgressCount)
	{
		_maxNoProgressCount = maxNoProgressCount;

		Reset();
	}

	public bool ShouldWork(GaManager<TBaseProblem, TBase, TNumber> state)
	{
		if (state.Statistics.Trace.Count == 0) return true;

		if (state.Statistics.Trace.Count == 1)
		{
			Services.FindBest(state.Statistics.Trace[0], state.Phenotype, out var fitness);
			_lastBestFitness = fitness;
			return true;
		}

		Services.FindBest(state.Statistics.Trace.Last(), state.Phenotype, out var bestFitness);

		if (bestFitness < _lastBestFitness)
		{
			_lastBestFitness = bestFitness;
			_noProgressCount = 0;
			return true;
		}

		_noProgressCount++;
		return _noProgressCount < _maxNoProgressCount;
	}

	public void Reset()
	{
		_lastBestFitness = null;
		_noProgressCount = 0;
	}
}
