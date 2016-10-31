using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Fusion;
using Fusion.Core;
using Fusion.Core.Content;
using Fusion.Core.Mathematics;
using Fusion.Engine.Common;
using Fusion.Engine.Input;
using Fusion.Engine.Client;
using Fusion.Engine.Server;
using Fusion.Engine.Graphics;
using ShooterDemo.Core;
using Fusion.Engine.Audio;


namespace ShooterDemo.SFX {

	/// <summary>
	/// 
	/// </summary>
	public partial class SfxInstance {

		protected class LightStage : Stage {

			OmniLight	light = null;
			float	lightIntensity;
			float	lightFadeInRate;
			float	lightFadeOutRate;
			float	lightFadeRate;
			Color4	lightColor;

			/// <summary>
			/// 
			/// </summary>
			/// <param name="instance"></param>
			/// <param name="position"></param>
			/// <param name="color"></param>
			/// <param name="radius"></param>
			/// <param name="fadeInRate"></param>
			/// <param name="fadeOutRate"></param>
			public LightStage ( SfxInstance instance, Vector3 position, Color4 color, float radius, float fadeInRate, float fadeOutRate ) : base(instance)
			{
				if ( fadeInRate  < 0 ) {
					throw new ArgumentOutOfRangeException("fadeInRate < 0");
				}
				if ( fadeOutRate < 0 ) {
					throw new ArgumentOutOfRangeException("fadeOutRate < 0");
				}

				light				=	new OmniLight();
				light.Position		=	position;
				light.RadiusInner	=	radius * 0.1f;
				light.RadiusOuter	=	radius;
				light.Intensity		=	Color4.Zero;

				lightColor			=	color;

				lightFadeInRate		=	fadeInRate;
				lightFadeOutRate	=	fadeOutRate;

				lightIntensity		=	0.0001f;
				lightFadeRate		=	lightFadeInRate;

				SfxInstance.rw.LightSet.OmniLights.Add(light); 
			}


			public override void Stop ( bool immediate )
			{
				if (immediate) {
					lightIntensity	=	0;
				}
			}

			public override bool IsExhausted ()
			{
				return lightIntensity <= 0;
			}

			public override void Kill ()
			{
				SfxInstance.rw.LightSet.OmniLights.Remove(light);
			}

			public override void Update ( float dt, FXEvent fxEvent )
			{
				light.Position	=	fxEvent.Origin;

				lightIntensity += dt * lightFadeRate;
				lightIntensity =  MathUtil.Clamp( lightIntensity, 0, 1 );

				if ( lightIntensity >= 1 ) {
					lightIntensity = 1;
					lightFadeRate = -lightFadeOutRate;
				}

				light.Intensity = lightColor * lightIntensity;
			}
		}

	}
}
