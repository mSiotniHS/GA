﻿namespace EA.Core;

public interface IMutation
{
	public Genotype Perform(Genotype genotype);
	public bool GuaranteesValidGenotype { get; }
}