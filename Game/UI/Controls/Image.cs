using Fusion.Core.Mathematics;
using Fusion.Engine.Frames;
using Fusion.Engine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronStar.UI.Controls
{
    public class Image : Frame
    {

        public Image(FrameProcessor fp) : base(fp)
        {
            
        }

        public Image(FrameProcessor fp,Texture image, int posX, int posY, int width, int height) :
            base(fp, posX, posY, width, height, "", new Color(0, 0, 0, 0))
        {
            this.Image = image;
        } 
    }
}
