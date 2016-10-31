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
	class PlasmaExplosion : SfxInstance {
		
		Vector3 sparkDir;
		
		public PlasmaExplosion ( SfxSystem sfxSystem, FXEvent fxEvent ) : base(sfxSystem, fxEvent)
		{
			sparkDir = Matrix.RotationQuaternion(fxEvent.Rotation).Forward;

			AddParticleStage("plasmaCore",		0.00f, 0.0f, 0.1f,  50, false, EmitSpark );
			AddParticleStage("plasmaCore",		0.00f, 0.1f, 1.0f,   5, false, EmitBall );
			//AddParticleStage("plasmaPuff",	0.10f, 0.1f, 1.0f,   15, false, EmitSmoke );
			AddParticleStage("plasmaFire",	0.05f, 0.1f, 1.0f,   15, false, EmitFire );

			AddLightStage( fxEvent.Origin - sparkDir * 0.1f	, new Color4(195, 195, 250,1), 2, 100f, 3f );

			AddSoundStage( @"sound\weapon\plasmaHit",	fxEvent.Origin, 1, false );
		}



		void EmitSpark ( ref Particle p, FXEvent fxEvent )
		{
			//var vel		=	(sparkDir * rand.GaussDistribution(1,1) + rand.GaussRadialDistribution(0, 1f))*0.7f;
			//var accel	=	-vel*2 + rand.GaussRadialDistribution(0, 1.2f);
			//var pos		=	fxEvent.Origin;
			//var time	=	rand.GaussDistribution(1,0.2f);

			//SetupMotion		( ref p, pos, vel, accel, 0, 0 );
			//SetupAngles		( ref p, 160 );
			//SetupTiming		( ref p, time, 0.01f, 0.9f );
			//SetupSize		( ref p, 0.1f, 0.00f );
			//SetupColor		( ref p, 500, 500, 0, 1 );

			var vel		=	rand.GaussRadialDistribution(0, 3.0f) + sparkDir * 2;
			var pos		=	fxEvent.Origin;// + rand.UniformRadialDistribution(1,1) * 0.0f;
			var accel	=	-vel*2 + rand.GaussRadialDistribution(0, 1.2f);

			SetupMotion		( ref p, pos, vel, accel );
			SetupAngles		( ref p, 160 );
			SetupColor		( ref p, 1000, 1000, 0, 1 );
			SetupTiming		( ref p, 0.2f, 0.01f, 0.9f );
			SetupSize		( ref p, 0.15f, 0.00f );
		}


		void EmitBall ( ref Particle p, FXEvent fxEvent )
		{
			var dir = 	rand.UniformRadialDistribution(0,1);
			var vel	=	Vector3.Zero;
			var pos	=	fxEvent.Origin;

			float time	=	rand.NextFloat(0.05f, 0.10f);

			SetupMotion		( ref p, pos, Vector3.Zero, Vector3.Zero );
			SetupAngles		( ref p, 100 );
			SetupColor		( ref p, 1000, 1000, 0, 1.0f );
			SetupTiming		( ref p, time, 0.1f, 0.2f );
			SetupSize		( ref p, 0.2f, 0.5f );
		}


		void EmitFire ( ref Particle p, FXEvent fxEvent )
		{
			var vel	=	rand.UniformRadialDistribution(0, 0.5f);
			var pos	=	fxEvent.Origin + rand.UniformRadialDistribution(1,1) * 0.1f;

			float time	=	rand.NextFloat(0.1f, 0.3f);

			SetupMotion		( ref p, pos, vel, Vector3.Zero );
			SetupAngles		( ref p, 1 );
			SetupColor		( ref p, 1000, 1000, 0, 1.0f );
			SetupTiming		( ref p, time, 0.01f, 0.1f );
			SetupSize		( ref p, 0.3f, 0.7f );
		}
	}
}
