using System.Linq;
using EA.BaseProblem;
using EA.Core;

namespace Travelling_Salesman_Problem;

public sealed class BasicRouteCoder : ICoder<Route, Genotype>
{
	public Genotype Encode(Route route) => new(route.Cast<int?>());

	public Route Decode(Genotype genotype)
	{
		var from = new Route();

		for (var i = 0; i < genotype.Length; i++)
		{
			from.Add(genotype.GetNonNull(i));
		}

		return from;
	}
}
