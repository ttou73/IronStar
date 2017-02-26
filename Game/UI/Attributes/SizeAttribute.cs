using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronStar.UI.Attributes
{
    public class SizeAttribute : Attribute
    {
        public string Width { get; private set; }
        public string Height { get; private set; }

        public SizeAttribute(string width, string height)
        {
            this.Width = width;
            this.Height = height;
        }

        public int GetWidth(int width)
        {
            return Parse(Width, width);
        }

        public int GetHeight(int height)
        {
            return Parse(Height, height);
        }


        private int Parse(string value, int side)
        {
            if (char.IsDigit(value[0]))
            {
                if (char.IsDigit(value[value.Length - 1]))
                {
                    return int.Parse(value);
                }
                else
                {
                    float modificator = float.Parse(value.Substring(0, value.Length - 1)) / 100f;
                    return (int)(side * modificator);
                }
            } else
            {
                return -1;
            }
            return 0;
        }
    }
}
