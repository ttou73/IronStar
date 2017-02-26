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
using IronStar.UI.Attributes.Controls;
using Fusion.Engine.Graphics;
using Fusion.Core.Mathematics;

namespace IronStar.UI
{
    public class PrimitiveMenuGenerator : IMenuGenerator
    {

        private readonly Game game;
        public PrimitiveMenuGenerator(Game game)
        {
            this.game = game;
        }

        public Page CreateMenu(string name, IPageOption pageOption)
        {
            
            var page = new Page(game.Frames);
            page.Y = 0;
            page.X = 0;
            page.Width = game.RenderSystem.DisplayBounds.Width;
            page.Height = game.RenderSystem.DisplayBounds.Height;

            var members = pageOption.GetType().GetMembers().Where(t => t.GetCustomAttribute<UIInfoAttribute>() != null).ToList();
            members.Sort(Comparer<MemberInfo>.Create(
                (a, b) => 
                {
                    return a.GetCustomAttribute<UIInfoAttribute>().Order.CompareTo(b.GetCustomAttribute<UIInfoAttribute>().Order);
                }));
            foreach (var member in members)
            {
                var controlAttribute = member.GetCustomAttribute<ControlAttribute>();
                var frame = controlAttribute.GetFrame(game.Frames);

                ///set frame settings, check type
                
                frame.BackColor = Color.Transparent;

                //TODO: c# 7.0 pattern matching
                if (frame is Label)
                {
                    frame.Font = game.Content.Load<SpriteFont>(@"fonts\armata");
                }

                if (frame is Image)
                {
                    frame.ImageMode = FrameImageMode.Stretched;
                }

                if (member.GetCustomAttribute<BackgroundAttribute>() != null)
                {
                    frame.Width = page.Width;
                    frame.Height = page.Height;
                    frame.X = 0;
                    frame.Y = 0;
                    page.Add(frame);
                    continue;
                }

                ///set bounds
                if (member.GetCustomAttribute<SizeAttribute>() != null)
                {
                    var a = member.GetCustomAttribute<SizeAttribute>();
                    frame.Width = a.GetWidth(page.Width);
                    frame.Height = a.GetHeight(page.Height);
                    if (frame.Width == -1)
                    {
                        frame.Width = autosize(frame).Item1;
                    }

                    if (frame.Height == -1)
                    {
                        frame.Height = autosize(frame).Item2;
                    }
                }
                else
                {
                    ///generate 
                }

                //set position
                if (member.GetCustomAttribute<LocationAttribute>() != null)
                {
                    var a = member.GetCustomAttribute<LocationAttribute>();
                    frame.X = a.GetX(page.Width, frame.Width);
                    frame.Y = a.GetY(page.Height, frame.Height);

                } else
                {
                    ///generate position
                }

               
                page.Add(frame);
            }

            return page;
        }


        private Tuple<int,int> autosize(Frame frame)
        {
            int w = 0, h = 0;

            if (frame is Image)
            {
                var image = frame as Image;
                w = image.Image.Width;
                h = image.Image.Height;
            } else 
            if (frame is Label)
            {
                var label = frame as Label;
                var font = frame.Font ?? game.Frames.DefaultFont;
                var r = font.MeasureString(frame.Text);
                w = r.Width;
                h = r.Height;
            }
            return new Tuple<int, int>(w, h);
        }
    }
}
