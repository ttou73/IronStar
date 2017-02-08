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
	
	public class Decal {

		/// <summary>
		/// Decal basis matrix
		/// </summary>
		public Matrix DecalMatrix;

		/// <summary>
		/// Image index in decal atlas
		/// </summary>
		public int ImageIndex;

		/// <summary>
		/// Decal emission intensity
		/// </summary>
		public Color4 Emission = Color4.Zero;

		/// <summary>
		/// Decal base color
		/// </summary>
		public Color BaseColor = new Color(128,128,128,255);

		/// <summary>
		/// Decal roughness
		/// </summary>
		public float Roughness = 0.5f;

		/// <summary>
		/// Decal meatllic
		/// </summary>
		public float Metallic = 0.5f;

		/// <summary>
		/// Color blend factor [0,1]
		/// </summary>
		public float ColorFactor = 1.0f;

		/// <summary>
		/// Roughmess and specular blend factor [0,1]
		/// </summary>
		public float SpecularFactor = 1.0f;

		/// <summary>
		/// Normalmap blend factor [-1,1]
		/// </summary>
		public float NormalMapFactor = 1.0f;



		/// <summary>
		/// 
		/// </summary>
		public Decal ()
		{
		}
	}
}
