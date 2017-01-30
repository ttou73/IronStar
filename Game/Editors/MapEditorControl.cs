using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IronStar.Mapping;
using Fusion;
using Fusion.Core.Extensions;
using System.IO;
using Fusion.Build;
using Native.Fbx;
using System.Reflection;
using Fusion.Engine.Common;
using IronStar.Core;
using IronStar.Editor2;

namespace IronStar.Editors {
	public partial class MapEditorControl : UserControl {

		readonly Game Game;

		public MapEditorControl( Game game )
		{
			this.Game	=	game;
			InitializeComponent();

			PopulateCreateMenu();
		}



		void PopulateCreateMenu()
		{
			createToolStripMenuItem.Enabled = true;

			var types = Misc.GetAllSubclassesOf( typeof(EntityFactory), false );



			foreach ( var factType in types ) {

				var item    =   new ToolStripMenuItem();

				item.Text   =   factType.Name;

				item.Click  +=  ( s, e ) => {

					var fact  = new MapFactory();

					var mapEditor = Game.GameEditor.Instance as MapEditor;
	
					fact.Factory = (EntityFactory)Activator.CreateInstance( factType );
					
					mapEditor.Map.Factories.Add( fact );
					mapEditor.Select( fact );
				};

				createToolStripMenuItem.DropDownItems.Add( item );
			}

		}



		public void SetSelection ( IEnumerable<MapFactory> selection )
		{
			gridFactory.SelectedObjects		= selection.Select( fact => fact.Factory ).ToArray();	
			gridTransform.SelectedObjects	= selection.Select( fact => fact.Transform ).ToArray();	
			gridModel.SelectedObjects		= selection.Select( fact => fact.Model ).ToArray();	
		}
	}
}
