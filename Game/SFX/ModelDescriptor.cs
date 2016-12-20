﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
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

namespace IronStar.SFX {
	public class ModelDescriptor {

		[Category( "General" )]
		[XmlAttribute]
		[Editor(typeof(FbxFileLocationEditor), typeof(UITypeEditor))]
		public string ModelPath { get; set; } = "";

		[Category( "Appearance" )]
		[XmlAttribute]
		public float Scale { get; set; } = 1;

		[Category( "Appearance" )]
		public Color4 Color { get; set; } = new Color4(10,10,10,1);



		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Matrix ComputePreTransformMatrix ()
		{
			return Matrix.Scaling( Scale );
		}


		public static string SaveToXml ( ModelDescriptor descriptor )
		{
			return Misc.SaveObjectToXml( descriptor, descriptor.GetType() );
		}

		public static ModelDescriptor LoadFromXml( string xmlText )
		{
			return (ModelDescriptor)Misc.LoadObjectFromXml( typeof(ModelDescriptor), xmlText );
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
