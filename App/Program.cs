using Common;
using GA.Core;
using GA.Upper;
using GA.CommonModules.PopulationGenerators;
using GA.CommonModules.PairSelectors;
using GA.CommonModules.EvaluationStrategies;
using GA.CommonModules.Selections;
using GraphColoring.Problem;
using GraphColoring.Math;
using GraphColoring.Solvers;
using System.Numerics;
using App.Crossovers;
using App.Mutations;
using App.InitialPopulation;
using OptimizationProblemsFramework;
using GA.Helpers;
using System.Collections.Concurrent;
using System;
using System.Text;

namespace App;

using GraphColoring = GraphColoring.Math.GraphColoring;

internal class ModificationInfusedSelection<T>(ISelection<T> selection, IGenotypeModificator modificator) : ISelection<T>
	where T : INumber<T>
{
	public IEnumerable<Genotype> Perform(List<Genotype> fund, PhenotypeCalculator<T> phenotype, uint count) =>
		selection.Perform(fund.Select(modificator.Modify).ToList(), phenotype, count);
}

internal class Program
{
	static void Main()
	{
		// ItWasAThingSomeday();

		var rng = new Rng();
		var coder = new GraphColoringCoder();

		(string name, int vertexCount)[] specialGraphs = [
			("anna", 138),       // 493, оптим: 11
			//("fpsol2.i.1", 496), // 11654, оптим: 65
			//("fpsol2.i.2", 451), // 8691, оптим: 30
			("huck", 74),        // 301, оптим: 11
			//("inithx.i.2", 645), // 13979, оптим: 31
			//("le450_5a", 450),   // 5714, оптим: 5
			//("le450_5c", 450),   // 9803, оптим: 5
			//("le450_15b", 450),  // 8169, оптим: 15
			//("le450_15c", 450),  // 16680, оптим: 15
			("miles250", 128),   // 387, оптим: 8
			("mulsol.i.1", 197), // 3925, оптим: 49
			("myciel4", 23),     // 71, оптим: 5
			("myciel6", 95),     // 755, оптим: 7
			("myciel7", 191),    // 2360, оптим: 8
			("queen6_6", 36),    // 290, оптим: 7
			//("zeroin.i.2", 211)  // 3541, оптим: 30
		];

		GaParameters CreateParameters(GraphColoringProblem problem) =>
			new(
				PopulationSize: problem.Graph.VertexCount switch
					{
						<= 50 => 40,
						<= 100 => 70,
						<= 200 => 150,
						_ => problem.Graph.VertexCount / 2
					},
				MutationRate: 0.05,
				CrossoverRate: 0.95,
				GenerationalOverlapRatio: 0.5,
				UseElitistStrategy: false
			);

		GaManager<GraphColoringProblem, GraphColoring, uint> @base(GraphColoringProblem problem)
		{
			var modificator = new GraphColoringDomainModificator(problem, coder);
			uint phenotype(Genotype genotype) => problem.Criterion(coder.Decode(genotype));

			var modules = new GaModules<uint>(
				PopulationGenerator: new ProblemSolverPopulationGenerator<GraphColoringProblem, GraphColoring, uint>(
					problem,
					new GreedySequentialColoring(new RandomizedLfOrdering(rng)),
					coder
				),
				PairSelector: new RandomPairSelector(rng),
				Mutation: new GeneMutation(rng),
				Crossover: new MultipointCrossover(2),
				Selection: new BetaTournament<uint>(rng, 2)
			);

			var parameters = CreateParameters(problem);

			IGaIteration core = new GaCore<uint>(
				rng,
				parameters,
				modules.Crossover,
				modules.Mutation,
				modules.Selection,
				phenotype
			);
			core = new ModificationInfusedGaIteration<uint>(core, modificator);

			return new GaManager<GraphColoringProblem, GraphColoring, uint>(
				core: core,
				coder: coder,
				modules: modules,
				phenotype: phenotype,
				evaluationStrategy: new GenerationCountEvaluator<GraphColoringProblem, GraphColoring, uint>(20)
			);
		}

		// Multipoint
		/*(Func<GraphColoringProblem, GaManager<GraphColoringProblem, GraphColoring, uint>> creator, string name)[] gas = [
			((problem) =>
			{
				var modificator = new GraphColoringDomainModificator(problem, coder);
				uint phenotype(Genotype genotype) => problem.Criterion(coder.Decode(genotype));

				var modules = new GaModules<uint>(
					PopulationGenerator: new ProblemSolverPopulationGenerator<GraphColoringProblem, GraphColoring, uint>(
						problem,
						new GreedySequentialColoring(new RandomizedLfOrdering(rng)),
						coder
					),
					PairSelector: new RandomPairSelector(rng),
					Mutation: new GeneMutation(rng),
					Crossover: new MultipointCrossover(2),
					Selection: new BetaTournament<uint>(rng, 2)
				);

				var parameters = CreateParameters(problem);

				IGaIteration core = new GaCore<uint>(
					rng,
					parameters,
					modules.Crossover,
					modules.Mutation,
					modules.Selection,
					phenotype
				);
				core = new ModificationInfusedGaIteration<uint>(core, modificator);

				return new GaManager<GraphColoringProblem, GraphColoring, uint>(
					core: core,
					coder: coder,
					modules: modules,
					phenotype: phenotype,
					evaluationStrategy: new GenerationCountEvaluator<GraphColoringProblem, GraphColoring, uint>(20)
				);
			}, "Multipoint 2"),
			((problem) =>
			{
				var modificator = new GraphColoringDomainModificator(problem, coder);
				uint phenotype(Genotype genotype) => problem.Criterion(coder.Decode(genotype));

				var modules = new GaModules<uint>(
					PopulationGenerator: new ProblemSolverPopulationGenerator<GraphColoringProblem, GraphColoring, uint>(
						problem,
						new GreedySequentialColoring(new RandomizedLfOrdering(rng)),
						coder
					),
					PairSelector: new RandomPairSelector(rng),
					Mutation: new GeneMutation(rng),
					Crossover: new MultipointCrossover(5),
					Selection: new BetaTournament<uint>(rng, 2)
				);

				var parameters = CreateParameters(problem);

				IGaIteration core = new GaCore<uint>(
					rng,
					parameters,
					modules.Crossover,
					modules.Mutation,
					modules.Selection,
					phenotype
				);
				core = new ModificationInfusedGaIteration<uint>(core, modificator);

				return new GaManager<GraphColoringProblem, GraphColoring, uint>(
					core: core,
					coder: coder,
					modules: modules,
					phenotype: phenotype,
					evaluationStrategy: new GenerationCountEvaluator<GraphColoringProblem, GraphColoring, uint>(20)
				);
			}, "Multipoint 5"),
			((problem) =>
			{
				var modificator = new GraphColoringDomainModificator(problem, coder);
				uint phenotype(Genotype genotype) => problem.Criterion(coder.Decode(genotype));

				var modules = new GaModules<uint>(
					PopulationGenerator: new ProblemSolverPopulationGenerator<GraphColoringProblem, GraphColoring, uint>(
						problem,
						new GreedySequentialColoring(new RandomizedLfOrdering(rng)),
						coder
					),
					PairSelector: new RandomPairSelector(rng),
					Mutation: new GeneMutation(rng),
					Crossover: new MultipointCrossover(10),
					Selection: new BetaTournament<uint>(rng, 2)
				);

				var parameters = CreateParameters(problem);

				IGaIteration core = new GaCore<uint>(
					rng,
					parameters,
					modules.Crossover,
					modules.Mutation,
					modules.Selection,
					phenotype
				);
				core = new ModificationInfusedGaIteration<uint>(core, modificator);

				return new GaManager<GraphColoringProblem, GraphColoring, uint>(
					core: core,
					coder: coder,
					modules: modules,
					phenotype: phenotype,
					evaluationStrategy: new GenerationCountEvaluator<GraphColoringProblem, GraphColoring, uint>(20)
				);
			}, "Multipoint 10"),
		];*/

		// beta
		/*(Func<GraphColoringProblem, GaManager<GraphColoringProblem, GraphColoring, uint>> creator, string name)[] gas = [
			((problem) =>
			{
				var modificator = new GraphColoringDomainModificator(problem, coder);
				uint phenotype(Genotype genotype) => problem.Criterion(coder.Decode(genotype));

				var modules = new GaModules<uint>(
					PopulationGenerator: new ProblemSolverPopulationGenerator<GraphColoringProblem, GraphColoring, uint>(
						problem,
						new GreedySequentialColoring(new RandomizedLfOrdering(rng)),
						coder
					),
					PairSelector: new RandomPairSelector(rng),
					Mutation: new GeneMutation(rng),
					Crossover: new MultipointCrossover(2),
					Selection: new BetaTournament<uint>(rng, 2)
				);

				var parameters = CreateParameters(problem);

				IGaIteration core = new GaCore<uint>(
					rng,
					parameters,
					modules.Crossover,
					modules.Mutation,
					modules.Selection,
					phenotype
				);
				core = new ModificationInfusedGaIteration<uint>(core, modificator);

				return new GaManager<GraphColoringProblem, GraphColoring, uint>(
					core: core,
					coder: coder,
					modules: modules,
					phenotype: phenotype,
					evaluationStrategy: new GenerationCountEvaluator<GraphColoringProblem, GraphColoring, uint>(20)
				);
			}, "beta 2"),
			((problem) =>
			{
				var modificator = new GraphColoringDomainModificator(problem, coder);
				uint phenotype(Genotype genotype) => problem.Criterion(coder.Decode(genotype));

				var modules = new GaModules<uint>(
					PopulationGenerator: new ProblemSolverPopulationGenerator<GraphColoringProblem, GraphColoring, uint>(
						problem,
						new GreedySequentialColoring(new RandomizedLfOrdering(rng)),
						coder
					),
					PairSelector: new RandomPairSelector(rng),
					Mutation: new GeneMutation(rng),
					Crossover: new MultipointCrossover(2),
					Selection: new BetaTournament<uint>(rng, 3)
				);

				var parameters = CreateParameters(problem);

				IGaIteration core = new GaCore<uint>(
					rng,
					parameters,
					modules.Crossover,
					modules.Mutation,
					modules.Selection,
					phenotype
				);
				core = new ModificationInfusedGaIteration<uint>(core, modificator);

				return new GaManager<GraphColoringProblem, GraphColoring, uint>(
					core: core,
					coder: coder,
					modules: modules,
					phenotype: phenotype,
					evaluationStrategy: new GenerationCountEvaluator<GraphColoringProblem, GraphColoring, uint>(20)
				);
			}, "beta 3"),
			((problem) =>
			{
				var modificator = new GraphColoringDomainModificator(problem, coder);
				uint phenotype(Genotype genotype) => problem.Criterion(coder.Decode(genotype));

				var modules = new GaModules<uint>(
					PopulationGenerator: new ProblemSolverPopulationGenerator<GraphColoringProblem, GraphColoring, uint>(
						problem,
						new GreedySequentialColoring(new RandomizedLfOrdering(rng)),
						coder
					),
					PairSelector: new RandomPairSelector(rng),
					Mutation: new GeneMutation(rng),
					Crossover: new MultipointCrossover(2),
					Selection: new BetaTournament<uint>(rng, 5)
				);

				var parameters = CreateParameters(problem);

				IGaIteration core = new GaCore<uint>(
					rng,
					parameters,
					modules.Crossover,
					modules.Mutation,
					modules.Selection,
					phenotype
				);
				core = new ModificationInfusedGaIteration<uint>(core, modificator);

				return new GaManager<GraphColoringProblem, GraphColoring, uint>(
					core: core,
					coder: coder,
					modules: modules,
					phenotype: phenotype,
					evaluationStrategy: new GenerationCountEvaluator<GraphColoringProblem, GraphColoring, uint>(20)
				);
			}, "beta 5"),
		];*/

		// mutations
		/*(Func<GraphColoringProblem, GaManager<GraphColoringProblem, GraphColoring, uint>> creator, string name)[] gas = [
			((problem) =>
			{
				var modificator = new GraphColoringDomainModificator(problem, coder);
				uint phenotype(Genotype genotype) => problem.Criterion(coder.Decode(genotype));

				var modules = new GaModules<uint>(
					PopulationGenerator: new ProblemSolverPopulationGenerator<GraphColoringProblem, GraphColoring, uint>(
						problem,
						new GreedySequentialColoring(new RandomizedLfOrdering(rng)),
						coder
					),
					PairSelector: new RandomPairSelector(rng),
					Mutation: new GeneMutation(rng),
					Crossover: new MultipointCrossover(2),
					Selection: new BetaTournament<uint>(rng, 2)
				);

				var parameters = CreateParameters(problem);

				IGaIteration core = new GaCore<uint>(
					rng,
					parameters,
					modules.Crossover,
					modules.Mutation,
					modules.Selection,
					phenotype
				);
				core = new ModificationInfusedGaIteration<uint>(core, modificator);

				return new GaManager<GraphColoringProblem, GraphColoring, uint>(
					core: core,
					coder: coder,
					modules: modules,
					phenotype: phenotype,
					evaluationStrategy: new GenerationCountEvaluator<GraphColoringProblem, GraphColoring, uint>(20)
				);
			}, "gene mutation"),
			((problem) =>
			{
				var modificator = new GraphColoringDomainModificator(problem, coder);
				uint phenotype(Genotype genotype) => problem.Criterion(coder.Decode(genotype));

				var modules = new GaModules<uint>(
					PopulationGenerator: new ProblemSolverPopulationGenerator<GraphColoringProblem, GraphColoring, uint>(
						problem,
						new GreedySequentialColoring(new RandomizedLfOrdering(rng)),
						coder
					),
					PairSelector: new RandomPairSelector(rng),
					Mutation: new SwappingMutation(rng),
					Crossover: new MultipointCrossover(2),
					Selection: new BetaTournament<uint>(rng, 2)
				);

				var parameters = CreateParameters(problem);

				IGaIteration core = new GaCore<uint>(
					rng,
					parameters,
					modules.Crossover,
					modules.Mutation,
					modules.Selection,
					phenotype
				);
				core = new ModificationInfusedGaIteration<uint>(core, modificator);

				return new GaManager<GraphColoringProblem, GraphColoring, uint>(
					core: core,
					coder: coder,
					modules: modules,
					phenotype: phenotype,
					evaluationStrategy: new GenerationCountEvaluator<GraphColoringProblem, GraphColoring, uint>(20)
				);
			}, "swapping mutation"),
			((problem) =>
			{
				var modificator = new GraphColoringDomainModificator(problem, coder);
				uint phenotype(Genotype genotype) => problem.Criterion(coder.Decode(genotype));

				var modules = new GaModules<uint>(
					PopulationGenerator: new ProblemSolverPopulationGenerator<GraphColoringProblem, GraphColoring, uint>(
						problem,
						new GreedySequentialColoring(new RandomizedLfOrdering(rng)),
						coder
					),
					PairSelector: new RandomPairSelector(rng),
					Mutation: new FullMutation(),
					Crossover: new MultipointCrossover(2),
					Selection: new BetaTournament<uint>(rng, 2)
				);

				var parameters = CreateParameters(problem);

				IGaIteration core = new GaCore<uint>(
					rng,
					parameters,
					modules.Crossover,
					modules.Mutation,
					modules.Selection,
					phenotype
				);
				core = new ModificationInfusedGaIteration<uint>(core, modificator);

				return new GaManager<GraphColoringProblem, GraphColoring, uint>(
					core: core,
					coder: coder,
					modules: modules,
					phenotype: phenotype,
					evaluationStrategy: new GenerationCountEvaluator<GraphColoringProblem, GraphColoring, uint>(20)
				);
			}, "full mutation"),
		];*/

		// iterations 1
		/*(Func<GraphColoringProblem, GaManager<GraphColoringProblem, GraphColoring, uint>> creator, string name)[] gas = [
			((problem) =>
			{
				var modificator = new GraphColoringDomainModificator(problem, coder);
				uint phenotype(Genotype genotype) => problem.Criterion(coder.Decode(genotype));

				var modules = new GaModules<uint>(
					PopulationGenerator: new ProblemSolverPopulationGenerator<GraphColoringProblem, GraphColoring, uint>(
						problem,
						new GreedySequentialColoring(new RandomizedLfOrdering(rng)),
						coder
					),
					PairSelector: new RandomPairSelector(rng),
					Crossover: new ModificationInfusedCrossover(new MultipointCrossover(2), modificator),
					Mutation: new ModificationInfusedMutation(new GeneMutation(rng), modificator),
					Selection: new BetaTournament<uint>(rng, 2)
				);

				var parameters = CreateParameters(problem);

				IGaIteration core = new GaCore<uint>(
					rng,
					parameters,
					modules.Crossover,
					modules.Mutation,
					modules.Selection,
					phenotype
				);

				return new GaManager<GraphColoringProblem, GraphColoring, uint>(
					core: core,
					coder: coder,
					modules: modules,
					phenotype: phenotype,
					evaluationStrategy: new GenerationCountEvaluator<GraphColoringProblem, GraphColoring, uint>(20)
				);
			}, "each op"),
			((problem) =>
			{
				var modificator = new GraphColoringDomainModificator(problem, coder);
				uint phenotype(Genotype genotype) => problem.Criterion(coder.Decode(genotype));

				var modules = new GaModules<uint>(
					PopulationGenerator: new ProblemSolverPopulationGenerator<GraphColoringProblem, GraphColoring, uint>(
						problem,
						new GreedySequentialColoring(new RandomizedLfOrdering(rng)),
						coder
					),
					PairSelector: new RandomPairSelector(rng),
					Mutation: new GeneMutation(rng),
					Crossover: new MultipointCrossover(2),
					Selection: new BetaTournament<uint>(rng, 2)
				);

				var parameters = CreateParameters(problem);

				IGaIteration core = new GaCore<uint>(
					rng,
					parameters,
					modules.Crossover,
					modules.Mutation,
					modules.Selection,
					phenotype
				);
				core = new ModificationInfusedGaIteration<uint>(core, modificator);

				return new GaManager<GraphColoringProblem, GraphColoring, uint>(
					core: core,
					coder: coder,
					modules: modules,
					phenotype: phenotype,
					evaluationStrategy: new GenerationCountEvaluator<GraphColoringProblem, GraphColoring, uint>(20)
				);
			}, "every it"),
			((problem) =>
			{
				var modificator = new GraphColoringDomainModificator(problem, coder);
				uint phenotype(Genotype genotype) => problem.Criterion(coder.Decode(genotype));

				var modules = new GaModules<uint>(
					PopulationGenerator: new ProblemSolverPopulationGenerator<GraphColoringProblem, GraphColoring, uint>(
						problem,
						new GreedySequentialColoring(new RandomizedLfOrdering(rng)),
						coder
					),
					PairSelector: new RandomPairSelector(rng),
					Mutation: new GeneMutation(rng),
					Crossover: new MultipointCrossover(2),
					Selection: new BetaTournament<uint>(rng, 2)
				);

				var parameters = CreateParameters(problem);

				IGaIteration core = new GaCore<uint>(
					rng,
					parameters,
					modules.Crossover,
					modules.Mutation,
					modules.Selection,
					phenotype
				);
				core = new ModificationInfusedGaIteration<uint>(core, modificator, 3);

				return new GaManager<GraphColoringProblem, GraphColoring, uint>(
					core: core,
					coder: coder,
					modules: modules,
					phenotype: phenotype,
					evaluationStrategy: new GenerationCountEvaluator<GraphColoringProblem, GraphColoring, uint>(20)
				);
			}, "every 4 it"),
		];*/

		// iteration 2
		(Func<GraphColoringProblem, GaManager<GraphColoringProblem, GraphColoring, uint>> creator, string name)[] gas = [
			((problem) =>
			{
				var modificator = new GraphColoringDomainModificator(problem, coder);
				uint phenotype(Genotype genotype) => problem.Criterion(coder.Decode(genotype));

				var modules = new GaModules<uint>(
					PopulationGenerator: new ProblemSolverPopulationGenerator<GraphColoringProblem, GraphColoring, uint>(
						problem,
						new GreedySequentialColoring(new RandomizedLfOrdering(rng)),
						coder
					),
					PairSelector: new RandomPairSelector(rng),
					Mutation: new GeneMutation(rng),
					Crossover: new MultipointCrossover(2),
					Selection: new BetaTournament<uint>(rng, 2)
				);

				var parameters = CreateParameters(problem);

				IGaIteration core = new GaCore<uint>(
					rng,
					parameters,
					modules.Crossover,
					modules.Mutation,
					modules.Selection,
					phenotype
				);
				core = new ModificationInfusedGaIteration<uint>(core, modificator, 19);

				return new GaManager<GraphColoringProblem, GraphColoring, uint>(
					core: core,
					coder: coder,
					modules: modules,
					phenotype: phenotype,
					evaluationStrategy: new GenerationCountEvaluator<GraphColoringProblem, GraphColoring, uint>(20)
				);
			}, "in criterion with later validation"),
		];

		Research(
			specialGraphs.Select(
				graph => (
					GraphCollection.LoadSpecialGraph(graph.name, graph.vertexCount),
					graph.name
				)
			),
			gas
		);
	}

