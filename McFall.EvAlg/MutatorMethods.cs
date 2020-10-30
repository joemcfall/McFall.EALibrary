using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McFall.EALibrary
{
    public static class MutatorMethods
    {
        public static double MutationRate = 0.05;

        public static int RandomMutate(int val)
        {
            if (Numerical.RNG.NextDouble() < MutationRate)
            {
                //just generate a random number
                return Numerical.RNG.Next();
            }
            else
                return val;

        }

        public static BitVector32 RandomMutate(BitVector32 val)
        {
            //if (Numerical.RNG.NextDouble() < MutationRate)
            //{
            //    return new BitVector32(Numerical.RNG.Next());
            //}
            //else
            //    return val;
            var mask = BitVector32.CreateMask();
            for (int i = 0; i < 32; i++)
            {
                if (Numerical.RNG.NextDouble() < MutationRate)
                {
                    val[mask] = !val[mask];
                }

                if (i < 31)
                    mask = BitVector32.CreateMask(mask);
            }

            return val;
        }

        public static int MaxIncrement = 1;

        public static int IncrementMutate(int val)
        {
            int inc = 0;
            if (Numerical.RNG.NextDouble() <= MutationRate)
            {
                inc = Numerical.RNG.Next(MaxIncrement) + 1;
                if (Numerical.RNG.NextDouble() >= 0.5)
                    inc = -inc;
            }
            return val + inc;
        }

        public static double RandomMutate(double val)
        {
            if (Numerical.RNG.NextDouble() <= MutationRate)
                return Numerical.RNG.NextDouble();
            else
                return val;

        }

        public static double IncrementMutate(double val)
        {
            double rate = MutationRate;
            var inc = val * rate * Numerical.RNG.NextDouble();
            if (Numerical.RNG.NextDouble() > 0.5)
            {
                return val + inc;
            }
            else
            {
                return val - inc;
            }
        }
    }

}
