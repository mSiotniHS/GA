using EA.Core;

namespace EA.BaseProblem;

public interface IGaProblem<TBase> : IProblem<TBase>
{
	public ICoder<TBase, Genotype> Coder { get; }
}