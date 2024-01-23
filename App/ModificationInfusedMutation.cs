using GA.Core;

namespace App;

internal class ModificationInfusedMutation(IMutation mutation, IGenotypeModificator modificator) : IMutation
{
    public Genotype Perform(Genotype genotype) => modificator.Modify(mutation.Perform(genotype));
}
