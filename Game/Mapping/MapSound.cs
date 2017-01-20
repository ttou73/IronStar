using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Fusion.Core.Mathematics;

namespace IronStar.Mapping {

	public class MapSound {

		[Category( "Sound" )]
		public string Sound { get; set; } = "";

		[Category( "Spot Light" )]
		public float Volume { get; set; } = 1.0f;

		[Category( "Spot Light" )]
		public float InnerRadius { get; set; } = 0;

		[Category( "Spot Light" )]
		public float OuterRadius { get; set; } = 10;
	}
}
