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
using Fusion.Core.Content;
using Fusion.Development;
using Fusion.Engine.Storage;

namespace IronStar.SFX {

	public partial class FXFactory : IPrecachable {

		[Category("General")]
		public float Period { get; set; } = 1;

		[Category( "Misc Stages" )]
		[TypeConverter( typeof( ExpandableObjectConverter ) )]
		public FXSoundStage SoundStage { get; set; } = new FXSoundStage();

		[Category( "Misc Stages" )]
		[TypeConverter( typeof( ExpandableObjectConverter ) )]
		public FXLightStage LightStage { get; set; } = new FXLightStage();

		[Category( "Misc Stages" )]
		[TypeConverter( typeof( ExpandableObjectConverter ) )]
		public FXCameraShake CameraShake { get; set; } = new FXCameraShake();

		[Category( "Particle Stages" )]
		[TypeConverter( typeof( ExpandableObjectConverter ) )]
		public FXParticleStage ParticleStage1 { get; set; } = new FXParticleStage();

		[Category( "Particle Stages" )]
		[TypeConverter( typeof( ExpandableObjectConverter ) )]
		public FXParticleStage ParticleStage2 { get; set; } = new FXParticleStage();

		[Category( "Particle Stages" )]
		[TypeConverter( typeof( ExpandableObjectConverter ) )]
		public FXParticleStage ParticleStage3 { get; set; } = new FXParticleStage();

		[Category( "Particle Stages" )]
		[TypeConverter( typeof( ExpandableObjectConverter ) )]
		public FXParticleStage ParticleStage4 { get; set; } = new FXParticleStage();


		public FXInstance CreateFXInstance( FXPlayback fxPlayback, FXEvent fxEvent, bool looped )
		{
			return new FXInstance( fxPlayback, fxEvent, this, looped );
		}
	}



	[ContentLoader( typeof( FXFactory ) )]
	public sealed class FXFactoryLoader : ContentLoader {

		static Type[] extraTypes;

		public override object Load( ContentManager content, Stream stream, Type requestedType, string assetPath, IStorage storage )
		{
			return Misc.LoadObjectFromXml( typeof( FXFactory ), stream, extraTypes );
		}
	}


	public enum FXDistribution {
		Uniform,
		Gauss,
	}


	public enum FXDistribution3D {
		UniformRadial,
		GaussRadial,
	}


	public enum FXVelocityBias {
		None,
		LocalUp,
		//Centripital,
	}

	public enum FXDirection {
		None,
		LocalUp,
		LocalDown,
		LocalRight,
		LocalLeft,
		LocalForward,
		LocalBackward,
	}


	public class FXTiming {
		[XmlAttribute]
		[Description( "Relative delay of particle emission [0..1]" )]
		public float Delay { get; set; } = 0;
		
		[XmlAttribute]
		[Description( "Relative period of particle emission [0..1]" )]
		public float Bunch { get; set; } = 1;
		
		[XmlAttribute]
		[Description( "Relative fade-in period [0..1]" )]
		public float FadeIn { get; set; } = 0.1f;
		
		[XmlAttribute]
		[Description( "Relative fade-out period [0..1]" )]
		public float FadeOut { get; set; } = 0.1f;

		public override string ToString()
		{
			return string.Format("d:{0:0.##} b:{1:0.##} fade[{2:0.##}, {3:0.##}]", Delay, Bunch, FadeIn, FadeOut);
		}
	}


	public enum FXSoundAttenuation {
		Local,
		Normal,
		Loud,
		Distant,
	}


	public enum FXLightStyle {
		Const,
		Saw,
		InverseSaw,
		Random,
		Strobe,
	}


	public class FXLifetime {
		[XmlAttribute]
		[Description( "Lifetime distribution" )]
		public FXDistribution Distribution { get; set; } = FXDistribution.Uniform;
		[XmlAttribute]
		[Description( "Average (mean) value of particle lifetime" )]
		public float Average { get; set; } = 1;
		[XmlAttribute]
		[Description( "Probability interval (three-sigma rule for Gauss distribution)" )]
		public float Deviation { get; set; } = 0;

		public override string ToString()
		{
			return string.Format( "{0}: [{1:0.##}, {2:0.##}]", Distribution, Average, Deviation );
		}

		public float GetLifetime ( Random rand )
		{
			return FXFactory.GetLinearDistribution( rand, Distribution, Average, Deviation );
		}
	}


	public class FXAcceleration {
		[XmlAttribute]
		[Description( "Gravity factor [-1..1]. Zero value means no gravity. Negative values means buoyant particles" )]
		public float GravityFactor { get; set; } = 0;

		[XmlAttribute]
		[Description( "Velocity damping factor" )]
		public float Damping { get; set; } = 0;

