using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronStar.UI.Attributes
{
    public class UIInfoAttribute : Attribute
    {
        public int Order { get; private set; }
        public string Name { get; private set; }

        public UIInfoAttribute(string name, int order)
        {
            this.Name = name;
            this.Order = order;
        }
    }
}