	private static void ItWasAThingSomeday()
	{
		var problem = new GraphColoringProblem(GraphCollection.LoadGraph("generic", "small", "high", 1));
		var rng = new Rng();
		var coder = new GraphColoringCoder();
		var modificator = new GraphColoringDomainModificator(problem, coder);
		uint phenotype(Genotype genotype) => problem.Criterion(coder.Decode(genotype));

		var parameters = new GaParameters(
			PopulationSize: problem.Graph.VertexCount switch
			{
				50 => 40,
				100 => 70,
				250 => 180,
				_ => problem.Graph.VertexCount / 2
			},
			MutationRate: 0.05,
			CrossoverRate: 0.95,
			GenerationalOverlapRatio: 0.5,
			UseElitistStrategy: true
		);

		var modules = new GaModules<uint>(
			PopulationGenerator: new ProblemSolverPopulationGenerator<GraphColoringProblem, GraphColoring, uint>(
				problem,
				// new GreedySequentialColoring(new RandomizedLfOrdering(rng)),
				new RandomColoringGenerator(rng),
				coder),
			PairSelector:
				new RandomPairSelector(rng),
			Mutation:
					//new ModificationInfusedMutation(
					new GeneMutation(rng),
			//new SwappingMutation(rng),
			//modificator
			//),
			Crossover:
					//new ModificationInfusedCrossover(
					new MultipointCrossover(10),
			//modificator
			//),
			Selection:
				new BetaTournament<uint>(rng, 2)
		);

		IGaIteration core = new GaCore<uint>(
			rng,
			parameters,
			modules.Crossover,
			modules.Mutation,
			modules.Selection,
			phenotype
		);
		core = new ModificationInfusedGaIteration<uint>(core, modificator);

		var ga = new GaManager<GraphColoringProblem, GraphColoring, uint>(
			core: core,
			coder: coder,
			modules: modules,
			phenotype: phenotype,
			//evaluationStrategy: new NoProgressEvaluator<GraphColoringProblem, GraphColoring, uint>(200)
			evaluationStrategy: new GenerationCountEvaluator<GraphColoringProblem, GraphColoring, uint>(100)
		);

		GraphColoring solution = ga.FindSolution(problem);
		Console.WriteLine($"всего итераций: {ga.Statistics.TotalGenerationCount}");
		Console.WriteLine("Решение:");
		Console.WriteLine($"- color count: {solution.ColorCount}");
		Console.WriteLine($"- is valid: {problem.Set.Has(solution)}");

		Console.WriteLine("\nДругие способы решения:");
		Console.WriteLine($"- LfOrdering: {new GreedySequentialColoring(new LfOrdering()).FindSolution(problem).ColorCount}");
		// Console.WriteLine($"- RsOrdering: {new GreedySequentialColoring(new RsOrdering()).FindSolution(problem).ColorCount}");
		Console.WriteLine($"- SlOrdering: {new GreedySequentialColoring(new SlOrdering()).FindSolution(problem).ColorCount}");
	}

