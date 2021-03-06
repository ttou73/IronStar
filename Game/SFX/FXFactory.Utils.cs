﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion;
using Fusion.Core;
using Fusion.Core.Utils;
using Fusion.Core.Mathematics;
using Fusion.Core.Extensions;
using System.IO;
using IronStar.Core;
using BEPUphysics;
using BEPUphysics.Character;
using Fusion.Core.IniParser.Model;
using System.ComponentModel;
using System.Xml.Serialization;
using Fusion.Engine.Graphics;
using IronStar.Editors;
using System.Drawing.Design;
using Fusion.Engine.Audio;
using Fusion.Core.Content;

namespace IronStar.SFX {

	public partial class FXFactory {

		public void Precache ( ContentManager content )
		{
			content.Precache<SoundEffect>( SoundStage.Sound );
		}


		public static Vector3 GetPosition( FXDirection dir, float factor, FXEvent fxEvent )
		{
			var m = Matrix.RotationQuaternion(fxEvent.Rotation);

			switch ( dir ) {
				case FXDirection.LocalUp		: return fxEvent.Origin + m.Up * factor;
				case FXDirection.LocalDown		: return fxEvent.Origin + m.Down * factor;
				case FXDirection.LocalLeft		: return fxEvent.Origin + m.Left * factor;
				case FXDirection.LocalRight		: return fxEvent.Origin + m.Right * factor;
				case FXDirection.LocalForward	: return fxEvent.Origin + m.Forward * factor;
				case FXDirection.LocalBackward	: return fxEvent.Origin + m.Backward * factor;
				default: return fxEvent.Origin;
			}
		}


		public static Vector3 GetDirection( FXDirection dir, float factor, FXEvent fxEvent )
		{
			var m = Matrix.RotationQuaternion(fxEvent.Rotation);

			switch ( dir ) {
				case FXDirection.LocalUp		: return m.Up * factor;
				case FXDirection.LocalDown		: return m.Down * factor;
				case FXDirection.LocalLeft		: return m.Left * factor;
				case FXDirection.LocalRight		: return m.Right * factor;
				case FXDirection.LocalForward	: return m.Forward * factor;
				case FXDirection.LocalBackward	: return m.Backward * factor;
				default: return Vector3.Zero;
			}
		}


		public static Vector3 GetRadialDistribution ( Random rand, FXDistribution3D distribution, float min, float max )
		{
			switch (distribution) {
				case FXDistribution3D.UniformRadial: return rand.UniformRadialDistribution( min, max );
				case FXDistribution3D.GaussRadial: return rand.GaussRadialDistribution( (min+max)/2, (max-min)/3.0f );
				default: return Vector3.Zero;
			}
		}

		public static float GetLinearDistribution( Random rand, FXDistribution distribution, float min, float max )
		{
			switch ( distribution ) {
				case FXDistribution.Uniform: return rand.NextFloat( min, max );
				case FXDistribution.Gauss: return rand.GaussDistribution( (min+max)/2, (max-min)/3.0f );
				default: return 0;
			}

		}

		public static float GetRadius ( FXSoundAttenuation attn ) 
		{
			switch ( attn ) {
				case FXSoundAttenuation.Local:		return 5;
				case FXSoundAttenuation.Normal:		return 15;
				case FXSoundAttenuation.Loud:		return 45;
				case FXSoundAttenuation.Distant:	return 135;
				default: return 100;
			}
		}
	}

}
