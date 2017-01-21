using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fusion;
using Fusion.Build;
using Fusion.Engine.Common;
using IronStar.SFX;
using Fusion.Development;
using Fusion.Core.Extensions;
using IronStar.Editors;
using IronStar.Core;
using Fusion.Build.Mapping;

namespace IronStar.Editors {
	public partial class EditorForm : Form {

		readonly Game game;

		ObjectEditor	modelEditor;
		ObjectEditor	entityEditor;
		ObjectEditor	fxEditor;
		MapEditor		mapEditor;
		//VTEditor		vtEditor;
		ObjectEditor	vtEditor;


		/// <summary>
		/// 
		/// </summary>
		public EditorForm( Game game )
		{
			this.game		=	game;
			
			InitializeComponent();

			modelEditor		=	new ObjectEditor( game, "models", typeof(ModelDescriptor), "Model"  ) { Dock = DockStyle.Fill };
			entityEditor	=	new ObjectEditor( game, "entities", typeof(EntityFactory), "Entity" ) { Dock = DockStyle.Fill };
			fxEditor		=	new ObjectEditor( game, "fx", typeof(FXFactory), "FX" ) { Dock = DockStyle.Fill };
			mapEditor		=	new MapEditor() { Dock = DockStyle.Fill };
			//vtEditor		=	new VTEditor("megatexture.ini") { Dock = DockStyle.Fill };
			vtEditor		=	new ObjectEditor( game, "vt", typeof(VTTextureContent), "Megatexture" ) { Dock = DockStyle.Fill };
			//vtEditor.AddEditAction("QQQQQ", (list) => for

			mainTabs.TabPages["tabModels"].Controls.Add( modelEditor );
			mainTabs.TabPages["tabEntities"].Controls.Add( entityEditor );
			mainTabs.TabPages["tabMap"].Controls.Add( mapEditor );
			mainTabs.TabPages["tabFX"].Controls.Add( fxEditor );
			mainTabs.TabPages["tabMegatexture"].Controls.Add( vtEditor );

			Log.Message("Editor initialized");
		}




		/// <summary>
		/// 
		/// </summary>
		public void BuildContent ()
		{
			//mapEditor.SaveMap(false);
			//vtEditor.Save();

			Log.Message( "Building..." );
			Builder.SafeBuild();
			game.Reload();
		}


		/*-----------------------------------------------------------------------------------------
		 * 
		 *	Event handlers :
		 * 
		-----------------------------------------------------------------------------------------*/

		private void buttonExit_Click( object sender, EventArgs e )
		{
			Close();
		}

		private void buttonSaveAndBuild_Click( object sender, EventArgs e )
		{
			BuildContent();
		}

		private void exitToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Close();
		}

		private void buildToolStripMenuItem_Click( object sender, EventArgs e )
		{
			BuildContent();
		}

		private void EditorForm_FormClosing( object sender, FormClosingEventArgs e )
		{
			//mapEditor.CloseMap();
			//vtEditor.Save();
		}
	}
}
