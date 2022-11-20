using EA.BaseProblem;

namespace Travelling_Salesman_Problem;

public sealed class TspCriterion : ICriterion<Route>
{
	private readonly DistanceMatrix _distances;

	public TspCriterion(DistanceMatrix distances)
	{
		_distances = distances;
	}

	public int Calculate(Route route)
	{
		var sum = 0;
		for (var i = 0; i < route.Count - 1; i++)
		{
			sum += _distances[route[i], route[i + 1]];
		}

		sum += _distances[route[^1], route[0]];

		return sum;
	}
}
