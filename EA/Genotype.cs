using System;
using System.Collections.Generic;
using System.Linq;

namespace EA;

public record Genotype
{
	private List<Gene> Genes { get; }
	public int Length => Genes.Count;

	public Genotype(List<Gene> genes)
	{
		Genes = genes;
	}

	public Genotype(IEnumerable<int> values)
	{
		Genes = values.Select(v => new Gene(v)).ToList();
	}

	public Gene this[int idx]
	{
		get => Genes[idx];
		set => Genes[idx] = value;
	}

	public int DistanceTo(Genotype other)
	{
		var distance = 0;
		for (var i = 0; i < Length; i++)
		{
			distance += Math.Abs(this[i].Value - other[i].Value);
		}

		return distance;
	}
}
