using GA.Core;

namespace GA.BaseProblem;

public interface IGaProblem<TBaseType> : IProblem<TBaseType>
{
	public ICoder<TBaseType, Genotype> Coder { get; }
}
