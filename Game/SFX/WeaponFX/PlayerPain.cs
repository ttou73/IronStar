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

	class GenericPain : SfxInstance {
		public GenericPain ( SfxSystem sfxSystem, FXEvent fxEvent, string sound, float shake, int bloodAmount, int gibAmount=0 ) : base(sfxSystem, fxEvent)
		{
			ShakeCamera( rand.GaussDistribution(0,shake), rand.GaussDistribution(0,shake), rand.GaussDistribution(0,shake) );

			AddParticleStage("bloodSpray04", 0, 0f, 0.1f, bloodAmount, false, EmitBlood );
			AddParticleStage("bloodSpray04", 0, 0f, 0.1f, gibAmount,   false, EmitGib );
			
			if (sfxSystem.world.IsPlayer(fxEvent.ParentID)) {
				AddSoundStage( sound,	false );
			} else {
				AddSoundStage( sound, fxEvent.Origin, 1, false );
			}
		} 


		void EmitBlood ( ref Particle p, FXEvent fxEvent )
		{
			//var m   =	Matrix.RotationQuaternion( fxEvent.Rotation );
			//var vel	=	-m.Forward * rand.GaussDistribution(3,1) + rand.GaussRadialDistribution(0, 1.0f);
			//var pos	=	fxEvent.Origin;

			//SetupMotion		( ref p, pos, vel, Vector3.Zero, 0, 0.7f );
			//SetupAngles		( ref p, 10 );
			//SetupColor		( ref p, 10, 10, 1, 1 );
			//SetupTiming		( ref p, 0.5f, 0.01f, 0.9f );
			//SetupSize		( ref p, 0.3f, 0.00f );

			var m   =	Matrix.RotationQuaternion( fxEvent.Rotation );
			var dir =	rand.UniformRadialDistribution(0.1f,0.1f) + fxEvent.Velocity;
			var vel	=	Vector3.Up * 3 + rand.GaussRadialDistribution(0.5f,0.5f) + fxEvent.Velocity * rand.NextFloat(0,0.1f);
			var pos	=	fxEvent.Origin;

			var time = rand.NextFloat(0.7f, 1.3f);

			SetupMotion		( ref p, pos, vel, Vector3.Zero, 0, 0.8f );
			SetupAngles		( ref p, 50 );
			SetupColor		( ref p, 20, 20, 0, 1 );
			SetupTiming		( ref p, time, 0.01f, 0.1f );
			SetupSize		( ref p, 0.5f, 0.7f );
		}


		void EmitGib ( ref Particle p, FXEvent fxEvent )
		{
			var m   =	Matrix.RotationQuaternion( fxEvent.Rotation );
			var dir =	rand.UniformRadialDistribution(0.1f,0.1f) + fxEvent.Velocity;
			var vel	=	Vector3.Up * 3 + rand.GaussRadialDistribution(0.5f,0.5f) + fxEvent.Velocity * rand.NextFloat(0,0.1f);
			var pos	=	fxEvent.Origin;

			var time = rand.NextFloat(0.7f, 1.3f);

			SetupMotion		( ref p, pos, vel, Vector3.Zero, 0, 0.8f );
			SetupAngles		( ref p, 50 );
			SetupColor		( ref p, 20, 20, 0, 1 );
			SetupTiming		( ref p, time, 0.01f, 0.1f );
			SetupSize		( ref p, 0.5f, 2.0f );
		}
	}


	class PlayerPain25 : GenericPain {
		public PlayerPain25 ( SfxSystem sfxSystem, FXEvent fxEvent ) : base(sfxSystem, fxEvent, @"sound\character\pain25", 50, 50)	{}
	}


	class PlayerPain50 : GenericPain {
		public PlayerPain50 ( SfxSystem sfxSystem, FXEvent fxEvent ) : base(sfxSystem, fxEvent, @"sound\character\pain50", 100, 50)	{}
	}


	class PlayerPain75 : GenericPain {
		public PlayerPain75 ( SfxSystem sfxSystem, FXEvent fxEvent ) : base(sfxSystem, fxEvent, @"sound\character\pain75", 150, 50)	{}
	}

	class PlayerPain100 : GenericPain {
		public PlayerPain100 ( SfxSystem sfxSystem, FXEvent fxEvent ) : base(sfxSystem, fxEvent, @"sound\character\pain100", 150, 50) {}
	}


	class PlayerDeath : GenericPain {
		public PlayerDeath ( SfxSystem sfxSystem, FXEvent fxEvent ) : base(sfxSystem, fxEvent, @"sound\character\death1", 0, 0, 150) {}
	}

	class PlayerDeathMeat : GenericPain {
		public PlayerDeathMeat ( SfxSystem sfxSystem, FXEvent fxEvent ) : base(sfxSystem, fxEvent, @"sound\character\deathMeat", 0, 0, 150)	{}
	}


	
}
