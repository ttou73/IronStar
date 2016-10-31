using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Core.Shell;

namespace ShooterDemo.Commands {

	[Command("printCLWorldState", CommandAffinity.Client)]
	public class ShowCLWorldState : NoRollbackCommand {

		/// <summary>
		/// 
		/// </summary>
		/// <param name="invoker"></param>
		public ShowCLWorldState( Invoker invoker ) : base(invoker)
		{
			
		}


		/// <summary>
		/// 
		/// </summary>
		public override void Execute ()
		{
			(Invoker.Game.GameClient as ShooterClient).World.PrintState();
		}
	}



	[Command("printSVWorldState", CommandAffinity.Server)]
	public class ShowSVWorldState : NoRollbackCommand {

		/// <summary>
		/// 
		/// </summary>
		/// <param name="invoker"></param>
		public ShowSVWorldState( Invoker invoker ) : base(invoker)
		{
			
		}


		/// <summary>
		/// 
		/// </summary>
		public override void Execute ()
		{
			(Invoker.Game.GameServer as ShooterServer).World.PrintState();
		}
	}
}
