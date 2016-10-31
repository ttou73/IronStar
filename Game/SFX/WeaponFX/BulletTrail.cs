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
	class BulletTrail : SfxInstance {

		Vector3 sparkDir;
		
		public BulletTrail ( SfxSystem sfxSystem, FXEvent fxEvent ) : base(sfxSystem, fxEvent)
		{
			sparkDir = Matrix.RotationQuaternion(fxEvent.Rotation).Forward + rand.GaussRadialDistribution(0, 0.1f);
			sparkDir.Normalize();

			AddParticleStage("bulletSpark", 0, 0f, 0.1f, 30, false, EmitSpark );
																					  
			AddLightStage( fxEvent.Origin + sparkDir * 0.1f	, new Color4(125,110, 35,1), 0.5f, 100f, 3f );

			AddSoundStage( @"sound\weapon\bulletHit",	fxEvent.Origin, 1, false );
		}



		void EmitSpark ( ref Particle p, FXEvent fxEvent )
		{
			var vel	=	sparkDir * rand.GaussDistribution(4,3) + rand.GaussRadialDistribution(0, 0.7f);
			var pos	=	fxEvent.Origin;

			SetupMotion		( ref p, pos, vel, Vector3.Zero, 0, 0.3f );
			SetupAngles		( ref p, 160 );
			SetupColor		( ref p, 500, 500, 0, 1 );
			SetupTiming		( ref p, 0.3f, 0.01f, 0.9f );
			SetupSize		( ref p, 0.1f, 0.00f );
		}
	}


	class ShotTrail : SfxInstance {

		Vector3 sparkDir;
		
		public ShotTrail ( SfxSystem sfxSystem, FXEvent fxEvent ) : base(sfxSystem, fxEvent)
		{
			sparkDir = Matrix.RotationQuaternion(fxEvent.Rotation).Forward + rand.GaussRadialDistribution(0, 0.1f);
			sparkDir.Normalize();

			AddParticleStage("bulletSpark", 0, 0f, 0.1f, 10, false, EmitSpark );
																					  
			AddLightStage( fxEvent.Origin + sparkDir * 0.1f	, new Color4(125,110, 35,1), 0.5f, 100f, 3f );

			//AddSoundStage( @"sound\weapon\bulletHit",	fxEvent.Origin, 1, false );
		}



		void EmitSpark ( ref Particle p, FXEvent fxEvent )
		{
			var vel	=	sparkDir * rand.GaussDistribution(4,3) + rand.GaussRadialDistribution(0, 0.7f);
			var pos	=	fxEvent.Origin;

			SetupMotion		( ref p, pos, vel, Vector3.Zero, 0, 0.3f );
			SetupAngles		( ref p, 160 );
			SetupColor		( ref p, 500, 500, 0, 1 );
			SetupTiming		( ref p, 0.3f, 0.01f, 0.9f );
			SetupSize		( ref p, 0.1f, 0.00f );
		}
	}
}
