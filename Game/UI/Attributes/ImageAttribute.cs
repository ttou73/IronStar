using Fusion.Engine.Frames;
using Fusion.Engine.Graphics;
using IronStar.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronStar.UI.Attributes
{
    public class ImageAttribute : ControlAttribute
    {
        public string ImageName { get; private set; }

        public ImageAttribute(int order, string imageName)
        {
            this.Order = order;
            this.ImageName = imageName;
        }
    }
}
