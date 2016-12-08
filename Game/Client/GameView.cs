using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Fusion;
using Fusion.Core.Mathematics;
using Fusion.Engine.Common;
using Fusion.Core.Content;
using Fusion.Engine.Server;
using Fusion.Engine.Client;
using Fusion.Core.Extensions;
using IronStar.SFX;
using Fusion.Core.IniParser.Model;
using IronStar.Views;
using Fusion.Engine.Graphics;


namespace IronStar.Core {
	class GameView {

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cl"></param>
		/// <param name="map"></param>
		public GameView ( GameClient cl, Map map )
		{
		}



		/// <summary>
		/// 
		/// </summary>
		public void FeedSnapshot ( GameTime serverTime, byte[] snapshot, uint ackCommandID )
		{
			
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update ( GameTime gameTime )
		{
		}


	}
}
