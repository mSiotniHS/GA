using GA.Core;

namespace App;

internal interface IGenotypeModificator
{
    Genotype Modify(Genotype genotype);
}
