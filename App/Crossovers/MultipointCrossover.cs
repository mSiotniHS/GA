using GA.Core;

namespace App.Crossovers;

internal class MultipointCrossover : ICrossover
{
    private readonly uint _pointCount;

    public MultipointCrossover(uint pointCount)
    {
        if (pointCount == 0)
        {
            throw new ArgumentException("Point count should be greater than 0", nameof(pointCount));
        }

        _pointCount = pointCount;
    }

    public IEnumerable<Genotype> Perform(Genotype parent1, Genotype parent2)
    {
        var points = Enumerable
            .Range(0, parent1.Length - 1)
            .OrderBy(_ => Random.Shared.Next())
            .Take((int)_pointCount)
            .Order()
            .Select(el => (uint)el)
            .ToArray();

        var child1 = CoreOperation(parent1, parent2, points);
        var child2 = CoreOperation(parent2, parent1, points);

        return [child1, child2];
    }

    private static Genotype CoreOperation(Genotype parent1, Genotype parent2, uint[] points)
    {
        var child = new Genotype(parent1.Length);
        var takeSecondsGenes = false;
        var pointIdx = 0;

        for (var i = 0; i < parent1.Length; i++)
        {
            var currentParent = takeSecondsGenes ? parent2 : parent1;
            child[i] = currentParent.GetNonNull(i);

            if (pointIdx == points.Length) continue;

            if (i == points[pointIdx])
            {
                pointIdx++;
                takeSecondsGenes = !takeSecondsGenes;
            }
        }

        return child;
    }
}
