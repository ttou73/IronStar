using Fusion.Engine.Frames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronStar.UI.Attributes.Controls
{
    public abstract class ControlAttribute : Attribute
    {
        public abstract Frame GetFrame(FrameProcessor fp);
    }
}
