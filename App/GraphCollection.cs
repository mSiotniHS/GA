using GraphColoring.Math;

namespace App;

public static class GraphCollection
{
	private const string GraphCollectionAbsolutePath = @"D:\msiotnihs\Developer\real_projects\ega_lab10\App\graphs\";

	private static bool[] ReadFile(string fileName) =>
		File
			.ReadAllText($@"graphs\{fileName}.txt")
			.Trim()
			.Split(' ')
			.Select(raw => raw switch
			{
				"0" => false,
				"1" => true,
				_ => throw new Exception("Bad file format")
			})
			.ToArray();

	public static void GenerateAndSaveGraph(string fileName, int vertexCount, double density)
	{
		var data = GraphGenerator
			.GenerateData(vertexCount, density)
			.Select(x => x ? '1' : '0');
		var joined = string.Join(' ', data);

		File.WriteAllText(Path.Join(GraphCollectionAbsolutePath, $"{fileName}.txt"), joined);
	}

	public enum GraphSize { Small, Medium, Large }
	public enum GraphDensity { Low, Medium, High }

	private static string SizeToString(GraphSize size) =>
		size switch
		{
			GraphSize.Small => "small",
			GraphSize.Medium => "medium",
			GraphSize.Large => "large",
			_ => throw new ArgumentOutOfRangeException(nameof(size), size, null)
		};

	private static string DensityToString(GraphDensity density) =>
		density switch
		{
			GraphDensity.Low => "low",
			GraphDensity.Medium => "medium",
			GraphDensity.High => "high",
			_ => throw new ArgumentOutOfRangeException(nameof(density), density, null)
		};

	public static void GenerateAndSaveClassifiedGraph(int idx, GraphSize size, GraphDensity density)
	{
		var vertexCount = size switch
		{
			GraphSize.Small => 50,
			GraphSize.Medium => 100,
			GraphSize.Large => 250,
			_ => throw new ArgumentOutOfRangeException(nameof(size), size, null)
		};

		var densityFactor = density switch
		{
			GraphDensity.Low => 0.4,
			GraphDensity.Medium => 0.6,
			GraphDensity.High => 0.85,
			_ => throw new ArgumentOutOfRangeException(nameof(density), density, null)
		};

		var fileName = $"generic_{SizeToString(size)}_{DensityToString(density)}_{idx}";

		GenerateAndSaveGraph(fileName, vertexCount, densityFactor);
	}

	public static void GenerateAndSaveTree(int idx, GraphSize size)
	{
		var vertexCount = size switch
		{
			GraphSize.Small => 50,
			GraphSize.Medium => 100,
			GraphSize.Large => 250,
			_ => throw new ArgumentOutOfRangeException(nameof(size), size, null)
		};

		var fileName = $"tree_{SizeToString(size)}_{idx}";

		var tree = GraphGenerator.GenerateTree(vertexCount);
		var encoding = tree.ToString();

		File.WriteAllText(Path.Join(GraphCollectionAbsolutePath, $"{fileName}.txt"), encoding);
	}

	public static void GenerateAndSaveBipartiteGraph(int idx, GraphSize size, GraphDensity density)
	{
		var vertexCount = size switch
		{
			GraphSize.Small => 50,
			GraphSize.Medium => 100,
			GraphSize.Large => 250,
			_ => throw new ArgumentOutOfRangeException(nameof(size), size, null)
		};

		var densityFactor = density switch
		{
			GraphDensity.Low => 0.4,
			GraphDensity.Medium => 0.6,
			GraphDensity.High => 0.85,
			_ => throw new ArgumentOutOfRangeException(nameof(density), density, null)
		};

		var graph = Graph.Empty(vertexCount);
		var vertices = graph.VertexList().ToList();

		var firstPart = vertices.GetRange(0, Random.Shared.Next(1, vertexCount));
		var secondPart = vertices.GetRange(firstPart.Count, vertices.Count - firstPart.Count);

		foreach (var firstVertex in firstPart)
		{
			foreach (var secondVertex in secondPart)
			{
				if (Random.Shared.NextDouble() < densityFactor)
				{
					graph.AddEdge(firstVertex, secondVertex);
				}
			}
		}

		var encoding = graph.ToString();
		var fileName = $"bipartite_{SizeToString(size)}_{DensityToString(density)}_{idx}";

		File.WriteAllText(Path.Join(GraphCollectionAbsolutePath, $"{fileName}.txt"), encoding);
	}

	public static Graph LoadGraph(string type, string size, string? density, int idx)
	{
		var fileName = density switch
		{
			null => $"{type}_{size}_{idx}",
			{ } value => $"{type}_{size}_{density}_{idx}"
		};

		var data = ReadFile(fileName);
		return new Graph(new SymmetricMatrix<bool>(data, false));
	}

	public static Graph LoadSpecialGraph(string fileName, int vertexCount)
	{
		var lines = File.ReadAllLines(Path.Combine(GraphCollectionAbsolutePath, $@"special\{fileName}.col"));
		var graph = Graph.Empty(vertexCount);

		foreach (var line in lines)
		{
			var split = line.Split(' ').Select(int.Parse).ToArray();
			var from = split[0] - 1;
			var to = split[1] - 1;

			graph.AddEdge(new Vertex(from), new Vertex(to));
		}

		return graph;
	}
}
