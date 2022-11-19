using Common;
using EA.Core;

namespace Travelling_Salesman_Problem.Mutations;

public sealed class Inversion : IMutation
{
	public Genotype Perform(Genotype genotype)
	{
#if DEBUG
		Logger.Begin(nameof(Inversion), nameof(Perform));
		Logger.Log($"Исходный генотип: {genotype}");
#endif

		// [startIdx, endIdx)
		var startIdx = Randomness.GetInt(genotype.Length - 1); // -1, чтобы хотя бы 2 гена было
		var endIdx = Randomness.GetInt(startIdx + 2, genotype.Length + 1);

#if DEBUG
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

	public bool GuaranteesValidGenotype => true;
}
