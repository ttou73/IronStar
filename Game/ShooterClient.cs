using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Fusion;
using Fusion.Core;
using Fusion.Core.Mathematics;
using Fusion.Core.Extensions;
using Fusion.Core.Configuration;
using Fusion.Engine.Common;
using Fusion.Engine.Input;
using Fusion.Engine.Client;
using Fusion.Engine.Server;
using Fusion.Engine.Graphics;
using IronStar.Core;
using IronStar.Views;
using IronStar.Client;


namespace IronStar {
	public partial class ShooterClient : Fusion.Engine.Client.GameClient {

		GameWorld gameWorld;
		GameInput gameInput;

		public GameWorld World {
			get { return gameWorld; }
		}



		SpriteLayer	hudLayer;

		public SpriteLayer HudLayer {
			get { return hudLayer; }
		}


		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="engine"></param>
		public ShooterClient ( Game game )
			: base( game )
		{
			SetDefaults();
			
			gameInput	=	new GameInput(this);
		}



		/// <summary>
		/// Initializes game
		/// </summary>
		public override void Initialize ()
		{
			hudLayer	=	new SpriteLayer( Game.RenderSystem, 1024 );
			Game.RenderSystem.SpriteLayers.Add( hudLayer );

			InitializeComponent( gameInput );

			Game.RenderSystem.DisplayBoundsChanged += RenderSystem_DisplayBoundsChanged;
		}



		void RenderSystem_DisplayBoundsChanged ( object sender, EventArgs e )
		{
			var	vp = Game.RenderSystem.DisplayBounds;
			Game.RenderSystem.RenderWorld.Resize( vp.Width, vp.Height );
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose ( bool disposing )
		{
			if (disposing) {
				DisposeComponents();
				SafeDispose( ref hudLayer );
			}
			base.Dispose( disposing );
		}



		/// <summary>
		/// Called when connection request accepted by server.
		/// Client could start loading models, textures, models etc.
		/// </summary>
		/// <param name="map"></param>
		public override GameLoader LoadContent ( string serverInfo )
		{
			latestSnapshot	=	null;

			Log.Message("");
			Log.Message("---- Loading game: {0} ----", serverInfo );

			return new ShooterLoader( this, serverInfo );
		}



		/// <summary>
		/// Called when loader finished loading.
		/// This method lets client to complete loading process in main thread.
		/// </summary>
		/// <param name="loader"></param>
		public override void FinalizeLoad ( GameLoader loader )
		{
			var shooterLoader = (ShooterLoader)loader;

			gameWorld = shooterLoader.World;

			gameWorld.FinalizeLoad();

			gameWorld.ReplicaSpawned += ( s, e ) => {
					if ( e.Entity.UserGuid==Guid) {
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



		/// <summary>
		///	Called when client disconnected, dropped, kicked or timeouted.
		///	Client must purge all level-associated content.
		///	Reason???
		/// </summary>
		public override void UnloadContent ()
		{
			latestSnapshot	=	null;

			World?.Shutdown();

			Game.RenderSystem.RenderWorld.ClearWorld();

			Content.Unload();
			(Game.UserInterface as ShooterInterface).ShowMenu = true;
		}



		public UserCommand	UserCommand;

		byte[] latestSnapshot = null;

		public float entityLerpFactor {
			get;
			private set;
		}


		/// <summary>
		/// Runs one step of client-side simulation and renders world state.
		/// Do not close the stream.
		/// </summary>
		/// <param name="gameTime"></param>
		public override byte[] Update ( GameTime gameTime, uint sentCommandID )
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
			gameWorld.PresentWorld( gameTime.ElapsedSec, entityLerpFactor );

			return cmdBytes;
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
					gameWorld.ReadFromSnapshot( ms, entityLerpFactor );
					entityLerpFactor = 0;
				}

				latestSnapshot	=	null;

				return true;

			} else {
				return false;
			}
		}



		/// <summary>
		/// Feed server snapshot to client.
		/// Called when fresh snapshot arrived.
		/// </summary>
		/// <param name="snapshot"></param>
		public override void FeedSnapshot ( GameTime serverTime, byte[] snapshot, uint ackCommandID )
		{
			this.serverTime		=	serverTime;
			this.latestSnapshot	=	snapshot;
		}



		/// <summary>
		/// Feed server notification to client.
		/// </summary>
		/// <param name="snapshot"></param>
		public override void FeedNotification ( string message )
		{
			Log.Message( "NOTIFICATION : {0}", message );
		}



		/// <summary>
		/// Returns user informations.
		/// </summary>
		/// <returns></returns>
		public override string UserInfo ()
		{
			return "Bob" + System.Diagnostics.Process.GetCurrentProcess().Id.ToString();
		}



		/// <summary>
		/// 
		/// </summary>
		public void PrintState ()
		{
			
		}
	}
}
