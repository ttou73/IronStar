using System;
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

namespace IronStar.SFX {
	public class ModelDescriptor {

		[Category( "General" )]
		[ReadOnly( true )]
		[XmlAttribute]
		public string Name { get; set; }

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


		public static string SaveCollectionToXml( ICollection<ModelDescriptor> models )
		{
			var array = models.ToArray();

			return Misc.SaveObjectToXml( array, array.GetType() );
		}

		public static ICollection<ModelDescriptor> LoadCollectionFromXml( string xmlText )
		{
			return (ICollection<ModelDescriptor>)Misc.LoadObjectFromXml( typeof( ModelDescriptor[] ), xmlText );
		}
	}
}
