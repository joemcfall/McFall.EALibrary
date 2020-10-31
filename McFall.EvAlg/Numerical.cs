using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McFall.EvAlg
{
    public static class Numerical
    {
        [ThreadStatic]
        static Random r = new Random();

        public static Random RNG
        {
            get
            {
                if (r == null)
                    r = new Random();
                return r;
            }
        }
    }
}
