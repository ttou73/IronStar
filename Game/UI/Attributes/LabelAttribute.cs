using Fusion.Core.Mathematics;
using Fusion.Engine.Graphics;
using IronStar.UI.Attributes.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Engine.Frames;
using IronStar.UI.Controls;

namespace IronStar.UI.Attributes
{
    public class LabelAttribute : ControlAttribute
    {
        public string Text { get; private set; }
        public LabelAttribute(string text)
        {
            this.Text = text;
        }

        public override Frame GetFrame(FrameProcessor fp)
        {
            return new Label(fp, 0, 0, 0, 0, Text, Color.Transparent);
        }
    }
}
