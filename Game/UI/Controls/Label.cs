using Fusion.Core.Mathematics;
using Fusion.Engine.Frames;
using Fusion.Engine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Engine.Common;

namespace IronStar.UI.Controls
{
    public class Label : Frame
    {

        public Label(FrameProcessor fp) : base(fp)
        {

        }

        public Label(FrameProcessor fp, int posX, int posY, int width, int height, string text, Color backColor) :
            base(fp, posX, posY, width, height, text, backColor)
        {
        }

        public Label(FrameProcessor fp, int posX, int posY, int width, int height, string text, SpriteFont font, Color backColor ) :
            base(fp, posX, posY, width, height, text, font, backColor )
        {

        }

        protected override void Update(GameTime gameTime)
        {
            //TODO : remove
            if (ForeColor.A == 255)
            {
                RunTransition("ForeColor", new Color(0, 0, 0, 160), 0, 900);
            } else if (ForeColor.A == 160)
            {
                RunTransition("ForeColor", new Color(255, 255, 255, 255), 0, 900);
            }
            base.Update(gameTime);
        }
    }
}
