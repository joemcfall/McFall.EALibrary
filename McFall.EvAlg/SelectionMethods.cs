using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McFall.EALibrary
{

    public static class SelectionMethods
    {
        public static Tuple<Individual, Individual> RandomChoice(IEnumerable<Individual> population)
        {
            List<Individual> pops = new List<Individual>(population);
            int a = Numerical.RNG.Next(pops.Count);
            int b = Numerical.RNG.Next(pops.Count);
            return new Tuple<Individual, Individual>(pops[a], pops[b]);
        }

        public static Tuple<Individual, Individual> WeightedChoice(IEnumerable<Individual> population)
        {
            List<Individual> pops = new List<Individual>(population);
            var fitnessSum = (double)pops.Sum(i => i.Fitness);

            Individual a = null;
            Individual b = null;

            for (int k = 0; k < 2; k++)
            {
                double chance = Numerical.RNG.NextDouble();
                chance *= fitnessSum;

                double checkSum = 0.0;

                Individual theInd = null;
                for (int i = 0; i < pops.Count; i++)
                {

                    theInd = pops[i];
                    checkSum += theInd.Fitness;
                    if (checkSum >= chance)
                        break;
                }

                if (k == 0)
                    a = theInd;
                else
                    b = theInd;
            }
            return new Tuple<Individual, Individual>(a, b);
        }
    }

}
