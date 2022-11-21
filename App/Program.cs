using System;

namespace App;

internal static class Program
{
	private static void Main()
	{
		const int runCount = 100;

		var tests = new TestCases();

		/*tests.Test1(runCount);

		Console.WriteLine();
		tests.Test2(runCount);

		Console.WriteLine();
		tests.Test3(runCount);

		Console.WriteLine();
		tests.Test4(runCount);

		Console.WriteLine();
		tests.Test5(runCount);

		tests.Test5(1);*/

		tests.Test6(1);
	}
}
