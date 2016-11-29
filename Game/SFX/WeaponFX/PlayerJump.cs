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
	class PlayerJump : FXInstance {
		
		public PlayerJump ( FXPlayback sfxSystem, FXEvent fxEvent ) : base(sfxSystem, fxEvent)
		{
			AddSoundStage( @"sound\character\jump",	fxEvent.Origin, 4, false );
		}
	}
}
