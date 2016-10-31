using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion;
using Fusion.Core;
using Fusion.Core.Content;
using Fusion.Core.Mathematics;
using Fusion.Core.Extensions;
using ShooterDemo;
using ShooterDemo.Core;
using Fusion.Engine.Graphics;

namespace ShooterDemo.SFX.WeaponFX {
	class MZBlaster : SfxInstance {

		Vector3 sparkDir;
		
		public MZBlaster ( SfxSystem sfxSystem, FXEvent fxEvent ) : base(sfxSystem, fxEvent)
		{
			ShakeCamera( rand.GaussDistribution(0,5), rand.GaussDistribution(5,5), rand.GaussDistribution(0,5) );


			sparkDir = Matrix.RotationQuaternion(fxEvent.Rotation).Forward;
			//AddParticleStage("bulletSpark", 0, 0f, 0.1f, 30, EmitSpark );
																					  
			AddLightStage( fxEvent.Origin + sparkDir * 0.1f	, new Color4(137,137,228,1), 1, 100f, 3f );

			if (sfxSystem.world.IsPlayer(fxEvent.ParentID)) {
				AddSoundStage( @"sound\weapon\plasma",	false );
			} else {
				AddSoundStage( @"sound\weapon\plasma", fxEvent.Origin, 1, false );
			}
		}



		void EmitSpark ( ref Particle p, FXEvent fxEvent )
		{
			var vel	=	sparkDir * rand.GaussDistribution(4,3) + rand.GaussRadialDistribution(0, 0.7f);
			var pos	=	fxEvent.Origin;

			SetupMotion		( ref p, pos, vel, Vector3.Zero, 0, 1 );
			SetupAngles		( ref p, 160 );
			SetupColor		( ref p, 500, 500, 0, 1 );
			SetupTiming		( ref p, 0.5f, 0.01f, 0.9f );
			SetupSize		( ref p, 0.1f, 0.00f );
		}
	}
}
