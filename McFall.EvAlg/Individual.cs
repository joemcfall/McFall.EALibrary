using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace McFall.EvAlg
{
    [Serializable]
    public class Individual : ISerializable
    {
        public Individual() { }

        public string Note { get; set; }
        List<Chromosome> _chromosomes = new List<Chromosome>();
        public void AddChromosome(Chromosome c)
        {
            _chromosomes.Add(c);
        }
        public IEnumerable<Chromosome> Chromosomes
        {
            get
            {
                foreach (var c in _chromosomes)
                    yield return c;
            }
        }

        public void InitializeValues()
        {
            foreach (var c in Chromosomes)
                c.InitializeValues();
        }

        public void Mutate()
        {
            foreach (var c in Chromosomes)
            {
                foreach (var g in c.GetGeneDefs())
                {
                    var gene = c.GetGene(g.Identifier);

                    var t = g.GetType();


                    var mutatorsProperty = t.GetProperty("Mutator");
                    if (mutatorsProperty != null)
                    {
                        Delegate d = mutatorsProperty.GetValue(g, null) as Delegate;
                        if (d != null)
                            Gene.SetValue(gene, d.Method.Invoke(d.Target, new object[] { Gene.GetValue(gene) }));
                    }
                }
            }
        }

        public Gene GetGene(Guid id)
        {
            return (from c in _chromosomes
                    let g = c.GetGene(id)
                    where g != null
                    select g).FirstOrDefault();
        }

        public IEnumerable<GeneDefBase> GetGeneDefs()
        {
            foreach (var c in _chromosomes)
                foreach (var g in c.GetGeneDefs())
                    yield return g;
        }

        public double Fitness { get; set; }

        public static Action<Individual> CalculateFitness { get; set; }

        public Individual(SerializationInfo info, StreamingContext context)
        {
        }

        static readonly string ChromPrefix = "CHROM";

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            //fitness
            info.AddValue("Fitness", Fitness);
            //note
            info.AddValue("Note", Note);
            //_chromosomes
            for (int i = 0; i < _chromosomes.Count; i++)
            {
                string name = string.Format("{0}:{1}", ChromPrefix, i);
                var c = _chromosomes[i];
                info.AddValue(name, c, typeof(Chromosome));
            }

        }
    }
}
