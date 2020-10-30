using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McFall.EALibrary
{

    public static class CrossoverMethods
    {
        ///This function just chooses one of the parents randomly for each
        ///gene in the child
        public static Individual RandomParentGene(Tuple<Individual, Individual> parents)
        {
            Individual a = parents.Item1;
            Individual b = parents.Item2;

            var geneDefs = a.GetGeneDefs();

            Individual child = IndividualFactory.CreateIndividual(geneDefs);

            foreach (var def in geneDefs)
            {
                var id = def.Identifier;
                var aVal = a.GetGene(id);
                var bVal = b.GetGene(id);
                var cGene = child.GetGene(id);

                Gene.SetValue(cGene, Numerical.RNG.NextDouble() >= 0.5 ? Gene.GetValue(aVal) : Gene.GetValue(bVal));

            }

            return child;

        }






        public static int NPoint = 1;
        public static Individual NPointCrossover(Tuple<Individual, Individual> parents)
        {
            Individual a = parents.Item1;
            Individual b = parents.Item2;

            var geneDefs = a.GetGeneDefs();
            int geneCount = geneDefs.Count();
            Individual child = IndividualFactory.CreateIndividual(geneDefs);

            int i = 0;
            bool takeA = true;
            List<int> randoms = new List<int>();
            foreach (var r in Enumerable.Range(0, NPoint))
            {
                int n = Numerical.RNG.Next(geneCount);
                if (!randoms.Contains(n))
                    randoms.Add(n);
            }



            foreach (var def in geneDefs)
            {
                var id = def.Identifier;
                var aVal = a.GetGene(id);
                var bVal = b.GetGene(id);

                object val = takeA ? Gene.GetValue(aVal) : Gene.GetValue(bVal);
                var cGene = child.GetGene(id);

                Gene.SetValue(cGene, val);
                i++;

                if (randoms.Contains(i))
                    takeA = !takeA;

            }

            return child;
        }
    }

}
