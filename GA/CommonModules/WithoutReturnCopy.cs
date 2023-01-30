using System.Collections.Generic;
using System.Linq;
using Common;
using GA.Core;

namespace GA.CommonModules;

public sealed class WithoutReturnCopy : ICopyStrategy
{
	private readonly IRng _rng;

	public WithoutReturnCopy(IRng rng)
	{
		_rng = rng;
	}

	public IEnumerable<Genotype> Copy(IList<Genotype> fund, IList<double> weights, uint count)
	{
		Logger.Begin(nameof(WithoutReturnCopy), nameof(Copy));
		Logger.Log($"Особи и веса:\n{string.Join('\n', fund.Zip(weights, (w, f) => $"*) {w} - {f}"))}");

		var copiedFund = fund.Select(x => new Genotype(x)).ToList();
		var copiedWeights = weights.ToList();

		for (var i = 0; i < count; i++)
		{
			yield return Roulette.Spin(_rng, copiedFund, copiedWeights, out var idx);
			Logger.Log($"Выбрали {copiedFund[idx]}");

			copiedFund.RemoveAt(idx);
			copiedWeights.RemoveAt(idx);
		}

		Logger.End();
	}
}
