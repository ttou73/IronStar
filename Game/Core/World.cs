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
using ShooterDemo.SFX;

namespace ShooterDemo.Core {

	/// <summary>
	/// World represents entire game state.
	/// </summary>
	public abstract class World {

		public readonly Guid UserGuid;

		public readonly Game Game;
		public readonly ContentManager Content;
		readonly bool serverSide;

		public delegate void EntityConstructor ( World world, Entity entity );
		public delegate void EntityEventHandler ( object sender, EntityEventArgs e );

		Dictionary<uint, Prefab> prefabs = new Dictionary<uint,Prefab>();
		List<uint> entityToKill = new List<uint>();

		public Dictionary<uint, Entity> entities;
		uint idCounter = 1;

		public event EntityEventHandler ReplicaSpawned;
		public event EntityEventHandler ReplicaKilled;
		public event EntityEventHandler EntitySpawned;
		public event EntityEventHandler EntityKilled;

		/// <summary>
		/// Gets list of world views.
		/// </summary>
		readonly List<WorldView> views = new List<WorldView>();

		/// <summary>
		/// Gets list of world controllers.
		/// </summary>
		readonly List<WorldController> controllers = new List<WorldController>();


		List<FXEvent> fxEvents = new List<FXEvent>();

		SFX.SfxSystem	sfxSystem;



		/// <summary>
		/// We just received snapshot.
		/// Need to update client-side controllers.
		/// </summary>
		public bool snapshotDirty = false;


		class Prefab {
			public string Name;
			public EntityConstructor Construct;
		}


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
		public World ( GameServer server )
		{
			Log.Verbose("world: server");
			this.serverSide	=	true;
			this.Game		=	server.Game;
			this.UserGuid	=	new Guid();
			Content			=	server.Content;
			entities		=	new Dictionary<uint,Entity>();
		}



		/// <summary>
		/// Initializes client-side world.
		/// </summary>
		/// <param name="client"></param>
		public World ( GameClient client )
		{
			Log.Verbose("world: client");
			this.serverSide	=	false;
			this.Game		=	client.Game;
			this.UserGuid	=	client.Guid;
			Content			=	client.Content;
			entities		=	new Dictionary<uint,Entity>();
			sfxSystem		=	new SFX.SfxSystem((ShooterClient)client, this);
		}



