using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Core.Mathematics;
using IronStar.Core;
using Fusion.Engine.Graphics;

namespace IronStar.Mapping {
	public class MapEnvironment {

		[Category( "Physics" )]
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


		[Category("AI")]
		[Description("Character height")]
		public float CharacterHeight { get; set; } = 2;

		[Category("AI")]
		[Description("Character size")]
		public float CharacterSize { get; set; } = 1.2f;

		[Category("AI")]
		[Description("Walkable slope angle")]
		public float WalkableSlope { get; set; } = 45f;

		[Category("AI")]
		[Description("Stair step or climbable height")]
		public float StepHeight { get; set; } = 45f;

		[Category("AI")]
		[Description("Cell height for Recast voxelization")]
		public float RecastCellHeight { get; set; } = 0.25f;

		[Category("AI")]
		[Description("Cell width for Recast voxelization")]
		public float RecastCellSize { get; set; } = 0.25f;

		[Category("GI")]
		[TypeConverter( typeof( ExpandableObjectConverter ) )]
		public BoundingBox IrradianceVolume { get; set; } = new BoundingBox(128,64,128);
	}
}
