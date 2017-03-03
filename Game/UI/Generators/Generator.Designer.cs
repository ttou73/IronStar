using Fusion.Core.Mathematics;
using Fusion.Engine.Frames;
using IronStar.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronStar.UI.Generators
{
    public partial class MenuGenerator
    {
        public struct StartMenuInfo
        {
            public static string LogoPosition = "center;40%";
            public static string LogoWidth = "50%";
            public static string LogoTexture = @"ui\logo";
            public static string BackgroundTexture = @"ui\background";

            public static string LabelPosition = "center%;70%";
        }

        public struct MainMenuInfo
        {
            public static string LeftOffset = "20%";
            public static string TopOffset = "20%";
            public static string ButtonWidth = "30%";
            public static string ButtonOffset = "5%";
            internal static float currentPosition = 0;
        }


        private static Size2 Autosize(Frame frame)
        {
            int w = 0, h = 0;

            if (frame is Image)
            {
                var image = frame as Image;
                w = image.Image.Width;
                h = image.Image.Height;
            }
            else
            if (frame is Label)
            {
                var label = frame as Label;
                var font = frame.Font;
                var r = font.MeasureString(frame.Text);
                w = r.Width;
                h = r.Height;
            }
            return new Size2(w, h);
        }


        private static Size2 GetLocation(string value, int width, int elementWidth, int height, int elementHeight)
        {
            var arr = value.Split(';');
            return new Size2(ParseLocation(arr[0], width, elementWidth), ParseLocation(arr[1], height, elementHeight));
        }

        private static Size2 GetBounds(string value, int width, int height)
        {
            var arr = value.Split(';');
            return new Size2(ParseBounds(arr[0], width), ParseBounds(arr[1], height));
        }

        private static int ParseLocation(string value, int side, int elementSide)
        {
            if (char.IsDigit(value[0]))
            {
                if (char.IsDigit(value[value.Length - 1]))
                {
                    return int.Parse(value);
                }
                else
                {
                    float modificator = float.Parse(value.Substring(0, value.Length - 1)) / 100f;
                    return (int)(side * modificator);
                }
            }
            else
            {
                if (value.Equals("center"))
                {
                    return side / 2 - elementSide / 2;
                }
                else if (value.Equals("left"))
                {
                    return side - elementSide;
                }
                else //right
                {
                    return 0;
                }
            }
        }

        private static int ParseBounds(string value, int side)
        {
            if (char.IsDigit(value[0]))
            {
                if (char.IsDigit(value[value.Length - 1]))
                {
                    return int.Parse(value);
                }
                else
                {
                    float modificator = float.Parse(value.Substring(0, value.Length - 1)) / 100f;
                    return (int)(side * modificator);
                }
            }
            else
            {
                return -1;
            }
        }
    }
}
