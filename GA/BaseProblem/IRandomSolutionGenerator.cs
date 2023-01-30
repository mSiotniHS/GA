namespace GA.BaseProblem;

public interface IRandomSolutionGenerator<out TBaseType>
{
	public TBaseType PickRandom();
}
