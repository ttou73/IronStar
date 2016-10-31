using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fusion;
using Fusion.Engine.Client;
using Fusion.Engine.Common;
using Fusion.Engine.Server;
using Fusion.Core.Content;
using Fusion.Engine.Graphics;
using ShooterDemo.Core;
using Fusion.Core.Configuration;

namespace ShooterDemo {
	public partial class ShooterServer : GameServer {

		[Config] public float ServerTimeDriftRate { get; set; }
		[Config] public int SimulateDelay { get; set; }
		[Config] public bool ShowFootSteps { get; set; }
		[Config] public bool ShowFallings { get; set; }



		void SetDefaults ()
		{
			TargetFrameRate		=	60;
			SimulateDelay		=	0;
			ServerTimeDriftRate	=	0.001f;
		}
	}
}
