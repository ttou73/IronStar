using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEPUphysics.BroadPhaseEntries;
using Fusion.Core.Mathematics;
using Fusion.Engine.Graphics;
using IronStar.Core;
using IronStar.SFX;

namespace IronStar.Entities {

	public class StaticModelFactory : EntityFactory {

		/// <summary>
		/// 
		/// </summary>
		/// 
		[Category("Static Model")]
		public string Model { get; set; }


		/// <summary>
		/// 
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="world"></param>
		/// <returns></returns>
		public override EntityController Spawn( Entity entity, GameWorld world )
		{
			return new StaticModel( entity, world, this );
		}

	}
}
