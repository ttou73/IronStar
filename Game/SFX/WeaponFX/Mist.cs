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

	class Mist : SfxInstance {


		Vector3 sparkDir;
		
		public Mist ( SfxSystem sfxSystem, FXEvent fxEvent ) : base(sfxSystem, fxEvent)
		{
			sparkDir = Matrix.RotationQuaternion(fxEvent.Rotation).Forward;

			AddParticleStage("railDot", 0, 0f, 0.1f,   100, true, EmitSpark );
			AddParticleStage("smoke"  , 0, 0f, 0.1f,   200, true, EmitPuff );
																					  
			AddLightStage( fxEvent.Origin + sparkDir * 0.1f	, GetRailColor(0.03f), 10.0f, 100f, 3f );
		}



		void EmitSpark ( ref Particle p, FXEvent fxEvent )
		{
			var vel		=	rand.GaussRadialDistribution(0, 0.03f);
			var accel	=	rand.GaussRadialDistribution(0, 0.03f);
			var pos		=	fxEvent.Origin + rand.NextVector3( new Vector3(-20,-5,-20), new Vector3(20,0,20) );
			var time	=	rand.GaussDistribution(3,0.5f);

			SetupMotion		( ref p, pos, vel, accel, 0, 0 );
			SetupAngles		( ref p, 5 );
			SetupTiming		( ref p, time, 0.2f, 0.8f );
			SetupSize		( ref p, 0.1f, 0.00f );

			p.Color0		=	new Color4(0.0f, 0.0f, 0.0f, 0.0f);
			p.Color1		=	new Color4(0.7f, 0.7f, 0.7f, 0.7f);
			p.Effects		=	ParticleFX.Lit;
		}



		void EmitPuff ( ref Particle p, FXEvent fxEvent )
		{
			var vel		=	Vector3.Zero;
			var accel	=	Vector3.Zero;
			var pos		=	fxEvent.Origin + rand.NextVector3( new Vector3(-24,-5,-24), new Vector3(24,-2,24) );
			var time	=	rand.GaussDistribution(10,0.5f);

			SetupMotion		( ref p, pos, vel, accel, 0, 0 );
			SetupAngles		( ref p, 30 );
			SetupTiming		( ref p, time, 0.2f, 0.2f );
			SetupSize		( ref p, 2.5f, 2.5f );

			p.Color0		=	new Color4(0.0f, 0.0f, 0.0f, 0.0f);
			p.Color1		=	new Color4(0.2f, 0.2f, 0.2f, 0.2f);
			p.Effects		=	ParticleFX.LitShadow;
		}
	}
}
