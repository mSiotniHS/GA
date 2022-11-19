using System.Linq;
using EA.BaseProblem;
using EA.Core;

namespace Travelling_Salesman_Problem;

public sealed class BasicRouteCoder : ICoder<Route, Genotype>
{
	public Genotype Encode(Route from) => new(from.Cast<int?>());

	public Route Decode(Genotype to)
	{
		var from = new Route();

		for (var i = 0; i < to.Length; i++)
		{
			from.Add(to.GetNonNull(i));
		}

		return from;
	}
}
