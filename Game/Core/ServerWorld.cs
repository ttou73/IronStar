using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Engine.Common;
using Fusion.Core.Mathematics;
using Fusion.Engine.Server;
using System.IO;
using IronStar.SFX;

namespace IronStar.Core {
	class ServerWorld {

		Dictionary<uint, Entity> entities = new Dictionary<uint, Entity>();
		List<FXEvent> fxEvents = new List<FXEvent>();
		uint idCounter	=	1;


		/// <summary>
		/// 
		/// </summary>
		public ServerWorld ( GameServer server, string map )
		{
			throw new NotImplementedException();
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update ( GameTime gameTime )
		{
			throw new NotImplementedException();
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="className"></param>
		/// <param name="parameters"></param>
		public Entity SpawnEntity ( string className, uint parentId, Vector3 origin, Quaternion rotation, Dictionary<string,string> parameters )
		{
			//	get ID :
			uint id = idCounter;

			idCounter++;

			if (idCounter==0) {
				//	this actually will never happen, about 103 day of intense playing.
				throw new InvalidOperationException("Too much entities were spawned");
			}


			var entity = new Entity(id, prefabId, parentId, origin, rotation);

			entities.Add( id, entity );

			LogTrace("spawn: {0} - #{1}", prefab, id );

			return entity;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sfxName"></param>
		/// <param name="parentID"></param>
		/// <param name="position"></param>
		/// <param name="velocity"></param>
		/// <param name="rotation"></param>
		public void SpawnSFX ( string sfxName, uint parentID, Vector3 position, Vector3 velocity, Quaternion rotation )
		{
			throw new NotImplementedException();
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="frmt"></param>
		/// <param name="args"></param>
		protected void LogTrace ( string frmt, params object[] args )
		{
			Log.Verbose( "sv: " + frmt, args );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		public void WriteSnapshot ( BinaryWriter writer )
		{
			
		}



		/// <summary>
		/// Saves game
		/// </summary>
		/// <param name="stream"></param>
		public void SaveGame ( Stream stream )
		{
		}


		/// <summary>
		/// Loads game
		/// </summary>
		/// <param name="stream"></param>
		public void LoadGame ( Stream stream )
		{
		}
	}
}
