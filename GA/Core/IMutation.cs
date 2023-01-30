namespace GA.Core;

public interface IMutation
{
	public Genotype Perform(Genotype genotype);
}
