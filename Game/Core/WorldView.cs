using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion;
using Fusion.Core;
using Fusion.Core.Content;
using Fusion.Core.Mathematics;
using Fusion.Engine.Common;
using Fusion.Engine.Input;
using Fusion.Engine.Client;
using Fusion.Engine.Server;
using Fusion.Engine.Graphics;


namespace ShooterDemo.Core {
	public abstract class WorldView {
		
		public readonly Game Game;
		public readonly World World;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="world"></param>
		public WorldView ( World world )
		{
			this.World	=	world;
			this.Game	=	world.Game;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="elapsedTime"></param>
		/// <param name="lerpFactor"></param>
		public abstract void Update ( float elapsedTime, float lerpFactor );
	}
}
