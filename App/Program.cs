namespace App;

internal static class Program
{
	private static void Main()
	{
		const int runCount = 300;

		var tests = new TestCases();
		tests.Test1(runCount);
		tests.Test2(runCount);
		tests.Test3(runCount);
		tests.Test4(runCount);
		tests.Test5(runCount);
	}
}
