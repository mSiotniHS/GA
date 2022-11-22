using System;

namespace Common;

public sealed class Rng : IRng
{
	private static readonly Random Random = new();

	public int GetInt() => Random.Next();
	public int GetInt(int max) => Random.Next(max);
	public int GetInt(int min, int max) => Random.Next(min, max);
	public double GetDouble() => Random.NextDouble();
}
