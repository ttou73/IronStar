using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Fusion.Core.Mathematics;

namespace IronStar.Mapping {

	public class MapEnvironment : MapNode {

		[Category("Physics")]
		public float Gravity { get; set; } = 16;

		[Category( "Sky" )]
		public float SkyTrubidity {
			get {
				return turbidity;
			}
			set {
				turbidity = MathUtil.Clamp( value, 2, 8 );
			}
		}
		float turbidity = 2;

		[Category( "Sky" )]
		public Vector3 SunPosition { get; set; } = Vector3.One;

		[Category( "Sky" )]
		public float SunIntensity { get; set; } = 100;

		[Category( "Fog" )]
		public float FogDensity { get; set; } = 0.001f;


		public override string ToString()
		{
			return "(Environment)";
		}
	}
}
