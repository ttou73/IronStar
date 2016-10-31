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
	class PlayerLanding : SfxInstance {
		
		public PlayerLanding ( SfxSystem sfxSystem, FXEvent fxEvent ) : base(sfxSystem, fxEvent)
		{
			float fallSpeed = Math.Abs(fxEvent.Velocity.Y);
			ShakeCamera(0, -30 * fallSpeed, rand.GaussDistribution(0, 10*fallSpeed));

			AddSoundStage( @"sound\character\fall0", fxEvent.Origin, 4, false );
		}
		
	}
}
