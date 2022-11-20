using System.Collections.Generic;
using System.Linq;
using Common;
using EA.Core;

namespace EA.CommonModules;

public sealed class WithoutReturnCopy : ICopyStrategy
{
	public IEnumerable<Genotype> Copy(IList<Genotype> fund, IList<double> weights, uint count)
	{
#if DEBUG
		Logger.Begin(nameof(WithoutReturnCopy), nameof(Copy));
		Logger.Log($"Особи и веса:\n{string.Join('\n', fund.Zip(weights, (w, f) => $"*) {w} - {f}"))}");
#endif

		var copiedFund = fund.Select(x => new Genotype(x)).ToList();
		var copiedWeights = weights.ToList();

		for (var i = 0; i < count; i++)
		{
			yield return Roulette.Spin(copiedFund, copiedWeights, out var idx);

#if DEBUG
			Logger.Log($"Выбрали {copiedFund[idx]}");
#endif

			copiedFund.RemoveAt(idx);
			copiedWeights.RemoveAt(idx);
		}
#if DEBUG
		Logger.End();
#endif
	}
}
