using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McFall.EvAlg
{
    public static class IndividualFactory
    {
        public static Individual CreateIndividual(IEnumerable<GeneDefBase> geneDefs)
        {
            //we actually need to group these by object type
            var groups = from g in geneDefs
                         group g by g.ObjectType into x
                         select x;

            Individual ind = new Individual();


            foreach (var g in groups)
            {
                var genericGeneType = typeof(Gene<>);
                var specificGeneType = genericGeneType.MakeGenericType(g.Key);

                var genericChromsomeType = typeof(Chromosome<>);
                var specificChromosomeType = genericChromsomeType.MakeGenericType(g.Key);
                var chromosome = Activator.CreateInstance(specificChromosomeType) as Chromosome;

                foreach (var geneDef in g)
                {
                    var gene = Activator.CreateInstance(specificGeneType) as Gene;
                    gene.Definition = geneDef;

                    chromosome.AddGene(gene);

                }

                ind.AddChromosome(chromosome);

            }


            return ind;
        }
    }

}
