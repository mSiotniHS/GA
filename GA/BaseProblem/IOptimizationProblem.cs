using System.Numerics;

namespace GA.BaseProblem;

/// <summary>
/// Интерфейс, определяющий класс, содержащий
/// информацию о задачи оптимизации.
/// Задача оптимизации задаётся критерием.
/// </summary>
/// <typeparam name="TBase">Тип элемента допустимого множества</typeparam>
/// <typeparam name="TNumber">Тип возвращаемого численного значения критерия</typeparam>
public interface IOptimizationProblem<in TBase, out TNumber>
	where TNumber : INumber<TNumber>
{
	public TNumber Criterion(TBase value);
}
