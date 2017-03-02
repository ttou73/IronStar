using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Engine.Frames;
using Fusion.Engine.Common;
using IronStar.UI.Controls;
using System.Reflection;
using IronStar.UI.Attributes;
using Fusion.Engine.Graphics;
using Fusion.Core.Mathematics;

namespace IronStar.UI.Generators
{
    public partial class MenuGenerator : IMenuGenerator
    {

        private readonly Game game;
        delegate void GeneratorAction(FrameProcessor fp, Page page, ControlAttribute attribute, DescriptionAttribute description);
        Dictionary<Type, GeneratorAction> actions = new Dictionary<Type, GeneratorAction>();

        public MenuGenerator(Game game)
        {
            this.game = game;

            this.GetType().GetMethods().Where(w => w.GetCustomAttribute<GeneratorApplicabilityAttribute>() != null).ToList().ForEach(
                m =>
                {
                    var t = m.GetCustomAttribute<GeneratorApplicabilityAttribute>().Types;
                    foreach (var type in t)
                    {
                        actions[type] = (GeneratorAction)Delegate.CreateDelegate(typeof(GeneratorAction), m);
                    }
                }
            );
        }

        public Page CreateMenu(string name, IPageOption pageOption)
        {
            
            var page = new Page(game.Frames);
            page.Y = 0;
            page.X = 0;
            page.Width = game.RenderSystem.DisplayBounds.Width;
            page.Height = game.RenderSystem.DisplayBounds.Height;

            var members = pageOption.GetType().GetMembers().Where(t => t.GetCustomAttribute<ControlAttribute>(true) != null).ToList();
            members.Sort(Comparer<MemberInfo>.Create(
                (a, b) => 
                {
                    return a.GetCustomAttribute<ControlAttribute>().Order.CompareTo(b.GetCustomAttribute<ControlAttribute>().Order);
                }));

            foreach (var m in members)
            {
                actions[m.GetCustomAttribute<ControlAttribute>(true).GetType()]?.Invoke(game.Frames, page, m.GetCustomAttribute<ControlAttribute>(true), m.GetCustomAttribute<DescriptionAttribute>());
            }

            return page;
        }


        [GeneratorApplicability(typeof(LogoAttribute))]
        public static void GenerateLogo(FrameProcessor fp, Page page, ControlAttribute attribute, DescriptionAttribute description)
        {
            Image image = new Image(fp);
            image.Image = fp.Game.Content.Load<DiscTexture>(StartMenuInfo.LogoTexture);
            image.ImageMode = FrameImageMode.Stretched;

            int width = ParseBounds(StartMenuInfo.LogoWidth, page.Width);
            image.Width = width;
            image.Height = (int)((float)image.Width / (float)image.Image.Width * image.Image.Height);

            Size2 pos = GetLocation(StartMenuInfo.LogoPosition, page.Width, image.Width, page.Height, image.Height);

            image.X = pos.Width;
            image.Y = pos.Height;

            page.Add(image);
        }

        [GeneratorApplicability(typeof(BackgroundAttribute))]
        public static void GenerateBackground(FrameProcessor fp, Page page, ControlAttribute attribute, DescriptionAttribute description)
        {
            Image image = new Image(fp);
            image.Image = fp.Game.Content.Load<DiscTexture>(StartMenuInfo.BackgroundTexture);
            image.Width = page.Width;

            image.X = 0;
            image.Y = 0;

            image.Height = page.Height;
            image.ImageMode = FrameImageMode.Stretched;

            page.Add(image);
        }

        [GeneratorApplicability(typeof(LabelAttribute))]
        public static void GenerateLabel(FrameProcessor fp, Page page, ControlAttribute attribute, DescriptionAttribute description)
        {

        }

    }
}
