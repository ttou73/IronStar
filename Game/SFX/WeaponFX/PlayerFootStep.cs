using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion;
using Fusion.Core;
using Fusion.Core.Content;
using Fusion.Core.Mathematics;
using ShooterDemo;
using ShooterDemo.Core;
using Fusion.Engine.Graphics;

namespace ShooterDemo.SFX.WeaponFX {


	class PlayerFootStepR : SfxInstance {
		
		public PlayerFootStepR ( SfxSystem sfxSystem, FXEvent fxEvent ) : base(sfxSystem, fxEvent)
		{
			ShakeCamera(0,0,15);
			var name = "foot" + rand.Next(0,6);
			AddSoundStage( @"sound\character\" + name,	fxEvent.Origin, 4, false );
		}
	}



	class PlayerFootStepL : SfxInstance {
		
		public PlayerFootStepL ( SfxSystem sfxSystem, FXEvent fxEvent ) : base(sfxSystem, fxEvent)
		{
			ShakeCamera(0,0,-15);
			var name = "foot" + rand.Next(0,6);
			AddSoundStage( @"sound\character\" + name,	fxEvent.Origin, 4, false );
		}
	}
}
