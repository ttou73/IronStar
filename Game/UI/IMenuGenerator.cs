using Fusion.Engine.Frames;
using IronStar.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronStar.UI
{
    public interface IMenuGenerator
    {
        Page CreateMenu(string name, IPageOption pageOption);
    }
}
