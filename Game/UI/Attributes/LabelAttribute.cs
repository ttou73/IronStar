using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronStar.UI.Attributes
{
    public class LabelAttribute : ControlAttribute
    {
        public string Text { get; private set; }
        public LabelAttribute(int order, string text)
        {
            this.Order = order;
            this.Text = text;
        }
    }
}
