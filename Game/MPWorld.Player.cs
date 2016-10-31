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
			public void Update ( World world, float dt )
			{
				if (!Ready) {
					return;
				}

				if (respawnTime<20) {
					respawnTime += dt;
				}

				var player = world.GetEntityOrNull( e => e.UserGuid==Guid );

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
			public Entity Respawn (World world)
			{
				var sp = world.GetEntities("startPoint").OrderBy( e => rand.Next() ).FirstOrDefault();					

				if (sp==null) {
					Log.Warning("No 'startPoint' found");
				}

				var ent = world.Spawn( "player", 0, sp.Position, sp.Rotation );
				world.SpawnFX("TeleportOut", ent.ID, sp.Position );
				ent.UserGuid = Guid;

				PlayerEntity = ent;

				return ent;
			}
		}

	}
}
