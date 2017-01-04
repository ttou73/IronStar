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

		
		public class SoundStage : Stage {

			AudioEmitter	emitter;

			CurvePoint[]	curve	=	Enumerable.Range(0,5).Select( i => new CurvePoint(i/4.0f, 1.0f-i/4.0f) ).ToArray();

			/// <summary>
			/// 
			/// </summary>
			/// <param name="instance"></param>
			/// <param name="position"></param>
			/// <param name="soundPath"></param>
			public SoundStage ( FXInstance instance, FXSoundStage stageDesc, FXEvent fxEvent, bool looped ) : base(instance)
			{
				var sound	=	instance.fxPlayback.LoadSound( stageDesc.Sound );

				if (sound==null) {
					return;
				}

				emitter	=	instance.sw.AllocEmitter();
				emitter.Position		=	fxEvent.Origin;
				emitter.DistanceScale	=	FXFactory.GetRadius( stageDesc.Attenuation );
				emitter.DopplerScale	=	1;
				emitter.VolumeCurve		=	null;
				emitter.LocalSound		=	false;

				emitter.PlaySound( sound, looped ? PlayOptions.Looped : PlayOptions.None );
			}


			public override void Stop ( bool immediate )
			{
				if (emitter!=null) {
					emitter.StopSound( immediate );
				}
			}


			public override bool IsExhausted ()
			{
				return (emitter==null || emitter.SoundState==SoundState.Stopped);
			}


			public override void Kill ()
			{
				if (emitter!=null) {
					fxInstance.sw.FreeEmitter( emitter );
				}
			}


			public override void Update ( float dt, FXEvent fxEvent )
			{
				if (emitter!=null) {
					emitter.Position	=	fxEvent.Origin;
					emitter.Velocity	=	fxEvent.Velocity;
				}
			}

		}

	}
}
