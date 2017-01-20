using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Fusion.Core.Mathematics;

namespace IronStar.Mapping {

	public class MapOmniLight {

		[Category( "Omni Light" )]
		public Color4 Intensity { get; set; } = new Color4(1,1,1,0);

		[Category( "Omni Light" )]
		public float InnerRadius { get; set; } = 0.125f;

		[Category( "Omni Light" )]
		public float OuterRadius { get; set; } = 1.0f;

		[Category( "Omni Light" )]
		public float Period { get; set; } = 1;

		[Category( "Omni Light" )]
		public string Pulse { get; set; } = "z";
	}
}
