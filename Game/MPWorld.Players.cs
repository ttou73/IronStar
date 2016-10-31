using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Fusion;
using Fusion.Core;
using Fusion.Core.Content;
using Fusion.Core.Mathematics;
using Fusion.Engine.Common;
using Fusion.Engine.Input;
using Fusion.Engine.Client;
using Fusion.Engine.Server;
using Fusion.Engine.Graphics;
using ShooterDemo.Core;
using ShooterDemo.Views;
using ShooterDemo.Controllers;

namespace ShooterDemo {
	public partial class MPWorld : World {

		Random rand = new Random();

		/// <summary>
		/// List of players;
		/// </summary>
		public readonly List<Player> Players = new List<Player>();



		/// <summary>
		/// 
		/// </summary>
		/// <param name="guid"></param>
		/// <returns></returns>
		Player GetPlayer ( Guid guid )
		{
			return Players.LastOrDefault( p => p.Guid==guid );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="guid"></param>
		/// <param name="n"></param>
		public void AddScore ( Guid guid, int n )
		{
			var p = GetPlayer(guid);
			if (p!=null) {
				p.Score += n;
			}
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="dt"></param>
		public void UpdatePlayers ( float dt )
		{
			foreach ( var player in	Players ) {
				player.Update(this, dt);
			}
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="guid"></param>
		/// <param name="command"></param>
		/// <param name="lag"></param>
		public override void PlayerCommand ( Guid guid, byte[] command, float lag )
		{
			var p = GetPlayer(guid);
			if (p!=null) {
				p.FeedCommand(command);
			}
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void MPWorld_EntityKilled ( object sender, EntityEventArgs e )
		{
			foreach ( var pe in Players.Where( p => p.PlayerEntity == e.Entity ) ) {
				pe.Killed(e.Entity);
			}
			
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="guid"></param>
		/// <param name="userInfo"></param>
		/// <returns></returns>
		public override bool ApprovePlayer ( Guid guid, string userInfo )
		{
			return !Players.Any( p => p.Guid == guid );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="guid"></param>
		/// <param name="userInfo"></param>
		public override void PlayerConnected ( Guid guid, string userInfo )
		{
			LogTrace("player connected: {0} {1}", guid, userInfo );
			Players.Add( new Player( guid, userInfo ) );
		}



		/// <summary>
		/// Called internally when player entered.
		/// </summary>
		/// <param name="guid"></param>
		public override void PlayerEntered ( Guid guid )
		{
			LogTrace("player entered: {0}", guid );

			var p = GetPlayer(guid);

			if (p!=null) {
				p.Ready = true;
			}
		}



		/// <summary>
		/// Called internally when player left.
		/// </summary>
		/// <param name="guid"></param>
		public override void PlayerLeft ( Guid guid )
		{
			LogTrace("player left: {0}", guid );

			var ent = GetEntityOrNull( e => e.UserGuid == guid );

			if (ent!=null) {
				Kill( ent.ID );
			}
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="guid"></param>
		public override void PlayerDisconnected ( Guid guid )
		{
			LogTrace("player diconnected: {0}", guid );
			Players.RemoveAll( p => p.Guid == guid );
		}

	}
}
