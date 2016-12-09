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
	public partial class GameView {

		EntityCollection entities;
		FXPlayback fxPlayback;
		SnapshotReader snapshotReader;


		/// <summary>
		/// 
		/// </summary>
		public EntityCollection Entities {
			get {
				return entities;
			}
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="cl"></param>
		/// <param name="map"></param>
		public GameView ( GameClient cl, Map map )
		{
			entities		=	new EntityCollection( cl.Atoms );
			snapshotReader	=	new SnapshotReader();
		}



		/// <summary>
		/// 
		/// </summary>
		public void Cleanup ()
		{
		}



		/// <summary>
		/// 
		/// </summary>
		public void FeedSnapshot ( GameTime serverTime, byte[] snapshot, uint ackCommandID )
		{
			using ( var ms = new MemoryStream( snapshot ) ) {
				snapshotReader.Read( ms, entities, null, null, null );
			}
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update ( GameTime gameTime )
		{
			//fxPlayback.
		}


	}
}
