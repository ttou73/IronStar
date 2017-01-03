using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion;
using IronStar.Core;
using IronStar.Editors;

namespace IronStar.Entities {
	public class ProxyFactory : EntityFactory {

		
		[TypeConverter(typeof(EntityListConverter))]
		public string Classname { get; set; } = "";


		/// <summary>
		/// 
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="world"></param>
		/// <returns></returns>
		public override EntityController Spawn( Entity entity, GameWorld world )
		{
			if (string.IsNullOrWhiteSpace(Classname)) {
				Log.Warning("ProxyFactory: classname is null or white space, null-entity spawned");
				return null;
			}

			var factory = world.Content.Load<EntityFactory>(@"entities\" + Classname);

			return factory.Spawn( entity, world );
		}

	}
}
