using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fusion.Build;
using Fusion.Core.IniParser;
using System.IO;
using Fusion.Core.IniParser.Model;
using System.Drawing.Design;
using System.Reflection;
using Fusion.Development;
using Fusion;
using Native.Fbx;

namespace IronStar.Editors {
	public partial class VTEditor : UserControl {

		class VTTexture {
			[Category("General")]
			[ReadOnly(true)]
			public string  KeyPath { get;set; }

			[Category( "Textures" )]
			[Editor( typeof( ImageFileLocationEditor ), typeof( UITypeEditor ) )]
			public string  BaseColor { get;set; } = "";

			[Category( "Textures" )]
			[Editor( typeof( ImageFileLocationEditor ), typeof( UITypeEditor ) )]
			public string  NormalMap { get;set; } = "";

			[Category( "Textures" )]
			[Editor( typeof( ImageFileLocationEditor ), typeof( UITypeEditor ) )]
			public string  Metallic { get;set; } = "";

			[Category( "Textures" )]
			[Editor( typeof( ImageFileLocationEditor ), typeof( UITypeEditor ) )]
			public string  Roughness { get;set; } = "";

			[Category( "Textures" )]
			[Editor( typeof( ImageFileLocationEditor ), typeof( UITypeEditor ) )]
			public string  Emission { get;set; } = "";

			[Category( "Surface" )]
			public bool Transparent { get;set; } = false;

			[Browsable(false)]
			public bool Modified = false;



			public void FromSection ( SectionData section ) 
			{
				BaseColor	=	section.Keys["BaseColor"] ?? "";
				NormalMap	=	section.Keys["NormalMap"] ?? "";
				Metallic	=	section.Keys["Metallic"	] ?? "";
				Roughness	=	section.Keys["Roughness"] ?? "";
				Emission	=	section.Keys["Emission"	] ?? "";
				Transparent	=	section.Keys["Transparent"]=="True";
			}


			public void ToSection ( SectionData section )
			{
				section.Keys.AddKey( "BaseColor",	BaseColor );
				section.Keys.AddKey( "NormalMap",	NormalMap );
				section.Keys.AddKey( "Metallic",	Metallic );
				section.Keys.AddKey( "Roughness",	Roughness );
				section.Keys.AddKey( "Emission",	Emission );
				section.Keys.AddKey( "Transparent", Transparent ? "True" : "False" );
			}

			public override string ToString()
			{
				return KeyPath + (Modified ? "*" : "");
			}
		}


		BindingList<VTTexture> textureList;
		readonly string path;
		readonly string fullPath;


		public VTEditor( string path )
		{
			this.path	=	path;
			fullPath	=	Builder.GetFullPath( path );

			InitializeComponent();

			textureList = OpenMegatextureFile();

			textureListBox.DataSource = textureList;
		}




		BindingList<VTTexture> OpenMegatextureFile()
		{
			var ip = new StreamIniDataParser();
			ip.Parser.Configuration.AllowDuplicateSections  =   false;
			ip.Parser.Configuration.AllowDuplicateKeys      =   true;
			ip.Parser.Configuration.CommentString           =   "#";
			ip.Parser.Configuration.OverrideDuplicateKeys   =   true;
			ip.Parser.Configuration.KeyValueAssigmentChar   =   '=';
			ip.Parser.Configuration.AllowKeysWithoutValues  =   false;

			var list = new BindingList<VTTexture>();

			using ( var reader = new StreamReader( File.OpenRead(fullPath) ) ) {
				var data = ip.ReadData( reader );

				foreach ( var section in data.Sections ) {

					var tex = new VTTexture();
					tex.KeyPath = section.SectionName;
					tex.FromSection( section );

					list.Add( tex );
				}
			}

			return list;
		}


		public void Save()
		{
			SaveMegatextureFile();
		}


		void SaveMegatextureFile ()
		{
			var list = textureList;
			var ip = new StreamIniDataParser();
			ip.Parser.Configuration.AllowDuplicateSections  =   false;
			ip.Parser.Configuration.AllowDuplicateKeys      =   true;
			ip.Parser.Configuration.CommentString           =   "#";
			ip.Parser.Configuration.OverrideDuplicateKeys   =   true;
			ip.Parser.Configuration.KeyValueAssigmentChar   =   '=';
			ip.Parser.Configuration.AllowKeysWithoutValues  =   false;

			var data = new IniData();

			foreach ( var tex in list ) {
				var section = new SectionData( tex.KeyPath );
				tex.ToSection( section );
				tex.Modified = false;
				data.Sections.Add( section );
			}

			using ( var writer = new StreamWriter( File.OpenWrite( fullPath ) ) ) {
				ip.WriteData( writer, data );
			}
		}



		private void textureListBox_SelectedIndexChanged( object sender, EventArgs e )
		{
			texturePropertyGrid.SelectedObjects = textureListBox.SelectedItems.Cast<VTTexture>().ToArray();
		}

		private void saveToolStripMenuItem_Click( object sender, EventArgs e )
		{
			SaveMegatextureFile();
		}

		private void newTextureToolStripMenuItem_Click( object sender, EventArgs e )
		{
			var name = "";

			while (true) {
				name = NameDialog.Show(this, "New texture", "New texture", name);

				if (name==null) {
					return;
				}

				if (textureList.Any( t => t.KeyPath==name )) {
					MessageBox.Show(this, "Texture '" + name + "' already exists", "New texture", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				} else {
					
					var tex = new VTTexture();
					tex.KeyPath = name;
					tex.Modified = true;

					textureList.Add( tex );

					return;
				}
			}
		}

		private void renameTextureToolStripMenuItem_Click( object sender, EventArgs e )
		{
			var index = textureListBox.SelectedIndex;
			var selected = (VTTexture)textureListBox.SelectedItem;
			var name = selected.KeyPath;

			while ( true ) {
				name = NameDialog.Show( this, "New texture", "New texture", name );

				if ( name==null ) {
					return;
				}

				if ( textureList.Any( t => t.KeyPath==name ) ) {
					MessageBox.Show( this, "Texture '" + name + "' already exists", "New texture", MessageBoxButtons.OK, MessageBoxIcon.Warning );
				} else {

					selected.KeyPath = name;

					textureListBox.RefreshListBox();

					return;
				}
			}
		}

		private void removeToolStripMenuItem_Click( object sender, EventArgs e )
		{
			var r = MessageBox.Show(this, "Are you sure to remove selected items?", "Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Warning );

			if ( r!=DialogResult.Yes ) {
				return;
			}

			var selected = textureListBox.SelectedItems.Cast<VTTexture>().ToArray();

			foreach ( var so in selected ) {
				textureList.Remove( so );
			}

			textureListBox.RefreshListBox();
		}

		private void importFromFBXSceneToolStripMenuItem_Click( object sender, EventArgs e )
		{
			string fullPath;

			if ( Editor.OpenFileDialog( "Select Scene to Import Nodes", "FBX Scene (*.fbx)|*.fbx", "Scenes", false, out fullPath ) ) {

				var loader = new FbxLoader();
				var options = new Options();
				var scene = loader.LoadScene( Builder.GetFullPath( fullPath ), new Options() { ImportGeometry=true } );

				var list = CheckListDialog.ShowDialog(this, "Select materials to import:", "Import Materials", scene.Materials.Select( m => m.Name ));

				foreach ( var name in list ) {
				
					if ( textureList.Any( m=>m.KeyPath==name ) ) {
						continue;
					}

					var tex = new VTTexture();
					tex.KeyPath = name;
					tex.Modified = true;

					textureList.Add( tex );
				}
			}

		}
	}
}
