using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Core.Shell;

namespace Fusion.Engine.Common.Commands {

	[Command("atoms", CommandAffinity.Client)]
	public class Atoms : NoRollbackCommand {

		static public bool crashRequested = false;

			
		public Atoms ( Invoker invoker ) : base(invoker) 
		{
		}

		public override void Execute ()
		{
			var atoms = Invoker.Game.GameClient.Atoms.ToArray();

			for (int i=1; i<atoms.Length; i++) {
				Log.Message("{0} : {1}", i, atoms[i] );
			}
		}
	}
}
