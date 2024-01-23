using GraphColoring.Math;
using System.Collections.Generic;

namespace GraphColoring.Solvers;

public interface IOrderingStrategy
{
    public IEnumerable<Vertex> OrderVertices(IReadonlyGraph graph);
}
