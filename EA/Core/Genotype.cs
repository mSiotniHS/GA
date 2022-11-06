using System;
using System.Collections.Generic;
using System.Linq;

namespace EA.Core;

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

	// interval [start, end)
	public Genotype Copy(int start, int end)
	{
		var genes = new List<int?>();
		for (var i = 0; i < Length; i++)
		{
			if (i >= start && i < end)
			{
				genes.Add(this[i]);
			}
			else
			{
				genes.Add(null);
			}
		}

		return new Genotype(genes);
	}

	public bool Contains(int gene)
	{
		return ContainsIn(gene, 0, Length);
	}

	// interval: [start, end)
	public bool ContainsIn(int gene, int start, int end)
	{
		for (var i = start; i < end; i++)
		{
			if (this[i] is not null && this[i] == gene)
			{
				return true;
			}
		}

		return false;
	}

	public override string ToString() => $"({string.Join(", ", Genes)})";
}
