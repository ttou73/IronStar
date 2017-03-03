using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronStar.UI.Attributes
{
    public class GeneratorApplicabilityAttribute : Attribute
    {

        public  IReadOnlyCollection<Type> Types { get { return types.ToList().AsReadOnly(); } }
        private Type[] types;

        public GeneratorApplicabilityAttribute(Type type)
        {
            types = new Type[] { type };
        }

        public GeneratorApplicabilityAttribute(Type[] types)
        {
            this.types = types;
        }
    }
}
