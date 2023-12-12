using System.Linq;
using GA.Helpers;
using GA.Upper;

namespace GA.CommonModules.EvaluationStrategies;

/// <summary>
/// Завершает работу ЭГА, если приспособленность лучших особей
/// "n" поколений не улучшается
/// </summary>
/// <typeparam name="TBaseType"></typeparam>
public sealed class NoProgressEvaluator<TBaseType> : IEvaluationStrategy<TBaseType>
{
	private readonly uint _maxNoProgressCount;
	private uint _noProgressCount;
	private int _lastBestFitness;

	public NoProgressEvaluator(uint maxNoProgressCount)
	{
		_maxNoProgressCount = maxNoProgressCount;
		_noProgressCount = 0;
		_lastBestFitness = int.MaxValue;
	}

	public bool ShouldWork(GaManager<TBaseType> state)
	{
		if (state.Statistics.Trace.Count == 0) return true;

		if (state.Statistics.Trace.Count == 1)
		{
			Services.FindBest(state.Statistics.Trace[0], state.Phenotype, out _lastBestFitness);
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
		_lastBestFitness = int.MaxValue;
		_noProgressCount = 0;
	}
}
