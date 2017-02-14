///
///https://academo.org/demos/wavelength-to-colour-relationship/
///

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusion.Core.Mathematics
{
    public static class WaveLength
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="waveLength">A wave length. [nanometers]</param>
        /// <returns></returns>
        public static Color3 GetColor(float waveLength)
        {
            var Gamma = 0.80;
            float IntensityMax = 255;
            float factor, red, green, blue;
            if ((waveLength >= 380) && (waveLength < 440))
            {
                red = -(waveLength - 440) / (440 - 380);
                green = 0.0f;
                blue = 1.0f;
            }
            else if ((waveLength >= 440) && (waveLength < 490))
            {
                red = 0.0f;
                green = (waveLength - 440) / (490 - 440);
                blue = 1.0f;
            }
            else if ((waveLength >= 490) && (waveLength < 510))
            {
                red = 0.0f;
                green = 1.0f;
                blue = -(waveLength - 510) / (510 - 490);
            }
            else if ((waveLength >= 510) && (waveLength < 580))
            {
                red = (waveLength - 510) / (580 - 510);
                green = 1.0f;
                blue = 0.0f;
            }
            else if ((waveLength >= 580) && (waveLength < 645))
            {
                red = 1.0f;
                green = -(waveLength - 645) / (645 - 580);
                blue = 0.0f;
            }
            else if ((waveLength >= 645) && (waveLength < 781))
            {
                red = 1.0f;
                green = 0.0f;
                blue = 0.0f;
            }
            else
            {
                red = 0.0f;
                green = 0.0f;
                blue = 0.0f;
            };
            // Let the intensity fall off near the vision limits
            if ((waveLength >= 380) && (waveLength < 420))
            {
                factor = 0.3f + 0.7f * (waveLength - 380) / (420 - 380);
            }
            else if ((waveLength >= 420) && (waveLength < 701))
            {
                factor = 1.0f;
            }
            else if ((waveLength >= 701) && (waveLength < 781))
            {
                factor = 0.3f + 0.7f * (780 - waveLength) / (780 - 700);
            }
            else
            {
                factor = 0.0f;
            };
            if (red != 0)
            {
                red = (float)Math.Round(IntensityMax * Math.Pow(red * factor, Gamma));
            }
            if (green != 0)
            {
                green = (float)Math.Round(IntensityMax * Math.Pow(green * factor, Gamma));
            }
            if (blue != 0)
            {
                blue = (float)Math.Round(IntensityMax * Math.Pow(blue * factor, Gamma));
            }
            return new Color3(red, green, blue);
        }

    }
}