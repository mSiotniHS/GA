using System.Collections.Generic;
using System.Linq;
using Common;
using GA.Core;

namespace GA.CommonModules.CopyStrategies;

public sealed class WithoutReturnCopy(IRng rng) : ICopyStrategy
{
    public IEnumerable<Genotype> Copy(IList<Genotype> fund, IList<double> weights, uint count)
	{
		Logger.Begin(nameof(WithoutReturnCopy), nameof(Copy));
		Logger.Log($"Особи и веса:\n{string.Join('\n', fund.Zip(weights, (w, f) => $"*) {w} - {f}"))}");

		var copiedFund = fund.Select(x => new Genotype(x)).ToList();
		var copiedWeights = weights.ToList();

		for (var i = 0; i < count; i++)
		{
			yield return Roulette.Spin(rng, copiedFund, copiedWeights, out var idx);
			Logger.Log($"Выбрали {copiedFund[idx]}");

			copiedFund.RemoveAt(idx);
			copiedWeights.RemoveAt(idx);
		}

		Logger.End();
	}
}
