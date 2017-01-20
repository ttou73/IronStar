using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Fusion.Core.Mathematics;

namespace IronStar.Mapping {

	public class MapSpotLight {

		[Category( "Spot Light" )]
		public Color4 Intensity { get; set; } = new Color4(1,1,1,0);

		[Category( "Spot Light" )]
		public float InnerRadius { get; set; } = 0.125f;

		[Category( "Spot Light" )]
		public float OuterRadius { get; set; } = 1.0f;

		[Category( "Spot Light" )]
		public float SpotWidth { get; set; } = 1.0f;

		[Category( "Spot Light" )]
		public float SpotHeight { get; set; } = 1.0f;

		[Category( "Spot Light" )]
		public string SpotMask { get; set; } = "";

		[Category( "Spot Light" )]
		public float Period { get; set; } = 1;

		[Category( "Spot Light" )]
		public string Pulse { get; set; } = "z";
	}
}
