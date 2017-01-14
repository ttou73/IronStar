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
using IronStar.Mapping;
using IronStar.Client;

namespace IronStar.Core {

	/// <summary>
	/// World represents entire game state.
	/// </summary>
	public partial class GameWorld : IServerInstance, IClientInstance {

		SpriteLayer	hudLayer;
		GameInput gameInput;

		public SpriteLayer HudLayer {
			get { return hudLayer; }
		}


		readonly Guid clientGuid = new Guid();


		/// <summary>
		/// Initializes client-side world.
		/// </summary>
		/// <param name="client"></param>
		public GameWorld ( GameClient client, Guid clientGuid )
		{
			this.clientGuid	=	clientGuid;

			gameInput	=	new GameInput(client.Game);

			Log.Verbose("world: client");
			this.serverSide	=	false;
			this.Game		=	client.Game;
			this.UserGuid	=	clientGuid;
			Content			=	new ContentManager(Game);
			entities		=	new EntityCollection(null);
			fxPlayback		=	new SFX.FXPlayback(this);
			modelManager	=	new ModelManager(this);

			hudLayer		=	new SpriteLayer( Game.RenderSystem, 1024);

			AddView( new Hud( this ) );
			AddView( new GameCamera( this ) );

			//------------------------

			Game.Reloading += (s,e) => ForEachEntity( ent => ent.MakeRenderStateDirty() );
		}


		public void FeedAtoms( AtomCollection atoms )
		{
			this.Atoms = atoms;
		}


		void IClientInstance.Initialize( string serverInfo )
		{
			map     =   Content.Load<Map>( @"maps\" + serverInfo );
			map.ActivateMap( this );

			hudLayer	=	new SpriteLayer( Game.RenderSystem, 1024 );
			Game.RenderSystem.SpriteLayers.Add( hudLayer );

			ReplicaSpawned += ( s, e ) => {
					if ( e.Entity.UserGuid==clientGuid) {
					UserCommand.Yaw			=	0;
					UserCommand.Pitch		=	0;
					UserCommand.Roll		=	0;
					UserCommand.CtrlFlags	=	UserCtrlFlags.None;
				}
			};


			var rw = Game.RenderSystem.RenderWorld;

			rw.VirtualTexture = Content.Load<VirtualTexture>( "*megatexture" );

			rw.HdrSettings.BloomAmount  = 0.1f;
			rw.HdrSettings.DirtAmount   = 0.0f;
			rw.HdrSettings.KeyValue     = 0.18f;

			rw.SkySettings.SunPosition			= new Vector3( 1.0f, 0.8f, 1.3f );
			rw.SkySettings.SunLightIntensity	= 100;
			rw.SkySettings.SkyTurbidity			= 8;
			rw.SkySettings.SkyIntensity			= 0.5f;

			rw.LightSet.DirectLight.Direction	=	rw.SkySettings.SunLightDirection;
			rw.LightSet.DirectLight.Intensity	=	rw.SkySettings.SunLightColor;

			rw.LightSet.AmbientLevel	=	rw.SkySettings.AmbientLevel;
			rw.LightSet.SpotAtlas		=	Content.Load<TextureAtlas>(@"spots\spots");

			rw.FogSettings.Density		=	0.001f;

			/*for (int i=0; i<1; i++) {
				var spot = new SpotLight();
				spot.SpotView       =   Matrix.LookAtRH( new Vector3( 8, 10, 7 ), Vector3.Zero, Vector3.Up );
				spot.Intensity      =   new Color4( 500, 500, 500, 1 );
				spot.Projection     =   Matrix.PerspectiveRH( 0.1f, 0.1f, 0.1f, 100 );
				spot.RadiusOuter    =   100;
				spot.TextureIndex   =   0;
				rw.LightSet.SpotLights.Add( spot );
			} */
			Random rand = new Random();

			rw.LightSet.EnvLights.Add( new EnvLight( new Vector3(0,250,0), 1, 500 ) );
			for (float x=-32; x<=32; x+=16 ) {
				for (float y=-32; y<=32; y+=16 ) {
					rw.LightSet.EnvLights.Add( new EnvLight( new Vector3(x,4,y), 1, 16 ) );
				}
			} //*/

			for (float x=-32; x<=32; x+=8 ) {
				for (float y=-32; y<=32; y+=8 ) {
					//rw.LightSet.OmniLights.Add( new OmniLight( new Vector3(x,4,y), new Color4(50,50,50,1), 16 ) );
				}
			}

			Log.Message("Capturing radiance...");
			rw.RenderRadiance();

			(Game.UserInterface as ShooterInterface).ShowMenu = false;

			Log.Message("---- Loading game completed ----");
			Log.Message("");
		}


		public UserCommand	UserCommand;

		byte[] latestSnapshot = null;

		public float entityLerpFactor {
			get;
			private set;
		}



		public byte[] Update( GameTime gameTime, uint sentCommandID )
		{
			// update user input :
			gameInput.Update( gameTime, ref UserCommand );
			var cmdBytes = UserCommand.GetBytes( UserCommand );

			//	process incoming snapshots :
			ProcessSnapshot(gameTime);

			//	movement prediction :
			//gameWorld.PlayerCommand( this.Guid, cmdBytes, 0 );
			//gameWorld.SimulateWorld( gameTime.ElapsedSec );

			//	render world :
			PresentWorld( gameTime.ElapsedSec, entityLerpFactor );

			return cmdBytes;
		}

		public void FeedSnapshot( GameTime serverTime, byte[] snapshot, uint ackCommandID )
		{
			this.serverTime		=	serverTime;
			this.latestSnapshot	=	snapshot;
		}

		public void FeedNotification( string message )
		{
			Log.Message( "NOTIFICATION : {0}", message );
		}

		public string UserInfo()
		{
			return "Bob" + System.Diagnostics.Process.GetCurrentProcess().Id.ToString();
		}



		GameTime serverTime;

		Core.Filter filter = new Core.Filter(8);


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		bool ProcessSnapshot ( GameTime gameTime )
		{
			//	calc lerp factor :
			float lerpInc	=	1;
			
			if (serverTime.ElapsedSec!=0) {
				filter.Push(gameTime.ElapsedSec / serverTime.ElapsedSec);
				lerpInc  = filter.Value;
			}

			entityLerpFactor += lerpInc;

			// snapshot has arrived :
			if (latestSnapshot!=null) {

				//bool late = false;
				bool early = false;

				//	check whether chanpshot is too early or late :
				if ( !MathUtil.WithinEpsilon( entityLerpFactor, 1, lerpInc/2.0f ) ) {
					if (entityLerpFactor>1) {
						//late = true;
					}
					if (entityLerpFactor<1) {
						early = true;
					}
				}

				//	snapshot too early, wait next frame.
				if (early) {
					return false;
				}

				//	read snapshot :
				using ( var ms = new MemoryStream(latestSnapshot) ) {
					ReadFromSnapshot( ms, entityLerpFactor );
					entityLerpFactor = 0;
				}

				latestSnapshot	=	null;

				return true;

			} else {
				return false;
			}
		}


	}
}
