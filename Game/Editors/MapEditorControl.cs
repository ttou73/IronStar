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

namespace IronStar.Editors {
	public partial class MapEditorControl : UserControl {

		readonly Game Game;

		public MapEditorControl( Game game )
		{
			this.Game	=	game;
			InitializeComponent();
		}



		public void SetSelection ( IEnumerable<MapFactory> selection )
		{
			//propertyGridTransform.SelectedObjects = selection.Select( fact => fact.Transform ).ToArray();	
			propertyGridFactory	 .SelectedObjects = selection.Select( fact => fact.Factory ).ToArray();	

		}
		

		void RefreshMapListItems ()
		{
			
			//mapListBox.RefreshListBox();
		}



		void PopulatePropertyGrid ()
		{
			//mapPropertyGrid.SelectedObjects = mapListBox.SelectedItems.Cast<MapFactory>().Select( n => n.Factory ).ToArray();
		}



		private void mapListBox_SelectedIndexChanged( object sender, EventArgs e )
		{
			PopulatePropertyGrid();
		}

		private void mapPropertyGrid_PropertyValueChanged( object s, PropertyValueChangedEventArgs e )
		{
		}

	}
}
