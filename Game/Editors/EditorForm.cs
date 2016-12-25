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

namespace IronStar.Editors {
	public partial class EditorForm : Form {

		readonly Game game;

		ObjectEditor modelEditor;
		ObjectEditor entityEditor;
		MapEditor	mapEditor;


		/// <summary>
		/// 
		/// </summary>
		public EditorForm( Game game )
		{
			this.game		=	game;
			
			InitializeComponent();

			modelEditor		=	new ObjectEditor( game, "models", typeof(ModelDescriptor), "Model"  ) { Dock = DockStyle.Fill };
			entityEditor	=	new ObjectEditor( game, "entities", typeof(EntityFactory), "Entity" ) { Dock = DockStyle.Fill };
			mapEditor		=	new MapEditor() { Dock = DockStyle.Fill };

			mainTabs.TabPages["tabModels"].Controls.Add( modelEditor );
			mainTabs.TabPages["tabEntities"].Controls.Add( entityEditor );
			mainTabs.TabPages["tabMap"].Controls.Add( mapEditor );


			Log.Message("Editor initialized");
		}




		/// <summary>
		/// 
		/// </summary>
		public void BuildContent ()
		{
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

		private void addModelToolStripMenuItem_Click( object sender, EventArgs e )
		{
			mainTabs.SelectTab("tabModels");
			modelEditor?.AddNewObjectUI();
		}

		private void removeModelToolStripMenuItem_Click( object sender, EventArgs e )
		{
			mainTabs.SelectTab("tabModels");
			modelEditor?.RemoveObjectUI();
		}

		private void exitToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Close();
		}

		private void buildToolStripMenuItem_Click( object sender, EventArgs e )
		{
			BuildContent();
		}
	}
}
