using Common;
using EA.Core;

namespace Travelling_Salesman_Problem.Mutations;

public sealed class PointMutation : IMutation
{
	private readonly IRng _rng;

	public PointMutation(IRng rng)
	{
		_rng = rng;
	}

	public Genotype Perform(Genotype genotype)
	{
		Logger.Begin(nameof(PointMutation), nameof(Perform));
		Logger.Log($"Получил генотип {genotype}");

		var startIdx = _rng.GetInt(genotype.Length - 1);
		Logger.Log($"Меняем, начиная с {startIdx}");

		var newGenotype = new Genotype(genotype);
		(newGenotype[startIdx], newGenotype[startIdx + 1]) = (newGenotype[startIdx + 1], newGenotype[startIdx]);

		Logger.Log($"Мутированный генотип: {newGenotype}");
		Logger.End();

		return newGenotype;
	}
}
