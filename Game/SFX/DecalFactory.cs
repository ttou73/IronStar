using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Core.Content;
using Fusion.Core.Extensions;
using Fusion.Core.Mathematics;
using Fusion.Development;
using Fusion.Engine.Storage;

namespace IronStar.SFX {
	public class DecalFactory {

		/// <summary>
		/// Image index in decal atlas
		/// </summary>
		[Category("Image")]
		[Editor( typeof( DecalFileLocationEditor ), typeof( UITypeEditor ) )]
		public string ImageName { get; set; } = "";

		/// <summary>
		/// 
		/// </summary>
		[Category("Size")]
		public float DefaultWidth { get; set;} = 0.5f;

		/// <summary>
		/// 
		/// </summary>
		[Category("Size")]
		public float DefaultHeight { get; set;} = 0.5f;

		/// <summary>
		/// 
		/// </summary>
		[Category("Size")]
		public float DefaultDepth { get; set;} = 0.25f;

		/// <summary>
		/// Decal emission intensity
		/// </summary>
		[Category("Properties")]
		public Color4 Emission { get; set;} = Color4.Zero;

		/// <summary>
		/// Decal base color
		/// </summary>
		[Category("Properties")]
		public Color BaseColor { get; set;} = new Color(128,128,128,255);

		/// <summary>
		/// Decal roughness
		/// </summary>
		[Category("Properties")]
		public float Roughness { get; set;}= 0.5f;

		/// <summary>
		/// Decal meatllic
		/// </summary>
		[Category("Properties")]
		public float Metallic { get; set;} = 0.5f;

		/// <summary>
		/// Color blend factor [0,1]
		/// </summary>
		[Category("Properties")]
		public float ColorFactor { get; set;} = 1.0f;

		/// <summary>
		/// Roughmess and specular blend factor [0,1]
		/// </summary>
		[Category("Properties")]
		public float SpecularFactor { get; set;} = 1.0f;

		/// <summary>
		/// Normalmap blend factor [-1,1]
		/// </summary>
		[Category("Properties")]
		public float NormalMapFactor { get; set;} = 1.0f;

	}



	/// <summary>
	/// Scene loader
	/// </summary>
	[ContentLoader( typeof( DecalFactory ) )]
	public sealed class DecalFactoryLoader : ContentLoader {

		public override object Load( ContentManager content, Stream stream, Type requestedType, string assetPath, IStorage storage )
		{
			using ( var sr = new StreamReader( stream ) ) {
				return Misc.LoadObjectFromXml( typeof(DecalFactory), stream, null );
			}
		}
	}
}
