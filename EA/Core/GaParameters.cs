namespace EA.Core;

/// <summary>
/// Record <c>GaParameters</c> содержит параметры ЭГА.
/// </summary>
public record GaParameters
{
	/// <summary>
	/// Размер популяции.
	/// </summary>
	public int PopulationSize { get; set; }

	/// <summary>
	/// Вероятность, с которой пройзойдёт мутация конкретной особи.
	///<remarks>
	/// Принимает значение от 0 до 1.
	/// </remarks>
	/// </summary>
	public double MutationRate { get; set; }

	/// <summary>
	/// Вероятность, с которой у двух родителей будет потомство.
	/// <remarks>
	/// Принимает значение от 0 до 1.
	/// </remarks>
	/// </summary>
	public double CrossoverRate { get; set; }

	/// <summary>
	/// Доля заменяемых особей при селекции. При 1 все особи
	/// поколения заменятся новыми из репродукционного множества.
	/// </summary>
	public double GenerationalPOverlapRatio { get; set; }

	/// <summary>
	/// Использовать ли элитарную стратегию при формировании
	/// нового поколения.
	/// </summary>
	public bool UseElitistStrategy { get; set; }
}
