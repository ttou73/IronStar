using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Core.Shell;

namespace Fusion.Engine.Graphics.Commands {
	[Command("vtrestart", CommandAffinity.Default)]
	public class VTRestart : NoRollbackCommand {


		[CommandLineParser.Name("path")]
		public string Path { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="invoker"></param>
		public VTRestart ( Invoker invoker ) : base(invoker) 
		{
			Path	=	null;
		}


		public override void Execute ()
		{
			Game.RenderSystem.RenderWorld.VirtualTexture = null;
			Game.RenderSystem.RenderWorld.VirtualTexture = Game.Content.Load<VirtualTexture>("*megatexture");
		}
	}
}
