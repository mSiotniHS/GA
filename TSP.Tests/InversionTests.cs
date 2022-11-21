using EA.Core;
using Travelling_Salesman_Problem.Mutations;

namespace TSP.Tests;

public sealed class InversionTests
{
	private readonly Inversion _inversion;

	public InversionTests(Inversion inversion)
	{
		_inversion = inversion;
	}

	[Theory]
	[MemberData(nameof(PpsbcTestData))]
	public void PurePerformShouldBehaveCorrectly(Genotype genotype, int start, int end, Genotype expected)
	{
		// var mutant = _inversion.PurePerform(genotype, start, end);

		// Assert.Equal(expected, mutant);
	}

	private static IEnumerable<object[]> PpsbcTestData()
	{
		yield return new object[]
		{
			new Genotype(new int?[] {1, 4, 3, 2, 5}),
			1, 4,
			new Genotype(new int?[] {1, 2, 3, 4, 5})
		};
	}
}
