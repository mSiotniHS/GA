using System.Numerics;

namespace OptimizationProblemsFramework;

/// <summary>
/// Виды экстремумов
/// </summary>
public enum Extremum
{
    Maximum,
    Minimum
}

/// <summary>
/// Описывает проблему оптимизации. Проблема оптимизации задаётся:
/// <list type="bullet">
/// <item><description>типом экстремума (мин/макс)</description></item>
/// <item><description>множеством, на котором ищутся объекты</description></item>
/// <item><description>критерием, по которому оценивается объект</description></item>
/// </list>
/// </summary>
/// <typeparam name="TPreimage">Тип объекта из множества</typeparam>
/// <typeparam name="TNumber">Тип возвращаемого критерием числа</typeparam>
public interface IOptimizationProblem<TPreimage, out TNumber>
    where TNumber : INumber<TNumber>
{
    /// <summary>
    /// Тип экстремума задачи оптимизации (мин/макс)
    /// </summary>
    Extremum ExtremumOption { get; }

    /// <summary>
    /// Множество, на котором задана задача оптимизации
    /// </summary>
    ISet<TPreimage> Set { get; }

    /// <summary>
    /// Критерий задачи оптимизации
    /// </summary>
    /// <param name="preimage">Объект, для которого рассчитывается критерий</param>
    /// <returns></returns>
    TNumber Criterion(TPreimage preimage);
}
