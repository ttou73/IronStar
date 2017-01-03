using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEPUphysics.BroadPhaseEntries;
using Fusion.Core.Mathematics;
using Fusion.Engine.Graphics;
using IronStar.Core;

namespace IronStar.Entities {

	public class StaticModelFactory : EntityFactory {

		
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
		/// <param name="gameWorld"></param>
		/// <param name="scene"></param>
		/// <param name="node"></param>
		public void CreateStaticCollisionModel ( GameWorld gameWorld, Scene scene, Node node, Matrix worldMatrix )
		{
			if (!Collidable) {
				return;
			}

			if (node.MeshIndex<0) {
				return;
			}

			var mesh		=	scene.Meshes[ node.MeshIndex ];
			var indices     =   mesh.GetIndices();
			var vertices    =   mesh.Vertices
								.Select( v1 => Vector3.TransformCoordinate( v1.Position, worldMatrix ) )
								.Select( v2 => MathConverter.Convert( v2 ) )
								.ToArray();

			var staticMesh = new StaticMesh( vertices, indices );
			staticMesh.Sidedness = BEPUutilities.TriangleSidedness.Clockwise;

			gameWorld.PhysSpace.Add( staticMesh );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameWorld"></param>
		/// <param name="scene"></param>
		/// <param name="node"></param>
		/// <param name="worldMatrix"></param>
		public void CreateStaticVisibleModel ( GameWorld gameWorld, Scene scene, Node node, Matrix worldMatrix )
		{
			if (!Visible) {
				return;
			}
			if (node.MeshIndex<0) {
				return;
			}
			if (gameWorld.IsServerSide) {
				return;
			}

			var rs = gameWorld.Game.RenderSystem;
			var rw = gameWorld.Game.RenderSystem.RenderWorld;
			var mi = new MeshInstance( rs, scene, scene.Meshes[ node.MeshIndex ] );
			
			mi.World = worldMatrix;

			rw.Instances.Add( mi );
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="world"></param>
		/// <returns></returns>
		public override EntityController Spawn( Entity entity, GameWorld world )
		{
			throw new InvalidOperationException("StaticModelFactory does not spawn entities");
		}

	}
}
