using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronStar.UI.Attributes
{
    public abstract class ControlAttribute : Attribute
    {
        public UIFlags Flags { get; protected set; }
        public int Order { get; protected set; }
        public string Name { get; protected set; }
    }
}
