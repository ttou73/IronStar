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

namespace IronStar.Core {

	/// <summary>
	/// World represents entire game state.
	/// </summary>
	public partial class GameWorld {

		readonly string mapName;

		Map map;

		public readonly Guid UserGuid;

		public AtomCollection Atoms { 
			get {																		   
				if (serverSide) {
					return GameServer.Atoms;
				} else {
					return GameClient.Atoms;
				}
			}
		}

		SnapshotWriter snapshotWriter = new SnapshotWriter();
		SnapshotReader snapshotReader = new SnapshotReader();

		public readonly Game Game;
		public readonly ContentManager Content;
		readonly bool serverSide;

		public delegate void EntityEventHandler ( object sender, EntityEventArgs e );

		Dictionary<string,Type> entityControllerTypes;
		List<uint> entityToKill = new List<uint>();

		public EntityCollection entities;
		uint idCounter = 1;

		public event EntityEventHandler ReplicaSpawned;
		public event EntityEventHandler ReplicaKilled;
		public event EntityEventHandler EntitySpawned;
		public event EntityEventHandler EntityKilled;

		/// <summary>
		/// Gets list of world views.
		/// </summary>
		readonly List<WorldView> views = new List<WorldView>();


		List<FXEvent> fxEvents = new List<FXEvent>();

		SFX.FXPlayback		fxPlayback;
		SFX.ModelManager	modelManager;


		/// <summary>
		/// Indicates that world is running on server side.
		/// </summary>
		public bool IsServerSide {
			get { return serverSide; }
		}



		/// <summary>
		/// Indicates that world is running on client side.
		/// </summary>
		public bool IsClientSide {
			get { return !serverSide; }
		}


		/// <summary>
		/// Gets server
		/// </summary>
		public GameServer GameServer {
			get { 
				if (!IsServerSide) {
					throw new InvalidOperationException("World is not server-side");
				}
				return Game.GameServer;
			}
		}


		/// <summary>
		/// Gets server
		/// </summary>
		public GameClient GameClient {
			get { 
				if (!IsClientSide) {
					throw new InvalidOperationException("World is nor client-side");
				}
				return Game.GameClient;
			}
		}


