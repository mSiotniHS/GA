using System;
using System.Collections.Generic;

namespace Common;

public sealed class PseudoRng : IRng
{
	private readonly IEnumerator<int> _ints;
	private readonly IEnumerator<double> _doubles;

	public PseudoRng(IEnumerable<int> ints, IEnumerable<double> doubles)
	{
		_ints = ints.GetEnumerator();
		_doubles = doubles.GetEnumerator();
	}

	public int GetInt() => NextItem(_ints);
	public int GetInt(int max) => NextItem(_ints);
	public int GetInt(int min, int max) => NextItem(_ints);
	public double GetDouble() => NextItem(_doubles);

	private static T NextItem<T>(IEnumerator<T> enumerator)
	{
		if (!enumerator.MoveNext())
		{
			throw new InvalidOperationException("No more items");
		}

		return enumerator.Current;
	}
}
