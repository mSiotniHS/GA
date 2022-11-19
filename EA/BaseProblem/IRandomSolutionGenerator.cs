namespace EA.BaseProblem;

public interface IRandomSolutionGenerator<out TBase>
{
	public TBase PickRandom();
}
