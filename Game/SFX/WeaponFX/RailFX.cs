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

	class RailHit : SfxInstance {


		Vector3 sparkDir;
		
		public RailHit ( SfxSystem sfxSystem, FXEvent fxEvent ) : base(sfxSystem, fxEvent)
		{
			sparkDir = Matrix.RotationQuaternion(fxEvent.Rotation).Forward;

			AddParticleStage("railDot", 0, 0f, 0.1f, 100, false, EmitSpark );
			//AddParticleStage("railDot", 0, 0f, 0.1f,  30, false, EmitPuff );
																					  
			AddLightStage( fxEvent.Origin + sparkDir * 0.1f	, GetRailColor(0.03f), 1.0f, 100f, 3f );

			AddSoundStage( @"sound\weapon\railshot",	fxEvent.Origin, 1, false );
		}



		void EmitSpark ( ref Particle p, FXEvent fxEvent )
		{
			var vel		=	(sparkDir * rand.GaussDistribution(1,1) + rand.GaussRadialDistribution(0, 1f))*0.7f;
			var accel	=	-vel*2 + rand.GaussRadialDistribution(0, 1.2f);
			var pos		=	fxEvent.Origin;
			var time	=	rand.GaussDistribution(1,0.2f);

			SetupMotion		( ref p, pos, vel, accel, 0, 0 );
			SetupAngles		( ref p, 160 );
			SetupTiming		( ref p, time, 0.01f, 0.9f );
			SetupSize		( ref p, 0.1f, 0.00f );

			p.Color0		=	GetRailColor();
			p.Color1		=	GetRailColor();
		}



		void EmitPuff ( ref Particle p, FXEvent fxEvent )
		{
			var vel	=	rand.GaussRadialDistribution(0, 1.5f);
			var pos	=	fxEvent.Origin;

			SetupMotion		( ref p, pos, vel, vel, 0, 0.0f );
			SetupAngles		( ref p, 10 );
			SetupTiming		( ref p, 1f, 0.01f, 0.1f );
			SetupSize		( ref p, 0.2f, 0.0f );

			p.Color0		=	GetRailColor();
			p.Color1		=	GetRailColor();
		}
	}



	class RailMuzzle : SfxInstance {

		Vector3 sparkDir;
		
		public RailMuzzle ( SfxSystem sfxSystem, FXEvent fxEvent ) : base(sfxSystem, fxEvent)
		{
			ShakeCamera( rand.GaussDistribution(0,20), rand.GaussDistribution(0,20), rand.GaussDistribution(0,20) );


			sparkDir = Matrix.RotationQuaternion(fxEvent.Rotation).Forward;
			//AddParticleStage("bulletSpark", 0, 0f, 0.1f, 30, EmitSpark );
																					  
			AddLightStage( fxEvent.Origin, GetRailColor(0.03f), 2.0f, 100f, 3f );

			if (sfxSystem.world.IsPlayer(fxEvent.ParentID)) {
				AddSoundStage( @"sound\weapon\railshot2",	false );
			} else {
				AddSoundStage( @"sound\weapon\railshot2", fxEvent.Origin, 1, false );
			}
		}
	}



	class RailTrail : SfxInstance {

		float sin ( float a ) { return (float)Math.Sin(a*6.28f); }
		float cos ( float a ) { return (float)Math.Cos(a*6.28f); }

		
		public RailTrail ( SfxSystem sfxSystem, FXEvent fxEvent ) : base(sfxSystem, fxEvent)
		{
			var p = new Particle();

			p.TimeLag		=	0;

			var m = Matrix.RotationQuaternion( fxEvent.Rotation );
			var up = m.Up;
			var rt = m.Right;

			int count = Math.Min((int)(fxEvent.Velocity.Length() * 20), 2000);

			//
			//	Overall color
			//

			//
			//	Beam :
			//
			p.ImageIndex	=	sfxSystem.GetSpriteIndex("railTrace");

			if (true) {
				var pos		=	fxEvent.Origin;
				var pos1	=	fxEvent.Origin + fxEvent.Velocity;
				
				SetupMotion	( ref p, pos, Vector3.Zero, Vector3.Zero, 0, 0 );
				SetupTiming	( ref p, 0.2f, 0.01f, 0.9f );
				SetupSize	( ref p, 0.1f, 0.0f );
				SetupAngles	( ref p, 160 );

				p.Color0		=	GetRailColor();
				p.Color1		=	GetRailColor();

				p.TailPosition	=	pos1;
				p.Effects	|=	ParticleFX.Beam;
 
				rw.ParticleSystem.InjectParticle( p );
			}


			//
			//	Spiral :
			//
			p.ImageIndex	=	sfxSystem.GetSpriteIndex("railDot");
			p.Effects		=	ParticleFX.None;

			for (int i=0; i<count; i++) {

				var t		=	i * 0.1f;
				var c		=	(float)Math.Cos(t);
				var s		=	(float)Math.Sin(t);

				#if true
				var pos		=	fxEvent.Origin + rt * c * 0.05f + up * s * 0.05f + fxEvent.Velocity * (i+0)/(float)count;
				var vel		=	rt * c * 0.15f + up * s * 0.15f + rand.GaussRadialDistribution(0,0.03f);
				#else
				var pos		=	fxEvent.Origin + rt * c * 0.01f + up * s * 0.01f + fxEvent.Velocity * (i+0)/(float)count;
				var vel		=	rt * c * 0.15f + up * s * 0.15f + rand.GaussRadialDistribution(0,0.02f);
				#endif

				var time	=	rand.GaussDistribution(1, 0.2f);
			
				SetupMotion	( ref p, pos, vel, Vector3.Zero, 0, 0 );
				SetupColor	( ref p, 1000, 1000, 1, 1 );
				SetupTiming	( ref p, time, 0.5f, 0.5f );
				SetupSize	( ref p, 0.03f, 0.0f );
				SetupAngles	( ref p, 160 );

				p.TimeLag		=	-0.1f;
				p.Color0		=	GetRailColor();
				p.Color1		=	GetRailColor();

				p.TailPosition	=	Vector3.Zero;
 
				//SfxInstance.rw.Debug.Trace( p.Position, 0.2f, Color.Yellow );
				rw.ParticleSystem.InjectParticle( p );
			}

			//	
			//	Trace
			//
			p.ImageIndex	=	sfxSystem.GetSpriteIndex("railDot");
			p.Effects		=	ParticleFX.None;

			for (int i=0; i<count/3; i++) {

				var pos		=	fxEvent.Origin + fxEvent.Velocity * (i*3)/(float)count;
				var vel		=	rand.GaussRadialDistribution(0,0.1f);
				var time	=	rand.GaussDistribution(1, 0.2f);
			
				SetupMotion	( ref p, pos, vel, Vector3.Zero, 0, 0 );
				SetupColor	( ref p, 0, 200, 0, 1 );
				SetupTiming	( ref p, time, 0.5f, 0.5f );
				SetupSize	( ref p, 0.05f, 0.0f );
				SetupAngles	( ref p, 160 );

				p.TimeLag		=	-0.1f;
				p.Color0		=	GetRailColor();
				p.Color1		=	GetRailColor();

				p.TailPosition	=	Vector3.Zero;
 
				//SfxInstance.rw.Debug.Trace( p.Position, 0.2f, Color.Yellow );
				rw.ParticleSystem.InjectParticle( p );
			}	   



		}
	}
}
