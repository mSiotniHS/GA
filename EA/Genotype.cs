using System;
using System.Collections.Generic;
using System.Linq;

namespace EA;

public record Genotype
{
	private int?[] Genes { get; }
	public int Length => Genes.Length;

	public Genotype(IEnumerable<int?> values)
	{
		Genes = values.ToArray();
	}

	public Genotype(int length)
	{
		Genes = new int?[length];
	}

	public int? this[int idx]
	{
		get => Genes[idx];
		set => Genes[idx] = value;
	}

	public int DistanceTo(Genotype other)
	{
		var distance = 0;
		for (var i = 0; i < Length; i++)
		{
			var value = this[i];
			var otherValue = other[i];

			if (value is null || otherValue is null)
			{
				throw new Exception("Genotype is not fully defined");
			}

			distance += Math.Abs(value.Value - otherValue.Value);
		}

		return distance;
	}
}
