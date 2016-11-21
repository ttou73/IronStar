using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Core;
using Fusion.Core.Mathematics;
using Fusion.Core.Configuration;
using Fusion.Engine.Common;
using Fusion.Drivers.Graphics;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Fusion.Core.Extensions;

namespace Fusion.Engine.Graphics {

	public class FogSettings {

		/// <summary>
		/// Aerial fog density.
		/// </summary>
		public float Density { 
			get {
				return density;
			}
			set {
				density	=	value;
			}
		}

		float density	=	0.01f;



		/// <summary>
		/// 
		/// </summary>
		public FogSettings ()
		{
		}
	}
}
