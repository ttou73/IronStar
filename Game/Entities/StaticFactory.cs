using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronStar.Core;

namespace IronStar.Entities {
	public class StaticFactory : EntityFactory {

		
		/// <summary>
		/// 
		/// </summary>
		public bool Visible { get; set; } = true;


		/// <summary>
		/// 
		/// </summary>
		public bool Collidable { get; set; } = true;


		/// <summary>
		/// 
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="world"></param>
		/// <returns></returns>
		public override EntityController Spawn( Entity entity, GameWorld world )
		{
			throw new InvalidOperationException("StaticFactory do not spawn entities!");
		}

	}
}
