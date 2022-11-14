using System;

namespace Common;

public static class Randomness
{
	private static readonly Random Random = new();

	public static int GetInt() => Random.Next();
	public static int GetInt(int max) => Random.Next(max);
	public static int GetInt(int min, int max) => Random.Next(min, max);

	/// <summary>
	/// Возвращает случайное число в диапазоне [0, 1)
	/// </summary>
	/// <returns>double в [0, 1)</returns>
	public static double GetDouble() => Random.NextDouble();
}
