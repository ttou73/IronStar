﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fusion.Engine.Common;
using Fusion.Engine.Graphics;
using System.IO;
using System.Diagnostics;


namespace Fusion.Development {
	public static class LaunchBox {

		public static bool ShowDialog ( Game game, string config, Action runEditor )
		{
			//var form = new Dashboard( game, config, true );
			var form = new LaunchBoxForm( game, config, runEditor );

			var dr = form.ShowDialog();

			if (dr==DialogResult.OK) {
				return true;
			} else {
				return false;
			}
		} 
	}

}
