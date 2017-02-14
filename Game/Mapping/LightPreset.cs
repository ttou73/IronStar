﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Fusion.Core.Mathematics;
using IronStar.Core;
using Fusion.Engine.Graphics;
using IronStar.SFX;
using Fusion.Development;
using System.Drawing.Design;

namespace IronStar.Mapping {
	public enum LightPreset {
		Red						=	1,
		Green					=	2,
		Blue					=	3,
		Yellow					=	4,
		Cyan					=	5,
		White					=	7,
		Magenta					=	6,

		//	TODO: wave lengths...

		MatchFlame				=	1700,
		CandleFlame				=	1850,
		IncandescentStandard	=	2400,
		IncandescentSoftWhite	=	2550,
		FlourescentSoftWhite	=	2700,
		FlourescentWarmWhite	=	3000,
		StudioLight				=	3200,
		Moonlight				=	4100,
		FluorescentTubular		=	5000,
		DaylightHorizon			=	5000,
		DaylightZenith			=	5750,
		XenonShortArcLamp		=	6200,
		DaylightOvercast		=	6500,
		CRTScreenWarm			=	5000,
		CRTScreenSRGB			=	6500,
		CRTScreenCool			=	9300,
		PolarClearBlueSky		=	15000,

		Cold20K					=	20000,
		Cold30K					=	30000,
		Cold40K					=	40000,
	}



	static class LightPresetColor {
	
		public static Color4 GetColor ( LightPreset preset, float intensity )
		{	
			var temp	=	(int)preset;

			switch (preset) {
				case LightPreset.Red	:	return new Color4(1,0,0,0) * intensity;
				case LightPreset.Green	:	return new Color4(0,1,0,0) * intensity;
				case LightPreset.Blue	:	return new Color4(0,0,1,0) * intensity;
				case LightPreset.Yellow	:	return new Color4(1,1,0,0) * intensity;
				case LightPreset.Magenta:	return new Color4(1,0,1,0) * intensity;
				case LightPreset.Cyan	:	return new Color4(0,1,1,0) * intensity;
				case LightPreset.White	:	return new Color4(1,1,1,0) * intensity;
			}
			
			var color	=	Temperature.Get( temp );
			return 	new Color4( color.X * intensity, color.Y * intensity, color.Z * intensity, 0 );
		}

	}

}
