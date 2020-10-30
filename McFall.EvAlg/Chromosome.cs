using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McFall.EvAlg
{
    [Serializable]
    public abstract class Chromosome
    {
        protected List<Gene> _genes = new List<Gene>();

        public virtual void AddGene(Gene g)
        {
            _genes.Add(g);
        }

        public virtual Gene GetGene(Guid id)
        {
            return (from g in _genes
                    where g.Definition.Identifier == id
                    select g).FirstOrDefault();
        }

        public IEnumerable<GeneDefBase> GetGeneDefs()
        {
            foreach (var g in _genes)
                yield return g.Definition;
        }

        public void InitializeValues()
        {
            foreach (var g in _genes)
            {
                g.InitializeValue();
            }
        }
    }

    [Serializable]
    public class Chromosome<T> : Chromosome
    {
        public IEnumerable<Gene<T>> Genes
        {
            get
            {
                foreach (var g in _genes)
                    if (g is Gene<T>)
                        yield return g as Gene<T>;
            }
        }
    }

}
