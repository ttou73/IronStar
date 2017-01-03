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

	/// <summary>
	/// World represents entire game state.
	/// </summary>
	public partial class GameWorld {


		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool IsPlayer( uint id )
		{
			if ( GameClient==null ) {
				return false;
			}

			Entity e;

			if ( entities.TryGetValue( id, out e ) ) {
				return e.UserGuid == GameClient.Guid;
			} else {
				return false;
			}
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
		/// If entity does not exist returns null
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


		public IEnumerable<Entity> GetEntities ()
		{
			return entities.Select( pair => pair.Value ).OrderBy( e => e.ID );
		}



		/// <summary>
		/// Gets entity with current id.
		/// If entity is dead -> exception...
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[Obsolete]
		public Entity GetEntityOrNull( string classname, Func<Entity, bool> predicate )
		{
			return GetEntities( classname ).FirstOrDefault( ent => predicate( ent ) );
		}


		/// <summary>
		/// Gets entity with current id.
		/// If entity is dead -> exception...
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[Obsolete]
		public Entity GetEntityOrNull( string classname )
		{
			return GetEntities( classname ).FirstOrDefault();
		}




		/// <summary>
		/// Gets entity with current id.
		/// If entity is dead -> exception...
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[Obsolete]
		public IEnumerable<Entity> GetEntities ( string classname )
		{
			var classId = Atoms[classname];
			return entities.Where( pair => pair.Value.ClassID==classId ).Select( pair1 => pair1.Value );
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
	}
}
