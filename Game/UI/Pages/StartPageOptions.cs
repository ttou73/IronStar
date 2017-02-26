using Fusion.Core.Mathematics;
using Fusion.Engine.Common;
using Fusion.Engine.Graphics;
using IronStar.UI.Attributes;
using IronStar.UI.Attributes.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronStar.UI.Pages
{
    public class StartPageOptions : IPageOption
    {
        private readonly Game game;
        public StartPageOptions(Game game)
        {
            this.game = game;
        }

        [Image(@"ui\background")]
        [UIInfo("Background", 0)]
        [Background]
        public void Background() { }

        [UIInfo("Label", 1)]
        [Label("Press any key")]
        [Location("center", "70%")]
        [Size("*", "*")]
        public void Label() { }

        [UIInfo("Logo", 2)]
        [Image(@"ui\Logo")]
        [Location("25%", "40%")]
        [Size("50%", "25%")]
        public void Logo() { }

    }
}
