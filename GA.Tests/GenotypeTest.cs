using GA.Core;

namespace GA.Tests;

public sealed class GenotypeTest
{
	[Theory]
	[MemberData(nameof(LsbaiTestData))]
	public void LengthShouldBeAsInput(int?[] rawGenotype, int expectedLength)
	{
		var genotype = new Genotype(rawGenotype);
		Assert.Equal(expectedLength, genotype.Length);
	}

	public static IEnumerable<object[]> LsbaiTestData()
	{
		yield return new object[] { new int?[] {1, 4, 3, 2}, 4 };
		yield return new object[] { new int?[] {1, 4, 2}, 3 };
		yield return new object[] { new int?[] {1, 4, 2, 4, 1}, 5 };
		yield return new object[] { Array.Empty<int?>(), 0 };
	}
}
