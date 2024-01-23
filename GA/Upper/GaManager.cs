using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using Common;
using GA.BaseProblem;
using GA.Core;
using GA.Helpers;
using OptimizationProblemsFramework;

namespace GA.Upper;

public class DelayedPhenotypeCalculator<TNumber>
	where TNumber : INumber<TNumber>
{
	public PhenotypeCalculator<TNumber>? Calculator { get; set; }

	public TNumber Calculate(Genotype genotype)
	{
		if (Calculator is null)
		{
            throw new ArgumentNullException(nameof(Calculator));
		}

		return Calculator(genotype);
	}
}

public sealed class GaManager<TBaseProblem, TBase, TNumber> : IProblemSolver<TBaseProblem, TBase, TNumber>
    where TBaseProblem : IOptimizationProblem<TBase, TNumber>
    where TNumber : INumber<TNumber>
{
	private readonly IGaCoder<TBase> _coder;

	private readonly IGaIteration _core;
	private readonly IPairSelector _pairSelector;
	private readonly IPopulationGenerator _populationGenerator;

	public StatisticsCommittee Statistics { get; }
	private readonly IEvaluationStrategy<TBaseProblem, TBase, TNumber> _evaluationStrategy;
	public PhenotypeCalculator<TNumber> Phenotype { get; private set; }

	public GaManager(
		IGaCoder<TBase> coder,
		IGaIteration core,
		GaModules<TNumber> modules,
		PhenotypeCalculator<TNumber> phenotype,
		IEvaluationStrategy<TBaseProblem, TBase, TNumber> evaluationStrategy,
		StatisticsCommittee? statistics = null)
	{
		Phenotype = phenotype;
		_core = core;
		_coder = coder;
		_evaluationStrategy = evaluationStrategy;
		_pairSelector = modules.PairSelector;
		_populationGenerator = modules.PopulationGenerator;
		Statistics = statistics ?? new StatisticsCommittee();
	}

	/*public (TBase, TNumber) FindSolution(TBaseProblem problem)
	{
		var genotype = FindBestGenotype(problem);
		var phenotype = Phenotype(problem)(genotype);

		return (_coder.Decode(genotype), phenotype);
	}*/

	private Genotype FindBestGenotype(TBaseProblem problem)
	{
		Logger.Begin(nameof(GaManager<TBaseProblem, TBase, TNumber>), nameof(FindBestGenotype));

		Statistics.Reset();
		_evaluationStrategy.Reset();
		Logger.Log("Сбросили статистику");


		var population = _populationGenerator.Generate(_core.Parameters.PopulationSize).ToList();
		Logger.Log($"Начальная популяция:\n{string.Join('\n', population)}");


		while (_evaluationStrategy.ShouldWork(this))
		{
			// Report(population);

			Logger.Log("Продолжаем работу");
			//Logger.Log("Сейчас имеем:\n" + string.Join(
			//	'\n',
			//	population.Zip(population.Select(Phenotype(problem)), (genotype, phenotype) => $"*) {genotype} - {phenotype}"))
			//);

			Logger.Log("Формируем пары");
			var pairs = _pairSelector.Select(population).ToList();

			//Console.WriteLine("Проводим итерацию");
			population = _core.PerformIteration(population, pairs);
			//Console.WriteLine("Итерация завершилась");

			if (population.Count != _core.Parameters.PopulationSize)
			{
				throw new UnreachableException("Популяция должна быть неизменной");
			}

			Statistics.Save(population);

            //var phenotypes = population.Select(genotype => (double) uint.CreateChecked(Phenotype(problem)(genotype))).Average();
            //Console.WriteLine($"{Statistics.TotalGenerationCount}) Средний фенотип: {phenotypes};  Среднее число цветов:");
        }

		// Report(population, true);

		// Logger.Log("Итого:\n" + string.Join(
		// 	'\n',
		// 	population.Zip(population.Select(Phenotype(problem), (genotype, phenotype) => $"*) {genotype} - {phenotype}"))
		// );

		Logger.End();

		return Services.FindBest(population, Phenotype);
	}

	public TBase FindSolution(TBaseProblem problem)
	{
		var genotype = FindBestGenotype(problem);

        return _coder.Decode(genotype);
    }

    // private void Report(List<Genotype> population, bool isEnd = false)
    // {
    // 	Console.WriteLine(isEnd
    // 		? $"\nФинальное поколение №{Statistics.TotalGenerationCount}"
    // 		: $"\nПоколение №{Statistics.TotalGenerationCount}");
    //
    // 	var tmpBest = population.MinBy(Phenotype());
    // 	Console.WriteLine($"> Лучшая особь: {tmpBest} ({Phenotype(tmpBest!)})");
    // 	Console.WriteLine("> Особи в поколении:");
    // 	foreach (var genotype in population)
    // 	{
    // 		Console.WriteLine($"> *) {genotype} ({Phenotype(genotype)})");
    // 	}
    // }
}