		/// <summary>
		/// Initializes server-side world.
		/// </summary>
		/// <param name="maxPlayers"></param>
		/// <param name="maxEntities"></param>
		public GameWorld ( GameServer server, string mapName )
		{
			Log.Verbose( "world: server" );
			this.serverSide =   true;
			this.Game       =   server.Game;
			this.UserGuid   =   new Guid();
			Content         =   server.Content;
			entities        =   new EntityCollection(server.Atoms);

			entityControllerTypes	=	Misc.GetAllSubclassesOf( typeof(EntityController) )
										.ToDictionary( type => type.Name );

			AddAtoms();

			//------------------------

			this.mapName	=	mapName;

			map     =   Content.Load<Map>( @"maps\" + mapName );

			map.ActivateMap( this );


			#region TEMP STUFF
			Random	r = new Random();
			for (int i=0; i<10; i++) {
				Spawn("box", 0, Vector3.Up * 400 + r.GaussRadialDistribution(20,2), 0 );
			}// */
			#endregion


			EntityKilled += MPWorld_EntityKilled;
		}



		/// <summary>
		/// 
		/// </summary>
		void AddAtoms ()
		{
			var atoms = new List<string>();

			atoms.AddRange( Content.EnumerateAssets( "fx" ) );
			atoms.AddRange( Content.EnumerateAssets( "entities" ) );
			atoms.AddRange( Content.EnumerateAssets( "models" ) );

			Atoms.AddRange( atoms );
		}



		/// <summary>
		/// Initializes client-side world.
		/// </summary>
		/// <param name="client"></param>
		public GameWorld ( GameClient client, string serverInfo )
		{
			Log.Verbose("world: client");
			this.serverSide	=	false;
			this.Game		=	client.Game;
			this.UserGuid	=	client.Guid;
			Content			=	client.Content;
			entities		=	new EntityCollection(null);
			fxPlayback		=	new SFX.FXPlayback((ShooterClient)client, this);
			modelManager	=	new ModelManager((ShooterClient)client, this);

			AddView( new Hud( this ) );
			AddView( new GameCamera( this ) );

			//------------------------

			map     =   Content.Load<Map>( @"maps\" + serverInfo );

			Game.Reloading += (s,e) => ForEachEntity( ent => ent.MakeRenderStateDirty() );
		}



		/// <summary>
		/// This method called in main thread to complete non-thread safe operations.
		/// </summary>
		public void FinalizeLoad()
		{
			map.ActivateMap( this );
			//foreach ( var mi in map.MeshInstance ) {
			//	Game.RenderSystem.RenderWorld.Instances.Add( mi );
			//}
		}



		/// <summary>
		/// 
		/// </summary>
		public void Shutdown()
		{
			if ( IsClientSide ) {
				fxPlayback?.Shutdown();
				modelManager?.Shutdown();
			}

			if ( IsClientSide ) {
				Game.RenderSystem.RenderWorld.ClearWorld();
			}
		}



		/// <summary>
		/// Returns server info.
		/// </summary>
		public string ServerInfo()
		{
			return mapName;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="frmt"></param>
		/// <param name="args"></param>
		protected void LogTrace ( string frmt, params object[] args )
		{
			var s = string.Format( frmt, args );

			if (IsClientSide) Log.Verbose("cl: " + s );
			if (IsServerSide) Log.Verbose("sv: " + s );
		}



		/// <summary>
		/// Adds view.
		/// </summary>
		/// <param name="view"></param>
		public void AddView( WorldView view )
		{
			if (IsServerSide) {
				return;
				//throw new InvalidOperationException("Can not add EntityView to server-side world");
			} 
			views.Add( view );
		}



		/// <summary>
		/// Gets view by its type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T GetView<T>() where T: WorldView
		{
			return (T)views.FirstOrDefault( v => v is T );
		}



		/// <summary>
		/// Simulates world.
		/// </summary>
		/// <param name="gameTime"></param>
		public virtual void SimulateWorld ( float elapsedTime )
		{
			if (IsServerSide) {

				UpdatePlayers( elapsedTime );

				var dt	=	1 / GameServer.TargetFrameRate;
				PhysSpace.TimeStepSettings.MaximumTimeStepsPerFrame = 6;
				PhysSpace.TimeStepSettings.TimeStepDuration = 1.0f/60.0f;
				PhysSpace.Update(dt);
			}


			fxSpawnSequence++;

			//
			//	Control entities :
			//
			ForEachEntity( e => e.ForeachController( c => c.Update( elapsedTime ) ) );

			//
			//	Kill entities :
			//
			CommitKilledEntities();
		}


		byte fxSpawnSequence	=	0;

		List<Vector3> clPos = new List<Vector3>();
		List<Vector3> visPos = new List<Vector3>();


		/// <summary>
		/// Updates visual and audial stuff
		/// </summary>
		/// <param name="gameTime"></param>
		public void PresentWorld ( float deltaTime, float lerpFactor )
		{
			var dr = Game.RenderSystem.RenderWorld.Debug;

			if (!IsClientSide) {
				throw new InvalidOperationException("PresentWorld could be called only on client side");
			}


			var visibleEntities = entities.Select( pair => pair.Value ).ToArray();

			//
			//	draw all entities :
			//
			foreach ( var entity in visibleEntities ) {
				entity.UpdateRenderState( fxPlayback, modelManager );
			}

			//
			//	update view :
			//
			foreach ( var view in views ) {
				view.Update( deltaTime, lerpFactor );
			}

			//
			//	updare effects :
			//	
			fxPlayback.Update( deltaTime, lerpFactor );
			modelManager.Update( deltaTime, lerpFactor );
		}


		/*-----------------------------------------------------------------------------------------
		 *	Entity creation
		-----------------------------------------------------------------------------------------*/


		public Entity Spawn( EntityFactory factory, short classID, uint parentId, Vector3 origin, Quaternion orient )
		{
			//	due to server reconciliation
			//	never create entities on client-side:
			if ( IsClientSide ) {
				return null;
			}

			//	get ID :
			uint id = idCounter;

			idCounter++;

			if ( idCounter==0 ) {
				//	this actually will never happen, about 103 day of intense playing.
				throw new InvalidOperationException( "Too much entities were spawned" );
			}

			//
			//	Create instance.
			//	If creation failed later, entity become dummy.
			//
			var entity = new Entity(id, classID, parentId, origin, orient);
			entities.Add( id, entity );

			//LogTrace( "spawn: {0} - #{1}", factory?.GetType(), id );

			entity.Controller = factory?.Spawn( entity, this );

			EntitySpawned?.Invoke( this, new EntityEventArgs( entity ) );

			return entity;
		}


		/// <summary>
		/// When called on client-side returns null.
		/// </summary>
		/// <param name="prefab"></param>
		/// <param name="parent"></param>
		/// <param name="origin"></param>
		/// <param name="angles"></param>
		/// <returns></returns>
		public Entity Spawn ( string classname, uint parentId, Vector3 origin, Quaternion orient )
		{
			//	due to server reconciliation
			//	never create entities on client-side:
			if (IsClientSide) {
				return null;
			}

			//	get ID :
			uint id = idCounter;

			idCounter++;

			if (idCounter==0) {
				//	this actually will never happen, about 103 day of intense playing.
				throw new InvalidOperationException("Too much entities were spawned");
			}

			//
			//	Create instance.
			//	If creation failed later, entity become dummy.
			//
			var classID	=	Atoms[classname];
			var factory	=	Content.Load<EntityFactory>(@"entities\" + classname, (EntityFactory)null );

			return Spawn( factory, classID, parentId, origin, orient );
		}


		/// <summary>
		/// Spawns entity with specified classname, parent ID and matrix.
		/// </summary>
		/// <param name="prefab"></param>
		/// <param name="parentId"></param>
		/// <param name="transform"></param>
		/// <returns></returns>
		public Entity Spawn( string classname, uint parentId, Matrix transform )
		{
			var p	=	transform.TranslationVector;
			var q	=	Quaternion.RotationMatrix( transform );

			return Spawn( classname, parentId, p, q );
		}



		/// <summary>
		/// Spawns entity with specified classname, parent ID and yaw angle.
		/// </summary>
		/// <param name="prefab"></param>
		/// <param name="parentId"></param>
		/// <param name="origin"></param>
		/// <param name="yaw"></param>
		/// <returns></returns>
		public Entity Spawn( string classname, uint parentId, Vector3 origin, float yaw )
		{
			return Spawn( classname, parentId, origin, Quaternion.RotationYawPitchRoll( yaw,0,0 ) );
		}


		
		/*-----------------------------------------------------------------------------------------
		 *	FX creation
		-----------------------------------------------------------------------------------------*/

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fxType"></param>
		/// <param name="position"></param>
		/// <param name="target"></param>
		/// <param name="orient"></param>
		public void SpawnFX ( string fxName, uint parentID, Vector3 origin, Vector3 velocity, Quaternion rotation )
		{
			LogTrace("fx : {0}", fxName);
			var fxID = GameServer.Atoms[ fxName ];

			if (fxID<0) {
				Log.Warning("SpawnFX: bad atom {0}", fxName);
			}

			fxEvents.Add( new FXEvent(fxID, parentID, origin, velocity, rotation ) );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="fxType"></param>
		/// <param name="position"></param>
		/// <param name="target"></param>
		/// <param name="orient"></param>
		public void SpawnFX ( string fxName, uint parentID, Vector3 origin, Vector3 velocity, Vector3 forward )
		{
			forward	=	Vector3.Normalize( forward );
			var rt	=	Vector3.Cross( forward, Vector3.Up );	

			if (rt.LengthSquared()<0.001f) {
				rt	=	Vector3.Cross( forward, Vector3.Right );
			}

			var up	=	Vector3.Cross( rt, forward );

			var m	=	Matrix.Identity;
			m.Forward	=	forward;
			m.Right		=	rt;
			m.Up		=	up;
			
			SpawnFX( fxName, parentID, origin, velocity, Quaternion.RotationMatrix(m) );
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="fxName"></param>
		/// <param name="parentID"></param>
		/// <param name="origin"></param>
		/// <param name="forward"></param>
		public void SpawnFX ( string fxName, uint parentID, Vector3 origin, Vector3 forward )
		{
			SpawnFX( fxName, parentID, origin, Vector3.Zero, forward );
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="fxType"></param>
		/// <param name="position"></param>
		/// <param name="target"></param>
		/// <param name="orient"></param>
		public void SpawnFX ( string fxName, uint parentID, Vector3 origin )
		{
			SpawnFX( fxName, parentID, origin, Vector3.Zero, Quaternion.Identity );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="damage"></param>
		/// <param name="kickImpulse"></param>
		/// <param name="kickPoint"></param>
		/// <param name="damageType"></param>
		public void InflictDamage ( Entity entity, uint attackerID, short damage, Vector3 kickImpulse, Vector3 kickPoint, DamageType damageType )
		{
			entity?.Controller?.Damage( entity.ID, attackerID, damage, kickImpulse, kickPoint, damageType );
		}



		void CommitKilledEntities ()
		{
			foreach ( var id in entityToKill ) {
				KillImmediatly( id );
			}
			
			entityToKill.Clear();			
		}


		void KillImmediatly ( uint id )
		{
			if (id==0) {
				return;
			}

			Entity ent;

			if ( entities.TryGetValue(id, out ent)) {

				if (IsClientSide) {
					ent.DestroyRenderState(fxPlayback);
					ReplicaKilled?.Invoke( this, new EntityEventArgs(ent) );
				}

				if (IsServerSide) {
					EntityKilled?.Invoke( this, new EntityEventArgs(ent) );
				}
				
				entities.Remove( id );
				ent?.Controller?.Killed();

			} else {
				Log.Warning("Entity #{0} does not exist", id);
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		public void Kill ( uint id )
		{
			LogTrace("kill: #{0}", id );
			entityToKill.Add( id );
		}


		/// <summary>
		/// Writes world state to stream writer.
		/// </summary>
		/// <param name="writer"></param>
		public virtual void WriteToSnapshot ( Stream stream )
		{
			snapshotWriter.Write( stream, entities, fxEvents );
		}



		/// <summary>
		/// Reads world state from stream reader.
		/// </summary>
		/// <param name="writer"></param>
		public virtual void ReadFromSnapshot ( Stream stream, float lerpFactor )
		{
			snapshotReader.Read( stream, entities, fxe=>fxPlayback.RunFX(fxe), null, id=>KillImmediatly(id) );
		}



		/// <summary>
		/// Prints entire world state to console.
		/// </summary>
		public void PrintState ()
		{		
			var ents = entities.Select( pair => pair.Value ).OrderBy( e => e.ID ).ToArray();

			Log.Message("");
			Log.Message("---- {0} World state ---- ", IsServerSide ? "Server side" : "Client side" );

			foreach ( var ent in ents ) {
				
				var id			=	ent.ID;
				var parent		=	ent.ParentID;
				var prefab		=	Atoms[ent.ClassID];
				var guid		=	ent.UserGuid;
				var controller	=	ent.Controller.GetType().Name;

				Log.Message("{0:X8} {1:X8} {2} {3,-32} {4,-32}", id, parent, guid, prefab, controller );
			}

			Log.Message("----------------" );
			Log.Message("");
		}
	}
}
