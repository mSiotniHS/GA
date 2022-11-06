using System.Collections.Generic;
using System.Linq;
using EA.BaseProblem;
using EA.Core;

namespace Travelling_Salesman_Problem;

public sealed class BasicRouteCoder: ICoder<List<int>, Genotype>
{
	public Genotype Encode(List<int> from) => new (from.Cast<int?>());

	public List<int> Decode(Genotype to)
	{
		var from = new List<int>();

		for (var i = 0; i < to.Length; i++)
		{
			from.Add(to.GetNonNull(i));
		}

		return from;
	}
}