		public bool IsPlayer ( uint id )
		{
			if (GameClient==null) {
				return false;
			}

			Entity e;

			if (entities.TryGetValue(id, out e)) {
				return e.UserGuid == GameClient.Guid;
			} else {
				return false;
			}
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
		/// 
		/// </summary>
		/// <param name="prefabName"></param>
		/// <param name="constructAction"></param>
		public void AddPrefab ( string prefabName, EntityConstructor constructAction )
		{
			uint crc = Factory.GetPrefabID( prefabName );

			if (prefabs.ContainsKey(crc)) {
				throw new ArgumentException("Prefab '" + prefabName + "' cause hash collision with + '" + prefabs[crc].Name + "'");
			}

			prefabs.Add( crc, new Prefab(){ Name = prefabName, Construct = constructAction } );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="prefabId"></param>
		void ConstructEntity ( Entity entity )
		{
			prefabs[ entity.PrefabID ].Construct(this, entity);
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="entity"></param>
		void Destruct ( Entity entity )
		{
			if (IsServerSide) {
				entity.ForeachController( c => c.Killed() );
			}

			if (IsClientSide) {
				entity.ForeachViews( v => v.Killed() );	
			}
		}



		/// <summary>
		/// Simulates world.
		/// </summary>
		/// <param name="gameTime"></param>
		public virtual void SimulateWorld ( float deltaTime )
		{
			fxSpawnSequnce++;

			//
			//	Control entities :
			//
			ForEachEntity( e => e.ForeachController( c => c.Update( deltaTime ) ) );

			//
			//	Kill entities :
			//
			CommitKilledEntities();
		}


		byte fxSpawnSequnce	=	0;

		List<Vector3> clPos = new List<Vector3>();
		List<Vector3> visPos = new List<Vector3>();


		/// <summary>
		/// Updates visual and audial stuff
		/// </summary>
		/// <param name="gameTime"></param>
		public virtual void PresentWorld ( float deltaTime, float lerpFactor )
		{
			var dr = Game.RenderSystem.RenderWorld.Debug;

			ForEachEntity( e => e.ForeachViews( v => v.Update( deltaTime, lerpFactor ) ) );

			if (IsClientSide) {
				foreach ( var view in views ) {
					view.Update( deltaTime, lerpFactor );
				}

				sfxSystem.Update( deltaTime );
			}
		}

		
		


		/// <summary>
		/// When called on client-side returns null.
		/// </summary>
		/// <param name="prefab"></param>
		/// <param name="parent"></param>
		/// <param name="origin"></param>
		/// <param name="angles"></param>
		/// <returns></returns>
		public Entity Spawn ( string prefab, uint parentId, Vector3 origin, Quaternion orient )
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


			uint prefabId = Factory.GetPrefabID( prefab );

			var entity = new Entity(id, prefabId, parentId, origin, orient);

			entities.Add( id, entity );

			ConstructEntity( entity );

			LogTrace("spawn: {0} - #{1}", prefab, id );

			if (EntitySpawned!=null) {
				EntitySpawned( this, new EntityEventArgs(entity) );
			}

			return entity;
		}



		public Entity Spawn( string prefab, uint parentId, Matrix transform )
		{
			var p	=	transform.TranslationVector;
			var q	=	Quaternion.RotationMatrix( transform );

			return Spawn( prefab, parentId, p, q );
		}



		public Entity Spawn( string prefab, uint parentId, Vector3 origin, float yaw )
		{
			return Spawn( prefab, parentId, origin, Quaternion.RotationYawPitchRoll( yaw,0,0 ) );
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="fxType"></param>
		/// <param name="position"></param>
		/// <param name="target"></param>
		/// <param name="orient"></param>
		public void SpawnFX ( string fxName, uint parentID, Vector3 origin, Vector3 velocity, Quaternion rotation )
		{
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
		/// <param name="fxType"></param>
		public SfxInstance RunFX ( FXEvent fxEvent )
		{
			return sfxSystem.RunFX( fxEvent );
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
			if (entity==null) {
				return;
			}

			entity.ForeachController( c => c.Damage( entity.ID, attackerID, damage, kickImpulse, kickPoint, damageType ) );
		}




		/// <summary>
		/// Check whether entity with id is dead.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool IsAlive ( uint id )
		{
			return entities.ContainsKey( id );
		}



		/// <summary>
		/// Gets entity with current id.
		/// If entity is dead -> returns null
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Entity GetEntity ( uint id )
		{
			Entity e;
			if (entities.TryGetValue( id, out e )) {
				return e;
			} else {
				return null;
			}
		}



		/// <summary>
		/// Gets entity with current id.
		/// If entity is dead -> exception...
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Entity GetEntityOrNull ( Func<Entity,bool> predicate )
		{
			return entities.FirstOrDefault( pair => predicate( pair.Value ) ).Value;
		}




		/// <summary>
		/// Gets entity with current id.
		/// If entity is dead -> exception...
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IEnumerable<Entity> GetEntities ( string prefabName )
		{
			uint prefabId = Factory.GetPrefabID( prefabName );

			return entities.Where( pair => pair.Value.PrefabID == prefabId ).Select( pair1 => pair1.Value );
		}


		/// <summary>
		/// Performs action on each entity.
		/// </summary>
		/// <param name="action"></param>
		public void ForEachEntity ( Action<Entity> action )
		{
			var list = entities.Select( p => p.Value ).ToList();

			foreach ( var e in list ) {
				action( e );
			}
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

				if (IsClientSide && ReplicaKilled!=null) {
					ReplicaKilled( this, new EntityEventArgs(ent) );
				}
				
				if (IsServerSide && EntityKilled!=null) {
					EntityKilled( this, new EntityEventArgs(ent) );
				}
				
				entities.Remove( id );
				Destruct( ent );

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
		/// 
		/// </summary>
		/// <param name="guid"></param>
		/// <param name="userInfo"></param>
		/// <returns></returns>
		public abstract bool ApprovePlayer ( Guid guid, string userInfo );

		/// <summary>
		/// Called when player connected.
		/// </summary>
		/// <param name="guid"></param>
		/// <param name="userInfo"></param>
		public abstract void PlayerConnected ( Guid guid, string userInfo );

		/// <summary>
		/// Called when player enetered.
		/// </summary>
		/// <param name="guid"></param>
		public abstract void PlayerEntered ( Guid guid );

		/// <summary>
		/// Called when player left.
		/// </summary>
		/// <param name="guid"></param>
		public abstract void PlayerLeft ( Guid guid );

		/// <summary>
		/// Called when player disconnected.
		/// </summary>
		/// <param name="guid"></param>
		public abstract void PlayerDisconnected ( Guid guid );

		/// <summary>
		/// Called when player left.
		/// </summary>
		/// <param name="guid"></param>
		public abstract void PlayerCommand ( Guid guid, byte[] command, float lag );

		/// <summary>
		/// This method called in main thread to complete non-thread safe operations.
		/// </summary>
		public abstract void FinalizeLoad (); 


		/// <summary>
		/// Returns server info.
		/// </summary>
		public abstract string ServerInfo (); 


		/// <summary>
		/// Called when client or server is 
		/// </summary>
		public virtual void Cleanup ()
		{
			if (IsClientSide) {
				sfxSystem.StopAllSFX();
			}
		}


		/// <summary>
		/// 
		/// </summary>
		public void PrintState ()
		{		
			var ents = entities.Select( pair => pair.Value ).OrderBy( e => e.ID ).ToArray();

			Log.Message("");
			Log.Message("---- {0} World state ---- ", IsServerSide ? "Server side" : "Client side" );

			foreach ( var ent in ents ) {
				
				var id		=	ent.ID;
				var parent	=	ent.ParentID;
				var prefab	=	prefabs[ ent.PrefabID ].Name;
				var guid	=	ent.UserGuid;

				Log.Message("{0:X8} {1:X8} {2} {3,-32}", id, parent, guid, prefab );
			}

			Log.Message("----------------" );
			Log.Message("");
		}


		int sendSnapshotCounter = 1;

		/// <summary>
		/// Writes world state to stream writer.
		/// </summary>
		/// <param name="writer"></param>
		public virtual void WriteToSnapshot ( BinaryWriter writer )
		{
			var entArray = entities.OrderBy( pair => pair.Key ).ToArray();

			writer.Write( sendSnapshotCounter );
			sendSnapshotCounter++;

			//
			//	Write fat entities :
			//
			writer.WriteFourCC("ENT0");
			writer.Write( entArray.Length );

			foreach ( var ent in entArray ) {
				writer.Write( ent.Key );
				ent.Value.Write( writer );
			}


			//
			//	Write FX events :
			//
			writer.WriteFourCC("FXE0");
			writer.Write( fxEvents.Count );
			
			foreach ( var fxe in fxEvents ) {
				fxe.SendCount ++;
				fxe.Write( writer );
			}

			fxEvents.RemoveAll( fx => fx.SendCount >= 3 );
		}



		int recvSnapshotCounter = 0;

		/// <summary>
		/// Reads world state from stream reader.
		/// </summary>
		/// <param name="writer"></param>
		public virtual void ReadFromSnapshot ( BinaryReader reader, uint ackCmdID, float lerpFactor )
		{
			int snapshotCounter			=	reader.ReadInt32();
			int snapshotCountrerDelta	=	snapshotCounter - recvSnapshotCounter;
			recvSnapshotCounter			=	snapshotCounter;

			reader.ExpectFourCC("ENT0", "Bad snapshot");

			int length	=	reader.ReadInt32();
			var oldIDs	=	entities.Select( pair => pair.Key ).ToArray();
			var newIDs	=	new uint[length];

			for ( int i=0; i<length; i++ ) {

				uint id		=	reader.ReadUInt32();
				newIDs[i]	=	id;

				if ( entities.ContainsKey(id) ) {

					//	Entity with given ID exists.
					//	Just update internal state.
					entities[id].Read( reader, lerpFactor );

				} else {
					
					//	Entity does not exist.
					//	Create new one.
					var ent = new Entity(id);

					ent.Read( reader, lerpFactor );
					entities.Add( id, ent );

					ConstructEntity( ent );

					if (ReplicaSpawned!=null) {
						ReplicaSpawned( this, new EntityEventArgs(ent) );
					}
				}
			}

			//	Kill all stale entities :
			var staleIDs = oldIDs.Except( newIDs );

			foreach ( var id in staleIDs ) {
				KillImmediatly( id );
			}


			reader.ExpectFourCC("FXE0", "Bad snapshot");

			int count = reader.ReadInt32();

			for (int i=0; i<count; i++) {
				var fxe = new FXEvent();
				fxe.Read( reader );

				if (fxe.SendCount<=snapshotCountrerDelta) {
					RunFX( fxe );
				}
			}
		}
	}
}
