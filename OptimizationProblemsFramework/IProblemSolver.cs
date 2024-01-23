using System.Numerics;

namespace OptimizationProblemsFramework;

/// <summary>
/// Описывает инструмент для поиска решения заданного типа задач оптимизации
/// </summary>
/// <typeparam name="TProblem">Проблема оптимизации, которую инструмент решает</typeparam>
/// <typeparam name="TPreimage">Объект из множества, на котором задача оптимизации определяется</typeparam>
/// <typeparam name="TNumber">Тип возвращаемого критерием задачи числа</typeparam>
public interface IProblemSolver<in TProblem, out TPreimage, out TNumber>
    where TProblem : IOptimizationProblem<TPreimage, TNumber>
    where TNumber : INumber<TNumber>
{
    /// <summary>
    /// Находит решение предоставляемой задачи оптимизации
    /// </summary>
    /// <param name="problem">Проблема оптимизации, решение которой находится</param>
    /// <returns>Объект-решение из множества (по задумке), на котором задана задача</returns>
    TPreimage FindSolution(TProblem problem);
}
