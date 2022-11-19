using EA.Core;

namespace EA.BaseProblem;

public interface IGaProblem<TBaseType> : IProblem<TBaseType>
{
	public ICoder<TBaseType, Genotype> Coder { get; }
}
