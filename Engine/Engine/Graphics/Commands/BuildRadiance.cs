using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Core.Shell;

namespace Fusion.Engine.Graphics.Commands {
	[Command("buildRadiance", CommandAffinity.Default)]
	public class BuildRadiance : NoRollbackCommand {


		/// <summary>
		/// 
		/// </summary>
		/// <param name="invoker"></param>
		public BuildRadiance ( Invoker invoker ) : base(invoker) 
		{
		}


		public override void Execute ()
		{
			Game.RenderSystem.RenderWorld.CaptureRadiance();
		}
	}
}
