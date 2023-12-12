namespace Common;

/// <summary>
/// Интерфейс задаёт общий вид класса,
/// генерирующего случайные числа.
/// </summary>
public interface IRng
{
	/// <param name="max">Не включительно</param>
	/// <returns>int в [0, max)</returns>
	public int GetInt(int max);

	/// <param name="min">Включительно</param>
	/// <param name="max">Не включительно</param>
	/// <returns>int в [min, max)</returns>
	public int GetInt(int min, int max);

	/// <returns>Число типа double в промежутке [0, 1)</returns>
	public double GetDouble();
}
