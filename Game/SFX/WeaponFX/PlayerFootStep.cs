﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion;
using Fusion.Core;
using Fusion.Core.Content;
using Fusion.Core.Mathematics;
using IronStar;
using IronStar.Core;
using Fusion.Engine.Graphics;

namespace IronStar.SFX.WeaponFX {


	class PlayerFootStepR : FXInstance {
		
		public PlayerFootStepR ( FXPlayback sfxSystem, FXEvent fxEvent ) : base(sfxSystem, fxEvent)
		{
			ShakeCamera(0,0,15);
			var name = "foot" + rand.Next(0,6);
			AddSoundStage( @"sound\character\" + name,	fxEvent.Origin, 4, false );
		}
	}



	class PlayerFootStepL : FXInstance {
		
		public PlayerFootStepL ( FXPlayback sfxSystem, FXEvent fxEvent ) : base(sfxSystem, fxEvent)
		{
			ShakeCamera(0,0,-15);
			var name = "foot" + rand.Next(0,6);
			AddSoundStage( @"sound\character\" + name,	fxEvent.Origin, 4, false );
		}
	}
}
