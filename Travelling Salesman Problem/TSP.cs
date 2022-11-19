using EA.BaseProblem;
using EA.Core;

namespace Travelling_Salesman_Problem;

/// <summary>
/// Представляет собой задачу коммивояжёра. Реализует
/// интерфейс <c>IGAProblem</c>, поэтому может быть
/// использован как базовая задача ЭГА.
/// </summary>
public sealed class Tsp : IGaProblem<Route>
{
	public DistanceMatrix Distances { get; }
	public ICoder<Route, Genotype> Coder { get; }
	public ICriterion<Route> Criterion { get; }

	public Tsp(DistanceMatrix distances)
	{
		Distances = distances;
		Coder = new BasicRouteCoder();
		Criterion = new TspCriterion(distances);
	}
}
