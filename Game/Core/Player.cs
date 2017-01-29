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
using IronStar.Core;
using IronStar.Entities;

namespace IronStar {

	public class Player {

		static Random rand = new Random();

		/// <summary>
		/// User's GUID
		/// </summary>
		public Guid Guid { get; private set; }

		/// <summary>
		/// User's info
		/// </summary>
		public string UserInfo { get; private set; }

		/// <summary>
		///	Player score.
		/// </summary>
		public int Score;

		/// <summary>
		/// 
		/// </summary>
		public bool Ready;


		float respawnTime = 9999;


		public Entity PlayerEntity { get; private set; }


		/// <summary>
		/// 
		/// </summary>
		public UserCommand UserCmd = new UserCommand();


		/// <summary>
		/// 
		/// </summary>
		/// <param name="guid"></param>
		/// <param name="userInfo"></param>
		public Player ( Guid guid, string userInfo )
		{
			Guid		=	guid;
			UserInfo	=	userInfo;
			Score		=	0;
		}



		/// <summary>
		/// Called when player's entity was killed
		/// </summary>
		public void Killed ( Entity entity )
		{
			respawnTime	=	0;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="cmdData"></param>
		public void FeedCommand ( byte[] cmdData )
		{
			UserCmd	=	UserCommand.FromBytes( cmdData );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="world"></param>
		/// <param name="dt"></param>
		public void Update ( GameWorld world, float dt )
		{
			if (!Ready) {
				return;
			}

			if (respawnTime<20) {
				respawnTime += dt;
			}

			var player = world.GetEntityOrNull( "player", e => e.UserGuid==Guid );

			if (player!=null) {
				player.Rotation			=	Quaternion.RotationYawPitchRoll( UserCmd.Yaw, UserCmd.Pitch, UserCmd.Roll );
				player.UserCtrlFlags	=	UserCmd.CtrlFlags;
			}

			if (player==null) {
				if ( UserCmd.CtrlFlags.HasFlag(UserCtrlFlags.Attack) && respawnTime>1 || respawnTime>3 ) {
					player	=	Respawn(world);
				}
			}
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="world"></param>
		public Entity Respawn (GameWorld world)
		{
			var sp = world.GetEntities()
				.Where( e1 => e1.Controller is StartPoint && (e1.Controller as StartPoint).StartPointType==StartPointType.SinglePlayer)
				.OrderBy( e => rand.Next() )
				.FirstOrDefault();					

			Entity ent;

			if (sp==null) {
				#warning do not spawn player
				Log.Warning("No 'startPoint' found");

				ent = world.Spawn( "player", 0, Vector3.Up*10, Quaternion.Identity );
				world.SpawnFX( "TeleportOut", ent.ID, Vector3.Up*10 );
				ent.UserGuid = Guid;

				PlayerEntity = ent;

				return ent;
			}

			ent = world.Spawn( "player", 0, sp.Position, sp.Rotation );
			world.SpawnFX("TeleportOut", ent.ID, sp.Position );
			ent.UserGuid = Guid;

			PlayerEntity = ent;

			return ent;
		}
	}
}
