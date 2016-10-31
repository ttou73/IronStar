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

	class TeleportOut : SfxInstance {


		Vector3 sparkDir;
		
		public TeleportOut ( SfxSystem sfxSystem, FXEvent fxEvent ) : base(sfxSystem, fxEvent)
		{
			sparkDir = Matrix.RotationQuaternion(fxEvent.Rotation).Forward;

			AddParticleStage("teleportSpark", 0, 0f, 0.1f, 150, false, EmitSpark );
																					  
			AddLightStage( fxEvent.Origin + sparkDir * 0.1f	, GetRailColor(0.03f), 1.0f, 100f, 3f );

			if (sfxSystem.world.IsPlayer(fxEvent.ParentID)) {
				AddSoundStage( @"sound\misc\teleport",	false );
			} else {
				AddSoundStage( @"sound\misc\teleport", fxEvent.Origin, 1, false );
			}
		}



		void EmitSpark ( ref Particle p, FXEvent fxEvent )
		{
			var vel		=	rand.GaussRadialDistribution(0, 1f)*2 + Vector3.Up;
			var accel	=	-vel*1 + rand.GaussRadialDistribution(0, 1.2f);
			var pos		=	fxEvent.Origin + Vector3.Up * rand.GaussDistribution(1,0.5f);
			var time	=	rand.GaussDistribution(1.0f,0.2f);

			SetupMotion		( ref p, pos, vel, accel, 0, 0 );
			SetupAngles		( ref p, 160 );
			SetupTiming		( ref p, time, 0.01f, 0.9f );
			SetupSize		( ref p, 0.25f, 0.00f );

			p.Color0		=	GetRailColor();
			p.Color1		=	GetRailColor();
		}
	}
}
