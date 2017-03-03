using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronStar.UI.Attributes
{
    public class DescriptionAttribute : Attribute
    {
        public string Decsription { get; private set; }

        public DescriptionAttribute(string description)
        {
            this.Decsription = description;
        }
    }
}
