using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Core.Mathematics;
using Fusion.Core;
using Fusion.Drivers.Graphics;
using Fusion.Engine.Common;

namespace Fusion.Engine.Graphics {
	public class EnvLight {

		internal int radianceCacheIndex;

		/// <summary>
		/// Environment light position
		/// </summary>
		public Vector3	Position { get; set; }

		/// <summary>
		/// Size of light probe
		/// </summary>
		public Vector3	Dimensions { get; set; }

		/// <summary>
		/// Outer radius of the environment light.
		/// </summary>
		public float	Factor { get; set; }

		/// <summary>
		/// Creates instance of EnvLight
		/// </summary>
		public EnvLight ()
		{
			Position	=	Vector3.Zero;
			Dimensions	=	Vector3.Zero;
			Factor		=	0;
		}


		/// <summary>
		/// Creates instance of EnvLight
		/// </summary>
		/// <param name="position"></param>
		/// <param name="innerRadius"></param>
		/// <param name="outerRadius"></param>
		public EnvLight ( Vector3 position, float w, float h, float d, float f )
		{
			this.Position		=	position;
			this.Dimensions		=	new Vector3(w,h,d);
			this.Factor			=	f;
		}
		
	}
}
