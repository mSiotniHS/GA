using System;
using Common;

namespace Travelling_Salesman_Problem;

public sealed class DistanceMatrix
{
	private readonly int[] _data;
	public int CityCount { get; }

	public DistanceMatrix(int cityCount, int[] data)
	{
		CityCount = cityCount;
		if (data.Length != CityCount * (CityCount - 1) / 2)
			throw new ArgumentException("Invalid data length");
		_data = data;
	}

	private int ConvertIndex(int i, int j)
	{
		if (i == j) return 0;

		if (i > j)
		{
			(i, j) = (j, i);
		}

		var idx = j - i - 1;
		var jump = Convert.ToInt32(Utilities.APSum(CityCount - 1, -1, i));

		return jump + idx;
	}

	public int this[int from, int to] => _data[ConvertIndex(from, to)];
}