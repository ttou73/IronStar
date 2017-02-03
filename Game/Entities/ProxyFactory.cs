using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion;
using Fusion.Core.Mathematics;
using Fusion.Engine.Graphics;
using IronStar.Core;
using IronStar.Editors;

namespace IronStar.Entities {
	public class ProxyFactory : EntityFactory {

		
		[TypeConverter(typeof(EntityListConverter))]
		public string Classname { 
			get { return classname; }
			set {
				if (classname!=value) {
					classname = value;
					dirty = true;
				}
			}
		}


		string classname = "";
		bool dirty = true;
		EntityFactory factory = null;


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

			factory = world.Content.Load<EntityFactory>(@"entities\" + Classname);

			return factory.Spawn( entity, world );
		}



		public override void Draw( DebugRender dr, Matrix transform, Color color )
		{
			if (factory==null) {
				base.Draw( dr, transform, color );
			} else {
				factory.Draw( dr, transform, color );
			}
		}


	}
}
