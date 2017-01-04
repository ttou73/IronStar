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
using IronStar.Core;
using Fusion.Engine.Audio;


namespace IronStar.SFX {

	/// <summary>
	/// 
	/// </summary>
	public partial class FXInstance {

		public class LightStage : Stage {

			OmniLight	light = null;
			FXLightStage stageDesc;
			bool stopped = false;

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

				light.RadiusInner	=	stageDesc.Radius * 0.1f;
				light.RadiusOuter	=	stageDesc.Radius;
				light.Intensity     =   stageDesc.Intensity;

				instance.rw.LightSet.OmniLights.Add(light); 
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
				light.Position      =   FXFactory.GetPosition( stageDesc.OffsetDirection, stageDesc.OffsetFactor, fxEvent );
			}
		}

	}
}
