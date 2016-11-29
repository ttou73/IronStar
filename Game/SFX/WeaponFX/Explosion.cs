﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion;
using Fusion.Core;
using Fusion.Core.Content;
using Fusion.Core.Mathematics;
using Fusion.Core.Extensions;
using IronStar;
using IronStar.Core;
using Fusion.Engine.Graphics;

namespace IronStar.SFX.WeaponFX {
	class Explosion : FXInstance {
		
		Vector3 sparkDir;
		
		public Explosion ( FXPlayback sfxSystem, FXEvent fxEvent ) : base(sfxSystem, fxEvent)
		{
			sparkDir = Matrix.RotationQuaternion(fxEvent.Rotation).Forward;

			AddParticleStage("bulletSpark",		0.00f, 0.0f, 0.1f,  250, false, EmitSpark );
			AddParticleStage("explosionSmoke",	0.10f, 0.1f, 1.0f,  25, false, EmitSmoke );
			AddParticleStage("explosionSmoke2",	0.10f, 0.1f, 1.0f,  25, false, EmitSmoke );
			AddParticleStage("explosionFire",	0.00f, 0.1f, 1.0f,  45, false, EmitFire );

			AddLightStage( fxEvent.Origin + sparkDir * 0.1f	, new Color4(200,150,100,1), 3, 100f, 3f );

			AddSoundStage( @"sound\weapon\explosion",	fxEvent.Origin, 1, false );
		}



		void EmitSpark ( ref Particle p, FXEvent fxEvent )
		{
			var vel	=	rand.GaussRadialDistribution(0, 4.0f) + sparkDir * 2;
			var pos	=	fxEvent.Origin + rand.UniformRadialDistribution(1,1) * 0.3f;

			SetupMotion		( ref p, pos, vel, -vel );
			SetupAngles		( ref p, 160 );
			SetupColor		( ref p, 500, 500, 0, 1 );
			SetupTiming		( ref p, rand.GaussDistribution(0.3f, 0.1f), 0.01f, 0.9f );
			SetupSize		( ref p, 0.2f, 0.00f );
		}


		void EmitSmoke ( ref Particle p, FXEvent fxEvent )
		{
			var dir = 	rand.UniformRadialDistribution(0,1);
			var vel	=	dir * 0.5f;
			var pos	=	fxEvent.Origin + dir;

			float time	=	rand.GaussDistribution(1.4f, 0.4f);

			SetupMotion		( ref p, pos, vel, -vel*1.5f );
			SetupAngles		( ref p, 10 );
			SetupColor		( ref p, 0.1f, 0.1f, 0, 0.7f );
			SetupTiming		( ref p, time, 0.1f, 0.2f );
			SetupSize		( ref p, 1.2f, 1.2f );

			p.Effects	=	ParticleFX.LitShadow;
		}


		void EmitFire ( ref Particle p, FXEvent fxEvent )
		{
			var vel	=	rand.GaussRadialDistribution(0, 0.9f);
			var pos	=	fxEvent.Origin + rand.UniformRadialDistribution(1,1) * 0.25f;

			float time	=	rand.GaussDistribution(0.2f, 0.1f);

			SetupMotion		( ref p, pos, vel, Vector3.Zero );
			SetupAngles		( ref p, 10 );
			SetupColor		( ref p, 2000, 2000, 0, 1.0f );
			SetupTiming		( ref p, time, 0.01f, 0.8f );
			SetupSize		( ref p, 0.9f, 1.3f );
		}
	}
}
