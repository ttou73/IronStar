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
using IronStar.Core;
using IronStar.Views;
using IronStar.Controllers;

namespace IronStar {
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
		}



		void AddTemplate ( string classname, params string[] parameters )
		{
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
			entity.Controller	=	new Characters( entity, world );
		}



		public static void PrefabRocket ( World world, Entity entity )
		{
			entity.Controller	=	new Projectiles( entity, world, "Explosion", 30, 5, 100, 200, 5 );
		}



		public static void PrefabPlasma ( World world, Entity entity )
		{
			entity.Controller	=	new Projectiles( entity, world, "PlasmaExplosion", 50, 0, 17, 20, 3 );
		}



		public static void PrefabMist ( World world, Entity entity )
		{
		}



		public static void PrefabBox ( World world, Entity entity )
		{
			entity.Controller	=	new RigidBody(entity, world, 1.0f, 0.75f, 0.75f,5 ); 
		}

	}
}
