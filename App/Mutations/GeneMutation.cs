using Common;
using GA.Core;

namespace App.Mutations;

internal class GeneMutation(IRng rng) : IMutation
{
    public Genotype Perform(Genotype genotype)
    {
  //      var mutant = new Genotype(genotype);
  //      var geneToMutate = rng.GetInt(mutant.Length);

  //      int newValue;
  //      do
  //      {
  //          newValue = rng.GetInt(mutant.Length);
  //      } while (newValue == mutant[geneToMutate]);

		//mutant[geneToMutate] = newValue;

  //      return mutant;

        // OR

        var mutant = new Genotype(genotype);
        var geneToMutate = rng.GetInt(mutant.Length);
        mutant[geneToMutate] = (mutant.Length - 1) - mutant[geneToMutate];
        return mutant;
    }
}
