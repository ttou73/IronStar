﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Core.Mathematics;
using Fusion.Core;
using Fusion.Drivers.Graphics;
using Fusion.Engine.Common;

namespace Fusion.Engine.Graphics {
	public class OmniLight {
		/// <summary>
		/// Omni-light position
		/// </summary>
		public Vector3	Position;

		/// <summary>
		/// Omni-light intensity
		/// </summary>
		public Color4	Intensity;

		/// <summary>
		/// Omni-light inner radius.
		/// </summary>
		public float	RadiusInner;

		/// <summary>
		/// Omni-light outer radius.
		/// </summary>
		public float	RadiusOuter;


		/// <summary>
		/// 
		/// </summary>
		public OmniLight ()
		{
			Position	=	Vector3.Zero;
			Intensity	=	Color4.Zero;
			RadiusInner	=	0;
			RadiusOuter	=	1;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="position"></param>
		/// <param name="color"></param>
		/// <param name="radius"></param>
		public OmniLight ( Vector3 position, Color4 color, float radius )
		{
			Position	=	position;
			Intensity	=	color;
			RadiusInner	=	0;
			RadiusOuter	=	radius;
		}
	}
}
