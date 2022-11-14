using System.Collections.Generic;
using EA.BaseProblem;

namespace Travelling_Salesman_Problem;

public sealed class TspCriterion : ICriterion<List<int>>
{
	private readonly DistanceMatrix _distances;

	public TspCriterion(DistanceMatrix distances)
	{
		_distances = distances;
	}

	public int Calculate(List<int> bas)
	{
		var sum = 0;
		for (var i = 0; i < bas.Count - 1; i++)
		{
			sum += _distances[bas[i], bas[i + 1]];
		}

		sum += _distances[bas[^1], bas[0]];

		return sum;
	}
}
