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
using Fusion.Core.Extensions;
using Fusion.Engine.Common;
using Fusion.Engine.Input;
using Fusion.Engine.Client;
using Fusion.Engine.Server;
using Fusion.Engine.Graphics;
using IronStar.Core;
using Fusion.Engine.Audio;


namespace IronStar.SFX {

	/// <summary>
	/// 
	/// </summary>
	public partial class FXInstance {

		public class LightStage : Stage {

			static Random rand = new Random();

			OmniLight	light = null;
			FXLightStage stageDesc;

			readonly bool			looped;
			readonly float			period;

			float   timer = 0;
			bool	stopped = false;
			float	intensityScale = 1;
			int		counter;

			/// <summary>
			/// 
			/// </summary>
			/// <param name="instance"></param>
			/// <param name="position"></param>
			/// <param name="color"></param>
			/// <param name="radius"></param>
			/// <param name="fadeInRate"></param>
			/// <param name="fadeOutRate"></param>
			public LightStage ( FXInstance instance, FXLightStage stageDesc, FXEvent fxEvent, bool looped ) : base(instance)
			{
				light				=	new OmniLight();
				this.stageDesc		=	stageDesc;

				light.Position		=	FXFactory.GetPosition( stageDesc.OffsetDirection, stageDesc.OffsetFactor, fxEvent );

				light.RadiusInner	=	stageDesc.InnerRadius;
				light.RadiusOuter	=	stageDesc.OuterRadius;
				light.Intensity     =   stageDesc.Intensity;

				this.period			=	stageDesc.Period;
				this.looped         =   looped;

				instance.rw.LightSet.OmniLights.Add(light); 

				UpdatePeriodIntensity();
				UpdateLightStyle();
			}


			public override void Stop ( bool immediate )
			{
				stopped	=	true;
			}

			public override bool IsExhausted ()
			{
				return stopped;
			}

			public override void Kill ()
			{
				fxInstance.rw.LightSet.OmniLights.Remove(light);
			}

			public override void Update ( float dt, FXEvent fxEvent )
			{
				timer += dt;


				while (timer>stageDesc.Period) {
					timer -= period;

					if ( !looped) {
						stopped = true;
						Kill();
					} else {
						counter++;
						UpdatePeriodIntensity();
					}
				}

				UpdateLightStyle();

				light.Position      =   FXFactory.GetPosition( stageDesc.OffsetDirection, stageDesc.OffsetFactor, fxEvent );
			}


			void UpdatePeriodIntensity ()
			{
				if ( stageDesc.LightStyle==FXLightStyle.Random ) {
					intensityScale = rand.NextFloat( 0, 1 );
				}
				if ( stageDesc.LightStyle==FXLightStyle.Strobe ) {
					intensityScale = counter % 1;
				}
			}


			float GetPulseString ( string pulse, float frac )
			{
				var index = (int)Math.Floor(frac * pulse.Length);
				return (pulse[index] - 'a') / 26.0f;
			}


			void UpdateLightStyle()
			{
				float frac = 0;
				if ( period>0 ) {
					frac = (timer / period) % 1;
				}

				float scale = 1;

				switch ( stageDesc.LightStyle ) {
					case FXLightStyle.Const:		scale = 1; break;
					case FXLightStyle.Saw:			scale = 1 - frac; break;
					case FXLightStyle.InverseSaw:	scale = frac; break;
					default: scale = 1; break;
				}

				var pulse = GetPulseString( stageDesc.PulseString, frac );

				light.Intensity = stageDesc.Intensity * scale * intensityScale * pulse;
			}
		}

	}
}
