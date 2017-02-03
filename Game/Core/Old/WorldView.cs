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


namespace IronStar.Core {
	public abstract class WorldView {
		

		/// <summary>
		/// 
		/// </summary>
		/// <param name="world"></param>
		public WorldView ( GameWorld world )
		{
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="elapsedTime"></param>
		/// <param name="lerpFactor"></param>
		public abstract void Update ( float elapsedTime, float lerpFactor );
	}
}
