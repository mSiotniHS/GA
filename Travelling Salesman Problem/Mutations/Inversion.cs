using Common;
using EA.Core;

namespace Travelling_Salesman_Problem.Mutations;

public sealed class Inversion : IMutation
{
	private readonly IRng _rng;

	public Inversion(IRng rng)
	{
		_rng = rng;
	}

	public Genotype Perform(Genotype genotype)
	{
		var startIdx = _rng.GetInt(genotype.Length - 1); // -1, чтобы хотя бы 2 гена было
		var endIdx = _rng.GetInt(startIdx + 2, genotype.Length + 1);

		return PurePerform(genotype, startIdx, endIdx);
	}

	public bool GuaranteesValidGenotype => true;

	// [startIdx, endIdx)
	private static Genotype PurePerform(Genotype genotype, int startIdx, int endIdx)
	{
#if DEBUG
		Logger.Begin(nameof(Inversion), nameof(PurePerform));
		Logger.Log($"Исходный генотип: {genotype}");
		Logger.Log($"[startIdx, endIdx) = [{startIdx}, {endIdx})");
#endif

		var newGenotype = new Genotype(genotype);

		var length = endIdx - startIdx;
		var halfLength = length / 2;
		for (var i = 0; i < halfLength; i++)
		{
			var left = i + startIdx;
			var right = endIdx - i - 1;
			(newGenotype[left], newGenotype[right]) = (newGenotype[right], newGenotype[left]);
		}

#if DEBUG
		Logger.Log($"Мутированный генотип: {newGenotype}");
		Logger.End();
#endif

		return newGenotype;
	}
}
