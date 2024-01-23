using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphColoring.Math;

public readonly record struct Color(int Id)
{
    public Color Next() => new(Id + 1);
    public static Color Smallest() => new(0);
}

public sealed class GraphColoring : IDeepCloneable<GraphColoring>
{
    private readonly Dictionary<Vertex, Color> _coloring;

    public IReadOnlyDictionary<Vertex, Color> Mapping => _coloring;

    public GraphColoring()
    {
        _coloring = [];
    }

    public Color? this[Vertex v]
    {
        get => _coloring.TryGetValue(v, out Color item) ? item : null;
        set
        {
            switch (value, _coloring.ContainsKey(v))
            {
                case (null, false): break;
                case (null, true): _coloring.Remove(v); break;
                case ({ } someValue, _): _coloring[v] = someValue; break;
            }
        }
    }

    public Color GetOrThrow(Vertex v)
    {
        var isSuccessful = _coloring.TryGetValue(v, out Color color);
        if (!isSuccessful) throw new ArgumentOutOfRangeException(nameof(v), "");

        return color;
    }

    public IEnumerable<Color> Colors => _coloring.Values.Distinct();

    public uint ColorCount => (uint)Colors.Count();

    public GraphColoring DeepClone()
    {
        var clone = new GraphColoring();
        foreach ((Vertex vertex, Color color) in _coloring)
        {
            clone[vertex] = color;
        }

        return clone;
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.Append("GraphColoring { ");
        foreach ((Vertex vertex, Color color) in _coloring)
        {
            builder.Append($"[{vertex.Id}]: {color.Id}, ");
        }
        builder.Append(" }");

        return builder.ToString();
    }
}
