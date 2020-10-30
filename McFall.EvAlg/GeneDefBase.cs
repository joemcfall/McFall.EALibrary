using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McFall.EvAlg
{

    [Serializable]
    public abstract class GeneDefBase
    {
        protected GeneDefBase()
        {
            Identifier = Guid.NewGuid();
        }

        public abstract Type ObjectType { get; }

        public string Description { get; set; }

        public Guid Identifier { get; private set; }

        public Func<object> ValueInitializer { get; set; }

    }

}
