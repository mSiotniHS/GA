using System.Collections.Generic;
using EA.Core;

namespace EA.Helpers;

public sealed class StatisticsCommittee
{
	public List<List<Genotype>> Trace { get; }

	private readonly uint _savingFrequency;
	private readonly uint _maxTraceLength;

	private uint _counter;

	public StatisticsCommittee(uint savingFrequency = 1, uint maxTraceLength = uint.MaxValue)
	{
		Trace = new List<List<Genotype>>();

		_savingFrequency = savingFrequency;
		_maxTraceLength = maxTraceLength;

		_counter = 0;
	}

	public void Save(List<Genotype> population)
	{
		_counter++;
		if (_counter != _savingFrequency) return;
		Trace.Add(population);
		if (Trace.Count > _maxTraceLength) Trace.RemoveAt(0);
		_counter = 0;
	}
}