	private static void Research(IEnumerable<(Graph, string)> graphs, IEnumerable<(Func<GraphColoringProblem, GaManager<GraphColoringProblem, GraphColoring, uint>> creator, string name)> gaCreators)
	{
		var graphStats = new ConcurrentDictionary<string, ConcurrentBag<Stats>>();

		Parallel.ForEach(graphs, anotherThing =>
		{
			var (graph, graphName) = anotherThing;
			graphStats[graphName] = [];

			Parallel.ForEach(gaCreators, thing =>
			{
				var (gaCreator, name) = thing;

				var bests = new ConcurrentBag<GraphColoring>();
				var problem = new GraphColoringProblem(graph);

				Parallel.For(0, 40, (_) =>
				{
					var ga = gaCreator(problem);
					var solution = ga.FindSolution(problem);
					bests.Add(solution);
				});

				var valid = bests.Where(coloring => problem.Set.Has(coloring));

				var stats = new Stats
				{
					TestName = name,
					TheBest = bests.MinBy(problem.Criterion),
					AvgColorCount = bests.Average(best => (double) problem.Criterion(best)),
					LowestColorCount = bests.Min(problem.Criterion),
					HighestColorCount = bests.Max(problem.Criterion),
					InvalidCount = (uint) bests.Where(coloring => !problem.Set.Has(coloring)).Count()
				};

				graphStats[graphName].Add(stats);
			});
		});

		var sb = new StringBuilder();

        foreach (var (key, value) in graphStats)
        {
			sb.AppendLine($"\n\nГраф '{key}'");
			Console.WriteLine($"\n\nГраф '{key}'");
            foreach (var stat in value)
            {
                sb.AppendLine($"ЭГА '{stat.TestName}");
				Console.WriteLine($"ЭГА '{stat.TestName}");
				sb.AppendLine($"*) Лучший из лучших: {stat.TheBest}");
				Console.WriteLine($"*) Лучший из лучших: {stat.TheBest}");
				sb.AppendLine($"*) Среднее значение приспособленности: {stat.AvgColorCount}");
				Console.WriteLine($"*) Среднее значение приспособленности: {stat.AvgColorCount}");
				sb.AppendLine($"*) Лучшая приспособленность: {stat.LowestColorCount}");
				Console.WriteLine($"*) Лучшая приспособленность: {stat.LowestColorCount}");
				sb.AppendLine($"*) Худшая приспособленность: {stat.HighestColorCount}");
				Console.WriteLine($"*) Худшая приспособленность: {stat.HighestColorCount}");
				sb.AppendLine($"*) Количество невалидных решений: {stat.InvalidCount}");
				Console.WriteLine($"*) Количество невалидных решений: {stat.InvalidCount}");
			}
		}

		File.WriteAllText(
			Path.Join(
				@"D:\msiotnihs\Developer\real_projects\ega_lab10\",
				$"report_2024.01.22_{TimeOnly.FromDateTime(DateTime.Now).ToString().Replace(':', '.')}_iterations_2.txt"),
			sb.ToString());
	}

	private record Stats
	{
		public required string TestName { get; init; }
		public required GraphColoring TheBest { get; init; }
		public required double AvgColorCount { get; init; }
		public required uint LowestColorCount { get; init; }
		public required uint HighestColorCount { get; init; }
		public required uint InvalidCount { get; init; }
	}
}
