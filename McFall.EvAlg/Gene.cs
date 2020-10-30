using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McFall.EvAlg
{

    [Serializable]
    public abstract class Gene
    {
        public GeneDefBase Definition { get; set; }

        public static object GetValue(Gene g)
        {
            var specificGeneType = g.GetType();
            var prop = specificGeneType.GetProperty("Value");
            return prop.GetValue(g, null);

        }

        public static void SetValue(Gene g, object value)
        {
            var specificGeneType = g.GetType();
            var t = specificGeneType.GetGenericArguments()[0];
            if (value.GetType() != t)
                return;
            else
            {
                var prop = specificGeneType.GetProperty("Value");
                prop.SetValue(g, value, null);
            }

        }

        public void InitializeValue()
        {
            if (Definition != null && Definition.ValueInitializer != null)
                SetValue(this, Definition.ValueInitializer());
        }
    }

    [Serializable]
    public class Gene<T> : Gene
    {

        public Gene()
        {
        }

        T _value = default(T);

        private GeneDef<T> GetDef()
        {
            return Definition as GeneDef<T>;
        }

        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                var d = GetDef();
                if (d != null)
                    _value = d.ValueConstrainer(value);
                else
                    _value = value;
            }
        }



    }

}
