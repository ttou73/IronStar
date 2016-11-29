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
	class PlasmaTrail : FXInstance {

		public PlasmaTrail ( FXPlayback sfxSystem, FXEvent fxEvent ) : base(sfxSystem, fxEvent)
		{
			//AddParticleStage("bulletSpark",		0.00f, 0.1f, 0.0f,  150, true, EmitSpark );
			AddParticleStage("plasmaCore",	0.00f, 0.2f, 0.0f,   30, true, EmitFire );
			//AddParticleStage("smoke",	0.00f, 0.2f, 0.0f,   60, true, EmitSmoke );

			AddLightStage( fxEvent.Origin * 0.1f	, new Color4(195, 195, 250,1), 2, 100f, 3f );

			AddSoundStage( @"sound\weapon\rfly",	fxEvent.Origin, 1, true );
		}



		//void EmitSpark ( ref Particle p, FXEvent fxEvent )
		//{
		//	var vel	=	rand.GaussRadialDistribution(0, 6.0f);
		//	var pos	=	fxEvent.Origin;

		//	SetupMotion		( ref p, pos, Vector3.Zero, Vector3.Zero );
		//	SetupAngles		( ref p, 160 );
		//	SetupColor		( ref p, 500, 0, 1 );
		//	SetupTiming		( ref p, 0.2f, 0.01f, 0.9f );
		//	SetupSize		( ref p, 0.1f, 0.00f );
		//}


		void EmitFire ( ref Particle p, FXEvent fxEvent )
		{
			var vel	=	rand.GaussRadialDistribution(0, 0.3f) + Matrix.RotationQuaternion(fxEvent.Rotation).Backward * 5;
			var pos	=	fxEvent.Origin;

			SetupMotion		( ref p, pos, vel, Vector3.Zero );
			SetupAngles		( ref p, 0 );
			SetupColor		( ref p, 1500, 1500, 0, 1.0f );
			SetupTiming		( ref p, 0.05f, 0.01f, 0.1f );
			SetupSize		( ref p, 0.20f, 0.3f );
		}

		
		
		void EmitSmoke ( ref Particle p, FXEvent fxEvent )
		{
			var dir = 	rand.UniformRadialDistribution(0,0.2f);
			var vel	=	dir * 0.5f;
			var pos	=	fxEvent.Origin;

			SetupMotion		( ref p, pos, vel, Vector3.Zero, 0, -0.05f );
			SetupAngles		( ref p, 10 );
			SetupColor		( ref p, 5, 5, 0, 1.0f );
			SetupTiming		( ref p, 1.5f, 0.1f, 0.1f );
			SetupSize		( ref p, 0.15f, 0.5f );
		}



	}
}
