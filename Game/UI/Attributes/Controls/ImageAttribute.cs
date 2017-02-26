using Fusion.Engine.Frames;
using Fusion.Engine.Graphics;
using IronStar.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronStar.UI.Attributes.Controls
{
    public class ImageAttribute : ControlAttribute
    {
        public string ImageName { get; private set; }
        public ImageAttribute(string imageName)
        {
            this.ImageName = imageName;
        }

        public override Frame GetFrame(FrameProcessor fp)
        {
            return new Image(fp) { Image = fp.Game.Content.Load<DiscTexture>(ImageName) };
        }
    }
}
