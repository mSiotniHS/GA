using GA.Core;

namespace App;

internal class ModificationInfusedCrossover(ICrossover crossover, IGenotypeModificator modificator) : ICrossover
{
    public IEnumerable<Genotype> Perform(Genotype parent1, Genotype parent2) =>
        crossover.Perform(parent1, parent2).Select(modificator.Modify);
}
