using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McFall.EALibrary
{
    [Serializable]
    public class GeneDef<T> : GeneDefBase
    {
        protected GeneDef() : base()
        {
            ValueConstrainer = t => t;
            Mutator = t => t;

        }

        public static GeneDef<T> Create()
        {
            return new GeneDef<T>();
        }



        public override Type ObjectType
        {
            get { return typeof(T); }
        }

        public Func<T, T> Mutator { get; set; }
        public Func<T, T> ValueConstrainer { get; set; }
        public Func<T, T, T> CrossoverMethod { get; set; }

    }

}
