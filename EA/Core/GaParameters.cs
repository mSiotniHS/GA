namespace EA.Core;

/// <summary>
/// Record <c>GaParameters</c> содержит параметры ЭГА.
/// </summary>
public record GaParameters(
	int PopulationSize,
	double MutationRate,
	double CrossoverRate,
	double GenerationalOverlapRatio,
	bool UseElitistStrategy)
{
	/// <summary>
	/// Размер популяции.
	/// </summary>
	public int PopulationSize { get; } = PopulationSize;

	/// <summary>
	/// Вероятность, с которой пройзойдёт мутация конкретной особи.
	///<remarks>
	/// Принимает значение от 0 до 1.
	/// </remarks>
	/// </summary>
	public double MutationRate { get; } = MutationRate;

	/// <summary>
	/// Вероятность, с которой у двух родителей будет потомство.
	/// <remarks>
	/// Принимает значение от 0 до 1.
	/// </remarks>
	/// </summary>
	public double CrossoverRate { get; } = CrossoverRate;

	/// <summary>
	/// Доля заменяемых особей при селекции. При 1 все особи
	/// поколения заменятся новыми из репродукционного множества.
	/// </summary>
	public double GenerationalOverlapRatio { get; } = GenerationalOverlapRatio;

	/// <summary>
	/// Использовать ли элитарную стратегию при формировании
	/// нового поколения.
	/// </summary>
	public bool UseElitistStrategy { get; } = UseElitistStrategy;
}
