using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Core.Shell;

namespace Fusion.Engine.Common.Commands {

	[Command("edClose", CommandAffinity.Default)]
	public class EdClose : NoRollbackCommand {
			
		public EdClose ( Invoker invoker ) : base(invoker) 
		{
		}

		public override void Execute ()
		{
			Invoker.Game.GameEditor.Stop();	
		}
	}
}
