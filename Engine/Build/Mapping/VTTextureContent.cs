using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Fusion;
using Fusion.Core.Mathematics;
using Fusion.Core.Extensions;
using Fusion.Engine.Graphics;
using Fusion.Engine.Imaging;
using Fusion.Engine.Storage;
using Fusion.Core.IniParser.Model;
using Fusion.Core.Content;
using System.Drawing.Design;
using Fusion.Development;
using System.Windows.Forms;
using System.ComponentModel;


namespace Fusion.Build.Mapping {

	public class VTTextureContent {

		[Category( "General" )]
		[ReadOnly( true )]
		public string KeyPath { get; set; }

		[Category( "General" )]
		public bool SkipProcessing { get; set; }

		[Category( "Textures" )]
		[Editor( typeof(ImageFileLocationEditor), typeof( UITypeEditor ) )]
		public string  BaseColor { get;set; } = "";

		[Category( "Textures" )]
		[Editor( typeof(ImageFileLocationEditor), typeof( UITypeEditor ) )]
		public string  NormalMap { get;set; } = "";

		[Category( "Textures" )]
		[Editor( typeof(ImageFileLocationEditor), typeof( UITypeEditor ) )]
		public string  Metallic { get;set; } = "";

		[Category( "Textures" )]
		[Editor( typeof(ImageFileLocationEditor), typeof( UITypeEditor ) )]
		public string  Roughness { get;set; } = "";

		[Category( "Textures" )]
		[Editor( typeof(ImageFileLocationEditor), typeof( UITypeEditor ) )]
		public string  Emission { get;set; } = "";

		[Category( "Surface" )]
		public bool Transparent { get;set; } = false;


		[Browsable(true)]
		[DisplayName("Replace Texture Names")]
		public void AutoReplaceTextureNames ()
		{
			NormalMap	= ReplaceIfExists( BaseColor, "NormalMap" );
			Metallic	= ReplaceIfExists( BaseColor, "Metallic"  );
			Roughness	= ReplaceIfExists( BaseColor, "Roughness" );
			Emission	= ReplaceIfExists( BaseColor, "Emission"  );
		}


		string ReplaceIfExists ( string baseColor, string suffix )
		{
			var dir = Builder.FullInputDirectory;

			var fn  = baseColor.Replace("_BaseColor.", "_" + suffix + "." );

			if ( File.Exists( Path.Combine(dir,fn) ) ) { 
				return fn;
			} else {
				return "";
			}
		}


		[Browsable(true)]
		[DisplayName("Import from Scene...")]
		public static void ImportFromScene ()
		{
			Log.Warning("Not implemented");
		}
	}
}
