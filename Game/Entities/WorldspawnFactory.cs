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
		public float SkyTrubidity {
			get {
				return turbidity;
			}
			set {
				turbidity = MathUtil.Clamp( value, 2, 8 );
			}
		}
		float turbidity = 2;

		[Category( "Sky" )]
		public Vector3 SunPosition { get; set; } = Vector3.One;

		[Category( "Sky" )]
		public float SunIntensity { get; set; } = 100;

		[Category( "Fog" )]
		public float FogDensity { get; set; } = 0.001f;



		/// <summary>
		/// 
		/// </summary>
		/// <param name="world"></param>
		/// <param name="scene"></param>
		/// <param name="node"></param>
		public void SetupWorldPhysics( GameWorld gameWorld )
		{
			gameWorld.PhysSpace.ForceUpdater.Gravity = BEPUutilities.Vector3.Down * Gravity;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="world"></param>
		/// <returns></returns>
		public override EntityController Spawn( Entity entity, GameWorld world )
		{
			throw new InvalidOperationException("WorldspawnFactory do not spawn entities!");
		}

	}
}