		[XmlAttribute]
		[Description( "Constant acceleration opposite to initial velocity vector" )]
		public float DragForce { get; set; } = 0;

		[XmlAttribute]
		[Description( "Constant normally distributed acceleration" )]
		public float Turbulence { get; set; } = 0;

		public override string ToString()
		{
			return string.Format( "G:{0:0.##} D:{1:0.##} Drag:{2:0.##}, Turb:{3:0.##}", GravityFactor, Damping, DragForce, Turbulence );
		}
	}


	public class FXVelocity {

		[XmlAttribute]
		public FXDirection Direction { get; set; } = FXDirection.None;
		
		[XmlAttribute]
		public FXDistribution LinearDistribution { get; set; } = FXDistribution.Uniform;

		[XmlAttribute]
		[Description( "Average radial velocity" )]
		public float LinearAverage { get; set; } = 1;

		[XmlAttribute]
		[Description( "Radial velocity deviation" )]
		public float LinearDeviation { get; set; } = 0;

		[XmlAttribute]
		[Description( "Radial velocity distribution" )]
		public FXDistribution3D RadialDistribution { get; set; } = FXDistribution3D.UniformRadial;
		
		[XmlAttribute]
		[Description( "Average radial velocity" )]
		public float RadialAverage { get; set; } = 1;
		
		[XmlAttribute]
		[Description( "Radial velocity deviation" )]
		public float RadialDeviation { get; set; } = 0;

		[XmlAttribute]
		[Description( "Source velocity addition factor" )]
		public float Advection { get; set; } = 0;

		public override string ToString()
		{
			return string.Format( "L:{0}:{1:0.##} R:{2}:{3:0.##}", LinearDistribution, LinearAverage, RadialDistribution, RadialAverage );
		}

		public Vector3 GetVelocity( FXEvent fxEvent, Random rand )
		{
			var velocityValue   =   FXFactory.GetLinearDistribution( rand, LinearDistribution, LinearAverage, LinearDeviation );
			var velocity		=   FXFactory.GetDirection( Direction, velocityValue, fxEvent );
			var addition		=	FXFactory.GetRadialDistribution( rand, RadialDistribution, RadialAverage, RadialDeviation );
			var advection		=	fxEvent.Velocity * Advection;

			return velocity + addition + advection;
		}
	}


	public class FXPosition {
		[XmlAttribute]
		[Description( "Offset direction" )]
		public FXDirection OffsetDirection { get; set; } = FXDirection.None;

		[XmlAttribute]
		[Description( "Offset along offset direction" )]
		public float OffsetFactor { get; set; } = 0;

		[XmlAttribute]
		[Description( "Average size of spawn area" )]
		public FXDistribution3D Distribution { get; set; } = FXDistribution3D.UniformRadial;

		[XmlAttribute]
		[Description( "Average size of spawn area" )]
		public float AverageSize { get; set; } = 0;

		[XmlAttribute]
		[Description( "Average size of spawn area" )]
		public float SizeDeviation { get; set; } = 0;

		public override string ToString()
		{
			return string.Format( "D:{0}:{1:0.##} Sz:{2}:{3:0.##}", OffsetDirection, OffsetFactor, Distribution, AverageSize );
		}

		public Vector3 GetPosition ( FXEvent fxEvent, Random rand )
		{
			var position = FXFactory.GetPosition( OffsetDirection, OffsetFactor, fxEvent);
			var radial	= FXFactory.GetRadialDistribution( rand, Distribution, AverageSize, SizeDeviation );
			return position + radial;
		}
	}


	public class FXShape {
		[XmlAttribute]
		public float Size0 { get; set; } = 1;

		[XmlAttribute]
		public float Size1 { get; set; } = 1;

		[XmlAttribute]
		public bool EnableRotation { get; set; } = false;

		[XmlAttribute]
		public float InitialAngle { get; set; } = 0;

		[XmlAttribute]
		public float MinAngularVelocity { get; set; } = 0;

		[XmlAttribute]
		public float MaxAngularVelocity { get; set; } = 0;

		public override string ToString()
		{
			return string.Format( "{0} {1} {2:0.##} [{3:0.##} {4:0.##}]", Size0/2+Size1/2, EnableRotation?"Enabled":"Disabled", InitialAngle, MinAngularVelocity, MaxAngularVelocity );
		}

		public void GetAngles ( Random rand, float lifetime, out float a, out float b )
		{
			a = b = 0;

			if (!EnableRotation) {
				return;
			}

			var sign = (rand.NextFloat(-1,1) > 0) ? 1 : -1;

			a = MathUtil.DegreesToRadians( rand.NextFloat( -InitialAngle, InitialAngle ) );

			b = a + MathUtil.DegreesToRadians( rand.NextFloat( MinAngularVelocity, MaxAngularVelocity ) * lifetime * sign );
		}
	}


