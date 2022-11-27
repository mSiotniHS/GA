using Common;
using EA.Core;
using Travelling_Salesman_Problem.Mutations;

namespace TSP.Tests;

public sealed class SaltationTest
{
	[Theory]
	[MemberData(nameof(SaltationShouldWorkTestData))]
	public void SaltationShouldWork(int[] range, int?[] rawGenotype, int?[] rawExpected)
	{
		var rng = new PseudoRng(range, Array.Empty<double>());
		var inversion = new Saltation(rng);

		var genotype = new Genotype(rawGenotype);
		var expected = new Genotype(rawExpected);

		var actual = inversion.Perform(genotype);

		Assert.Equal(expected.ToFilledArray(), actual.ToFilledArray());
	}

	public static IEnumerable<object[]> SaltationShouldWorkTestData()
	{
		yield return new object[]
		{
			new[] { 2, 5 },
			new int?[] {1, 2, 3, 4, 5, 6, 7},
			new int?[] {1, 2, 6, 4, 5, 3, 7}
		};

		yield return new object[]
		{
			new[] { 1, 3 },
			new int?[] {1, 2, 3, 4, 5, 6},
			new int?[] {1, 4, 3, 2, 5, 6}
		};

		yield return new object[]
		{
			new[] { 2, 6 },
			new int?[] {1, 2, 3, 4, 5, 6, 7},
			new int?[] {1, 2, 7, 4, 5, 6, 3}
		};
	}
}
