using System;
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

namespace IronStar.SFX {

	public partial class FXFactory {

		public static Vector3 GetPosition ( FXDirection dir, float factor, FXEvent fxEvent )
		{
			var m = Matrix.RotationQuaternion(fxEvent.Rotation);

			switch ( dir ) {
				case FXDirection.LocalUp:		return fxEvent.Origin + m.Up * factor;
				case FXDirection.LocalDown:		return fxEvent.Origin + m.Down * factor;
				case FXDirection.LocalLeft:		return fxEvent.Origin + m.Left * factor;
				case FXDirection.LocalRight:	return fxEvent.Origin + m.Right * factor;
				case FXDirection.LocalForward:	return fxEvent.Origin + m.Forward * factor;
				case FXDirection.LocalBackward: return fxEvent.Origin + m.Backward * factor;
				default:  return fxEvent.Origin;
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