	public class FXParticleStage {

		public override string ToString()
		{
			if (Enabled) {
				return string.Format("{0} [{1}] [{2}]", Effect, Count, Sprite); 
			} else {
				return string.Format( "Disabled" );
			}
		}

		[Description( "Enables and disables current particle stage" )]
		public bool Enabled { get; set; } = false;

		[Description( "Particle sprite name" )]
		[Editor( typeof( SpriteFileLocationEditor ), typeof( UITypeEditor ) )]
		public string Sprite { get; set; } = "";

		[Description( "Particle visual effect" )]
		public ParticleFX Effect { get; set; } = ParticleFX.LitShadow;

		[Description( "Total number of emitted particles per active period" )]
		public int Count { get; set; } = 10;

		[Description( "Particle stage actie period" )]
		public float Period { get; set; } = 1;

		[Description( "Defines temporal properties of particle stage" )]
		[TypeConverter( typeof( ExpandableObjectConverter ) )]
		public FXTiming Timing { get; set; } = new FXTiming();

		[Description( "Minimal intensity color" )]
		public Color4 Color0 { get; set; } = Color4.Zero;

		[Description( "Maximal intensity color" )]
		public Color4 Color1 { get; set; } = new Color4( 1, 1, 1, 1 );

		[Description( "Defines life-time properties of particle stage" )]
		[TypeConverter( typeof( ExpandableObjectConverter ) )]
		public FXLifetime Lifetime { get; set; } = new FXLifetime();

		[Description( "Defines shape of particles" )]
		[TypeConverter( typeof( ExpandableObjectConverter ) )]
		public FXShape Shape { get; set; } = new FXShape();

		[Description( "Defines particle spawn area" )]
		[TypeConverter( typeof( ExpandableObjectConverter ) )]
		public FXPosition Position { get; set; } = new FXPosition();

		[Description( "Defines particle velocity" )]
		[TypeConverter( typeof( ExpandableObjectConverter ) )]
		public FXVelocity Velocity { get; set; } = new FXVelocity();

		[Description( "Defines particle damping and acceleration" )]
		[TypeConverter( typeof( ExpandableObjectConverter ) )]
		public FXAcceleration Acceleration { get; set; } = new FXAcceleration();
	}



	public class FXSoundStage {

		public override string ToString()
		{
			if ( Enabled ) {
				return string.Format( "{0} [{1}]", Attenuation, Sound );
			} else {
				return string.Format( "Disabled" );
			}
		}

		[XmlAttribute]
		[Description( "Enables and disables sound stage" )]
		public bool Enabled { get; set; } = false;

		[XmlAttribute]
		[Description( "Sound path" )]
		[Editor( typeof( SoundFileLocationEditor ), typeof( UITypeEditor ) )]
		public string Sound { get; set; } = "";

		[XmlAttribute]
		[Description( "Sound emitter radius" )]
		public FXSoundAttenuation Attenuation { get; set; } = FXSoundAttenuation.Normal;
	}



	public class FXLightStage {

		public override string ToString()
		{
			if ( Enabled ) {
				return string.Format( "R:{0} I:[{1}{2}{3}]", OuterRadius, Intensity.Red, Intensity.Green, Intensity.Blue );
			} else {
				return string.Format( "Disabled" );
			}
		}

		[XmlAttribute]
		[Description( "Enables and disables light stage" )]
		public bool Enabled { get; set; } = false;

		[XmlAttribute]
		[Description( "Enables and disables light stage" )]
		public float Period { get; set; } = 1;

		[Description( "Light intensity" )]
		public Color4 Intensity { get; set; } = new Color4(10,10,10,1);

		[XmlAttribute]
		[Description( "Light radius" )]
		public float InnerRadius { get; set; } = 0;

		[XmlAttribute]
		[Description( "Light radius" )]
		public float OuterRadius { get; set; } = 5;

		[XmlAttribute]
		[Description( "Pulse string: 'a' - means zero intensity, 'z' - means double intensity" )]
		public string PulseString { get; set; } = "m";

		[XmlAttribute]
		[Description( "Light style" )]
		public FXLightStyle LightStyle { get; set; } = FXLightStyle.Const;

		[XmlAttribute]
		[Description( "Offset direction" )]
		public FXDirection OffsetDirection { get; set; } = FXDirection.None;

		[XmlAttribute]
		[Description( "Offset along offset direction" )]
		public float OffsetFactor { get; set; } = 0;
	}


	public class FXCameraShake {

		public override string ToString()
		{
			if ( Enabled ) {
				return string.Format( "Enabled" );
			} else {
				return string.Format( "Disabled" );
			}
		}

		[XmlAttribute]
		[Description( "Enables and disables camera shake stage" )]
		public bool Enabled { get; set; } = false;

	}
}
