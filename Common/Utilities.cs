using System.Collections.Generic;
using System;

namespace Common;

public static class Utilities
{
	public static double APSum(double first, double difference, int n) =>
        (2 * first + difference * (n - 1)) / 2 * n;

    public static IEnumerable<T> Generate<T>(int count, Func<T> function)
    {
        for (var i = 0; i < count; i++)
        {
            yield return function();
        }
    }
}
