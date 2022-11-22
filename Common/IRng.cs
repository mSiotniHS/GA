namespace Common;

public interface IRng
{
	public int GetInt();

	/// <param name="max">Не включительно</param>
	/// <returns>int в [0, max)</returns>
	public int GetInt(int max);

	/// <param name="min">Включительно</param>
	/// <param name="max">Не включительно</param>
	/// <returns>int в [min, max)</returns>
	public int GetInt(int min, int max);

	/// <returns>double в [0, 1)</returns>
	public double GetDouble();
}
