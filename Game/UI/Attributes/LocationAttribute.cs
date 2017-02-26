using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronStar.UI.Attributes
{
    public class LocationAttribute : Attribute
    {
        public string X { get; private set; }
        public string Y { get; private set; }

        public LocationAttribute(string x, string y)
        {
            this.X = x;
            this.Y = y;
        }


        public int GetX(int width, int elementWidth)
        {
            return Parse(X, width, elementWidth);
        }

        public int GetY(int height, int elementHeight)
        {
            return Parse(Y, height, elementHeight);
        }


        private int Parse(string value, int side, int elementSide)
        {
            if (char.IsDigit(value[0]))
            {
                if (char.IsDigit(value[value.Length - 1]))
                {
                    return int.Parse(value);
                } else
                {
                    float modificator = float.Parse(value.Substring(0, value.Length - 1)) / 100f;
                    return (int)(side * modificator);
                }
            }
            else
            {
                if (value.Equals("center")) {
                    return side / 2 - elementSide / 2;
                }
                else if (value.Equals("left")) {
                    return side - elementSide;
                } else //right
                {
                    return 0;
                }
            }
        }
    }
}
