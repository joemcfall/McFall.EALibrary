using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McFall.EvAlg
{
    public static class PopulationExtensions
    {
        public static double AverageFitness(this IEnumerable<Individual> individuals)
        {
            return (from i in individuals
                    select i.Fitness).Average();
        }

        public static IEnumerable<Individual> Evolve(this IEnumerable<Individual> individuals, int children,
        Func<IEnumerable<Individual>, Tuple<Individual, Individual>> parentSelector
        )
        {
            int count = individuals.Count();
            List<Individual> inds;

            if (parentSelector == SelectionMethods.WeightedChoice
                && count == children)
                inds = new List<Individual>();
            else
                inds = new List<Individual>(individuals);


            var r = Parallel.For(0, children, i =>
            {
                //select parents
                var parents = parentSelector(individuals);
                //generate children
                var child = Crossover(parents);
                child.Mutate();
                inds.Add(child);

                Individual.CalculateFitness(child);
            });

            
            //cull and return
            return (from i in inds
                    orderby i.Fitness descending
                    select i).Take(count);
        }

        static Individual Crossover(Tuple<Individual, Individual> parents)
        {
            var a = parents.Item1;
            var defs = a.GetGeneDefs();
            Individual kid = IndividualFactory.CreateIndividual(defs);
            //get the chromosomes and stuff
            foreach (var def in defs)
            {
                var aGene = a.GetGene(def.Identifier);
                var bGene = parents.Item2.GetGene(def.Identifier);
                var kidGene = kid.GetGene(def.Identifier);

                var t = def.GetType();


                var crossoverProperty = t.GetProperty("CrossoverMethod");
                if (crossoverProperty != null)
                {
                    Delegate d = crossoverProperty.GetValue(def, null) as Delegate;
                    if (d != null)
                    {
                        var aVal = Gene.GetValue(aGene);
                        var bVal = Gene.GetValue(bGene);

                        object val = d.DynamicInvoke(aVal, bVal);
                        Gene.SetValue(kidGene, val);
                    }
                }
            }

            return kid;
        }
    }
}
