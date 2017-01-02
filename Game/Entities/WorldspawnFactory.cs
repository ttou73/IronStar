using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Core.Mathematics;
using IronStar.Core;
using Fusion.Engine.Graphics;

namespace IronStar.Entities {
	public class WorldspawnFactory : EntityFactory {

		[Category( "Physics" )]
		public float Gravity { get; set; } = 16;

		[Category( "Sky" )]
		public float Turbidity { get; set; } = 2;

		[Category( "Sky" )]
		public Vector3 SunDirection { get; set; } = Vector3.One;

		[Category( "Sky" )]
		public float SunIntensity { get; set; } = 6000;

		[Category( "Sky" )]
		public float SkyIntensity { get; set; } = 1;



		/// <summary>
		/// 
		/// </summary>
		/// <param name="world"></param>
		/// <param name="scene"></param>
		/// <param name="node"></param>
		public void SetupServerWorld( GameWorld world, Scene scene, Node node )
		{
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="world"></param>
		/// <param name="scene"></param>
		/// <param name="node"></param>
		public void SetupClientWorld( GameWorld world, Scene scene, Node node )
		{
		}


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
