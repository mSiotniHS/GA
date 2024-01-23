using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphColoring.Math;

public class SymmetricMatrix<T>
{
    private readonly T[] _data;
    private readonly T _defaultValue;

    public int RowCount { get; }
    public T[] RawData => _data;

    public SymmetricMatrix(int rowCount, IReadOnlyCollection<T> data, T defaultValue)
    {
        RowCount = rowCount;
        if (data.Count != RowCount * (RowCount - 1) / 2)
            throw new ArgumentException($"[{nameof(SymmetricMatrix<T>)}/ctor] Длина data и rowCount не соотносятся");
        _data = [.. data];
        _defaultValue = defaultValue;
    }

    public SymmetricMatrix(IEnumerable<T> data, T defaultValue)
    {
        _data = data.ToArray();
        _defaultValue = defaultValue;
        var rawRowCount = 0.5 * (System.Math.Sqrt(8 * _data.Length + 1) + 1);
        if (!double.IsInteger(rawRowCount))
        {
            throw new ArgumentException($"[{nameof(SymmetricMatrix<T>)}/ctor] Bad data.Length");
        }

        RowCount = (int)System.Math.Round(rawRowCount);
    }

    // public static SymmetricMatrix<T> Zero(int rowCount, T defaultValue)
    // {
    // 	
    // }

    private int ConvertIndex(int i, int j)
    {
        if (i > j) (i, j) = (j, i);

        var idx = j - i - 1;
        var jump = Convert.ToInt32(ApSum(RowCount - 1, -1, i));

        return jump + idx;
    }

    public T this[int row, int column]
    {
        get =>
            row == column
                ? _defaultValue
                : _data[ConvertIndex(row, column)];

        set
        {
            if (row == column)
                throw new ArgumentException("Нельзя изменить значение элементов на главной диагонали");

            _data[ConvertIndex(row, column)] = value;
        }
    }

    private static double ApSum(double first, double difference, int n) =>
        (2 * first + difference * (n - 1)) / 2 * n;

    public override string ToString()
    {
        var sb = new StringBuilder();
        for (var i = 0; i < RowCount; i++)
        {
            for (var j = 0; j < RowCount; j++)
            {
                sb.Append($"{this[i, j]} ");
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }
}
