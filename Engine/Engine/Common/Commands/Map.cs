﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Core.Shell;

namespace Fusion.Engine.Common.Commands {

	[Command("map", CommandAffinity.Default)]
	public class MapCommand : NoRollbackCommand {

		[CommandLineParser.Required()]
		[CommandLineParser.Name("mapname")]
		public string MapName { get; set; }

		[CommandLineParser.Name("dedicated")]
		public bool Dedicated { get; set; }

		[CommandLineParser.Name("edit")]
		public bool Edit { get; set; }
			
		public MapCommand ( Invoker invoker ) : base(invoker) 
		{
		}

		public override void Execute ()
		{
			if (Edit) {
				Invoker.Game.GameEditor.Start( MapName );
			} else {
				Invoker.Game.StartServer( MapName, Dedicated );
			}
		}
	}
}
