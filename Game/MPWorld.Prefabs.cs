using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Fusion;
using Fusion.Core;
using Fusion.Core.Content;
using Fusion.Core.Mathematics;
using Fusion.Engine.Common;
using Fusion.Engine.Input;
using Fusion.Engine.Client;
using Fusion.Engine.Server;
using Fusion.Engine.Graphics;
using ShooterDemo.Core;
using ShooterDemo.Views;
using ShooterDemo.Controllers;

namespace ShooterDemo {
	public partial class MPWorld : World {


		/// <summary>
		/// 
		/// </summary>
		void InitializePrefabs ()
		{
			AddView( new HudView(this) );
			AddView( new CameraView(this) );

			AddPrefab( "startPoint"	, PrefabDummy	);
			AddPrefab( "camera"		, PrefabDummy	);
			AddPrefab( "player"		, PrefabPlayer	);
			AddPrefab( "box"		, PrefabBox		);
			AddPrefab( "rocket"		, PrefabRocket	);
			AddPrefab( "plasma"		, PrefabPlasma	);
			AddPrefab( "mist"		, PrefabMist	);
		}



		/*-----------------------------------------------------------------------------------------
		 * 
		 *	PREFABS :
		 * 
		-----------------------------------------------------------------------------------------*/

		/// <summary>
		/// Used for entities that are just locators, like starting points, etc.
		/// </summary>
		/// <param name="w"></param>
		/// <param name="e"></param>
		public static void PrefabDummy ( World w, Entity e )
		{
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="serverSide"></param>
		public static void PrefabPlayer ( World world, Entity entity )
		{
			entity.Attach( new Weaponry( entity, world ) );
			entity.Attach( new Characters( entity, world ) );

			if (world.IsClientSide) {
				entity.Attach( new ModelView( entity, world, @"scenes\characters\marine\marine", "marine", Matrix.Scaling(0.09f) * Matrix.RotationY(MathUtil.Pi), Matrix.Translation(0,-0.85f,0) ) );
			}

			entity.SetItemCount( Inventory.Health	,	100	);
			entity.SetItemCount( Inventory.Armor	,	0	);
		}



		public static void PrefabRocket ( World world, Entity entity )
		{
			entity.Attach( new Projectiles( entity, world, "Explosion", 30, 3, 100, 100, 5 ) );
			
			if (world.IsClientSide) {
				entity.Attach( new ModelView( entity, world, @"scenes\weapon\projRocket", "rocket", Matrix.Scaling(0.1f), Matrix.Identity ) );
				entity.Attach( new SfxView( entity, world, "RocketTrail" ) );
			}
		}



		public static void PrefabPlasma ( World world, Entity entity )
		{
			entity.Attach( new Projectiles( entity, world, "PlasmaExplosion", 50, 0, 17, 20, 3 ) );
			
			if (world.IsClientSide) {
				//entity.Attach( new ModelView( entity, world, @"scenes\weapon\projRocket", "rocket", Matrix.Scaling(0.1f), Matrix.Identity ) );
				entity.Attach( new SfxView( entity, world, "PlasmaTrail" ) );
			}
		}



		public static void PrefabMist ( World world, Entity entity )
		{
			if (world.IsClientSide) {
				//entity.Attach( new ModelView( entity, world, @"scenes\weapon\projRocket", "rocket", Matrix.Scaling(0.1f), Matrix.Identity ) );
				entity.Attach( new SfxView( entity, world, "Mist" ) );
			}
		}



		public static void PrefabBox ( World world, Entity entity )
		{
			entity.Attach( new RigidBody(entity, world, 1.0f, 0.75f, 0.75f,5 ) ); 

			if (world.IsClientSide) {
				entity.Attach( new ModelView( entity, world, @"scenes\boxes\boxModel", "pCube1", Matrix.Identity, Matrix.Identity ) );
			}
		}

	}
}
