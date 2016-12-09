using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Engine.Common;
using Fusion.Core.Mathematics;


namespace IronStar.Core {
	public class EntityCollection : Dictionary<uint,Entity> {

		readonly AtomCollection atoms;


		/// <summary>
		/// Creates instance of entity collection.
		/// </summary>
		/// <param name="atoms"></param>
		public EntityCollection ( AtomCollection atoms )
		{
			this.atoms	=	atoms;
		}


		/// <summary>
		/// Gets entity with given ID or null if entity does not exist.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		new public Entity this[uint id] {
			get {
				Entity e;
				if ( TryGetValue( id, out e ) ) {
					return e;
				} else {
					return null;
				}
			}
		}


		/// <summary>
		/// Gets entity with given ID or null if entity does not exist.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Entity GetEntity( uint id )
		{
			Entity e;
			if ( TryGetValue( id, out e ) ) {
				return e;
			} else {
				return null;
			}
		}



		/// <summary>
		/// Gets entity of given classname that meets given predicate.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Entity GetEntityOrNull( string classname, Func<Entity, bool> predicate )
		{
			return GetEntities( classname ).FirstOrDefault( ent => predicate( ent ) );
		}


		/// <summary>
		/// Gets first entity of given class or null.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Entity GetEntityOrNull( string classname )
		{
			return GetEntities( classname ).FirstOrDefault();
		}




		/// <summary>
		/// Gets all entities of given class.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IEnumerable<Entity> GetEntities( string classname )
		{
			var classId = atoms[classname];
			return this.Where( pair => pair.Value.ClassID==classId ).Select( pair1 => pair1.Value );
		}


	}
}
