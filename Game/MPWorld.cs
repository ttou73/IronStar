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

		
		readonly string mapName;

		
		public MPWorld( GameServer server, string map ) : base(server)
		{
			InitPhysSpace(16);
			this.mapName	=	map;

			InitializePrefabs();

			var scene = Content.Load<Scene>(@"scenes\" + map );

			ReadMapFromScene( Content, scene, IsClientSide );

			EntityKilled += MPWorld_EntityKilled;
		}



		public MPWorld( GameClient client, string serverInfo ) : base(client)
		{
			InitPhysSpace(16);

			InitializePrefabs();

			var scene = Content.Load<Scene>(@"scenes\" + serverInfo );

			ReadMapFromScene( Content, scene, IsClientSide );
		}



		public override void FinalizeLoad ()
		{
			AddMeshInstances();
		}



		public override void Cleanup ()
		{
			base.Cleanup();
			if (IsClientSide) {
				Game.RenderSystem.RenderWorld.ClearWorld();
			}
		}


		public override string ServerInfo ()
		{
			return mapName;
		}


		public override void PresentWorld ( float deltaTime, float lerpFactor )
		{
			base.PresentWorld( deltaTime, lerpFactor );
		}



		public override void SimulateWorld ( float elapsedTime )
		{
			if (IsServerSide) {

				UpdatePlayers( elapsedTime );

				var dt	=	1 / GameServer.TargetFrameRate;
				physSpace.TimeStepSettings.MaximumTimeStepsPerFrame = 6;
				physSpace.TimeStepSettings.TimeStepDuration = 1.0f/60.0f;
				physSpace.Update(dt);
			}

			base.SimulateWorld( elapsedTime );
		}



		public override void WriteToSnapshot ( BinaryWriter writer )
		{
			base.WriteToSnapshot( writer );
		}



		public override void ReadFromSnapshot ( BinaryReader reader, uint ackCmdID, float lerpFactor )
		{
			base.ReadFromSnapshot( reader, ackCmdID, lerpFactor );
		}
	}
}
