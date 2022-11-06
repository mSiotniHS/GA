using System.Collections.Generic;
using EA.BaseProblem;
using EA.Core;

namespace Travelling_Salesman_Problem;

/// <summary>
/// Представляет собой задачу коммивояжёра. Реализует
/// интерфейс <c>IGAProblem</c>, поэтому может быть
/// использован как базовая задача ЭГА.
/// </summary>
public sealed class Tsp : IGaProblem<List<int>>
{
	public DistanceMatrix Distances { get; }
	public ICoder<List<int>, Genotype> Coder { get; }
	public ICriterion<List<int>> Criterion { get; }

	public Tsp(DistanceMatrix distances)
	{
		Distances = distances;
		Coder = new BasicRouteCoder();
		Criterion = new TspCriterion(distances);
	}
}
