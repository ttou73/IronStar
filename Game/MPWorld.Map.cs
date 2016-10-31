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
using Fusion.Core.Extensions;
using Fusion.Engine.Common;
using Fusion.Engine.Input;
using Fusion.Engine.Client;
using Fusion.Engine.Server;
using Fusion.Engine.Graphics;
using ShooterDemo.Core;

namespace ShooterDemo {
	public partial class MPWorld : World {
		List<MeshInstance>	instances = new List<MeshInstance>();


		/// <summary>
		/// Gets list of static instances.
		/// </summary>
		public ICollection<MeshInstance> StaticMeshInstances {
			get { return instances; }
		}



		void AddMeshInstances ()
		{
			foreach ( var inst in instances ) {
				Game.RenderSystem.RenderWorld.Instances.Add( inst );
			}
		}



		/// <summary>
		/// We assume that scene represents level.
		/// </summary>
		/// <param name="scene"></param>
		void ReadMapFromScene ( ContentManager content, Scene scene, bool createRendMeshes )
		{
			//	create entity list :
			instances	=	new List<MeshInstance>();

			//	compute absolute transforms :
			var transforms	= new Matrix[ scene.Nodes.Count ];
			scene.ComputeAbsoluteTransforms( transforms );
			

			//	iterate through the scene's nodes :
			for ( int i=0; i<scene.Nodes.Count; i++) {

				var node	=	scene.Nodes[ i ];
				var world	=	transforms[ i ];
				var name	=	node.Name;
				var mesh	=	node.MeshIndex < 0 ? null : scene.Meshes[ node.MeshIndex ];


				if (name.StartsWith("startPoint")) {	
					Spawn("startPoint", 0, world.TranslationVector, 10 );
					continue;
				}

				if (name.StartsWith("camera")) {
					Spawn("camera", 0, world );
				}


				if (mesh!=null) {
					AddStaticCollisionMesh( mesh, world );

					if (createRendMeshes) {
						var mi		= new MeshInstance( content.Game.RenderSystem, scene, mesh );
						mi.World	= world;

						instances.Add( mi );
					}
				}
			}

			Random	r = new Random();

			for (int i=0; i<100; i++) {
				Spawn("box", 0, Vector3.Up * 400 + r.GaussRadialDistribution(20,2), 0 );
			}// */

			///Spawn("mist", 0, Vector3.Up * 5, 0);
		}

	}
}
