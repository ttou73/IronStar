using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Fusion.Development;
using Fusion.Core.Mathematics;
using Fusion.Core.Extensions;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Xml.Serialization;
using IronStar.Editors;
using Fusion.Core.Content;
using Fusion.Engine.Storage;
using System.IO;
using Fusion.Engine.Graphics;

namespace IronStar.SFX {
	public class ModelDescriptor : IPrecachable {

		[Category( "Appearance" )]
		[Description( "Path to FBX scene" )]
		[Editor( typeof( FbxFileLocationEditor ), typeof( UITypeEditor ) )]
		public string ScenePath { get; set; } = "";

		[Category( "Appearance" )]
		[Description( "Entire model scale" )]
		public float Scale { get; set; } = 1;

		[Category( "Appearance" )]
		[Description( "Model glow color multiplier" )]
		public Color4 Color { get; set; } = new Color4( 10, 10, 10, 1 );



		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Matrix ComputePreTransformMatrix()
		{
			return Matrix.Scaling( Scale );
		}


		public static string SaveToXml( ModelDescriptor descriptor )
		{
			return Misc.SaveObjectToXml( descriptor, descriptor.GetType() );
		}


		public static ModelDescriptor LoadFromXml( string xmlText )
		{
			return (ModelDescriptor)Misc.LoadObjectFromXml( typeof( ModelDescriptor ), xmlText );
		}


		public void Precache( ContentManager content )
		{
			content.Precache<Scene>(ScenePath);
		}
	}



	/// <summary>
	/// Scene loader
	/// </summary>
	[ContentLoader( typeof( ModelDescriptor ) )]
	public sealed class ModelDescriptorLoader : ContentLoader {

		public override object Load( ContentManager content, Stream stream, Type requestedType, string assetPath, IStorage storage )
		{
			using ( var sr = new StreamReader( stream ) ) {
				return ModelDescriptor.LoadFromXml( sr.ReadToEnd() );
			}
		}
	}
}
