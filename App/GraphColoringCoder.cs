using GA.BaseProblem;
using GA.Core;
using GraphColoring.Math;

namespace App;

using GraphColoring = GraphColoring.Math.GraphColoring;

internal class GraphColoringCoder : IGaCoder<GraphColoring>
{
    public Genotype Encode(GraphColoring coloring)
    {
        var genotype = new Genotype(coloring.Mapping.Count);

        foreach (var (vertex, color) in coloring.Mapping)
        {
            genotype[vertex.Id] = color.Id;
        }

        return genotype;
    }
 
    public GraphColoring Decode(Genotype genotype)
    {
        var coloring = new GraphColoring();
        var genotypeArray = genotype.ToFilledArray();

        for (var i = 0; i < genotypeArray.Length; i++)
        {
            coloring[new Vertex(i)] = new Color(genotypeArray[i]);
        }

        return coloring;
    }
}
