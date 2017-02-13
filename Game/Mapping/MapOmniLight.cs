using System;
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

	public enum LightTemperature {
		Custom					=	0,
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
	}


	public class MapOmniLight : MapNode {

		//[Category("Decal Image")]
		//[Editor( typeof( SpotFileLocationEditor ), typeof( UITypeEditor ) )]
		//public string SpotMaskName { get; set; } = "";
		
		
		[Category("Omni-light")]
		public float Intensity { get; set; } = 500;
		
		[Category("Omni-light")]
		public int Temperature { get; set; } = 2400;
		
		[Category("Omni-light")]
		public float Radius { get; set; } = 5;

		public LightTemperature TemperaturePreset { get; set; } = LightTemperature.IncandescentStandard;


		OmniLight	light;


		[XmlIgnore]
		public Color4 LightColor {
			get {
				var temp	=	(TemperaturePreset == LightTemperature.Custom) ? Temperature : (int)TemperaturePreset;
				var color	=	Fusion.Core.Mathematics.Temperature.Get( temp );
				return 	new Color4( color.X * Intensity, color.Y * Intensity, color.Z * Intensity, 0 );
			}
		}


		/// <summary>
		/// 
		/// </summary>
		public MapOmniLight ()
		{
		}



		public override void SpawnEntity( GameWorld world )
		{
			light		=	new OmniLight();

			light.Intensity		=	LightColor;
			light.Position		=	WorldMatrix.TranslationVector;
			light.RadiusOuter	=	Radius;
			light.RadiusInner	=	0;

			world.Game.RenderSystem.RenderWorld.LightSet.OmniLights.Add( light );
		}



		public override void ActivateEntity()
		{
		}



		public override void Draw( DebugRender dr, Color color, bool selected )
		{
			var transform	=	WorldMatrix;

			var max			= Math.Max( Math.Max( LightColor.Red, LightColor.Green ), Math.Max( LightColor.Blue, 1 ) );

			var dispColor   = new Color( (byte)(LightColor.Red / max * 255), (byte)(LightColor.Green / max * 255), (byte)(LightColor.Blue / max * 255), (byte)255 ); 

			dr.DrawPoint( transform.TranslationVector, 1, color, 1 );

			if (selected) {
				dr.DrawSphere( transform.TranslationVector, Radius, dispColor );
			} else {
			}
		}



		public override void ResetEntity( GameWorld world )
		{
			HardResetEntity( world );
		}



		public override void HardResetEntity( GameWorld world )
		{
			KillEntity( world );
			SpawnEntity( world );
		}



		public override void KillEntity( GameWorld world )
		{
			world.Game.RenderSystem.RenderWorld.LightSet.OmniLights.Remove( light );
		}


		public override MapNode Duplicate()
		{
			var newNode = (MapOmniLight)MemberwiseClone();
			newNode.light = null;
			return newNode;
		}
	}
}
