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
	class MZShotgun : FXInstance {

		Vector3 sparkDir;
		
		public MZShotgun ( FXPlayback sfxSystem, FXEvent fxEvent ) : base(sfxSystem, fxEvent)
		{
			ShakeCamera( rand.GaussDistribution(0,10), rand.GaussDistribution(0,10), rand.GaussDistribution(0,10) );


			sparkDir = Matrix.RotationQuaternion(fxEvent.Rotation).Forward;
			//AddParticleStage("bulletSpark", 0, 0f, 0.1f, 30, EmitSpark );
																					  
			AddLightStage( fxEvent.Origin + sparkDir * 0.1f	, new Color4(125,110, 35,1), 1, 100f, 3f );

			if (sfxSystem.world.IsPlayer(fxEvent.EntityID)) {
				AddSoundStage( @"sound\weapon\spas12_shot2",	false );
			} else {
				AddSoundStage( @"sound\weapon\spas12_shot2", fxEvent.Origin, 1, false );
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
