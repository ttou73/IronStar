using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEPUutilities;
using BEPUphysics;
using BEPUphysics.BroadPhaseEntries;
using BEPUVector3 = BEPUutilities.Vector3;
using BEPUTransform = BEPUutilities.AffineTransform;  
using BEPUMatrix = BEPUutilities.Matrix;
using IronStar.Core;
using Fusion.Engine.Common;
using IronStar.SFX;
using Fusion.Engine.Graphics;
using Fusion.Core.Mathematics;
using Matrix = Fusion.Core.Mathematics.Matrix;
using Vector3 = Fusion.Core.Mathematics.Vector3;


namespace IronStar.Physics {
	public class StaticCollisionModel {

		readonly PhysicsManager physicsManager;
		readonly StaticMesh[] staticMeshes;
		readonly Entity entity;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="modelManager"></param>
		/// <param name="descriptor"></param>
		/// <param name="scene"></param>
		/// <param name="entity"></param>
		public StaticCollisionModel ( PhysicsManager physicsManager, ModelDescriptor descriptor, Scene scene, Entity entity )
		{
			this.entity			=	entity;
			this.physicsManager	=	physicsManager;
			
			staticMeshes	=	new StaticMesh[ scene.Nodes.Count ];
			var transforms	=	new Matrix[ scene.Nodes.Count ];

			scene.ComputeAbsoluteTransforms( transforms );


			for ( int i=0; i<scene.Nodes.Count; i++ ) {

				var node = scene.Nodes[i];

				if (node.MeshIndex<0) {
					continue;
				}

				var mesh		=	scene.Meshes[ node.MeshIndex ];
				var indices     =   mesh.GetIndices();
				var vertices    =   mesh.Vertices
									.Select( v1 => Vector3.TransformCoordinate( v1.Position, transforms[i] ) )
									.Select( v2 => MathConverter.Convert( v2 ) )
									.ToArray();

				var staticMesh = new StaticMesh( vertices, indices );
				staticMesh.Sidedness = BEPUutilities.TriangleSidedness.Clockwise;

				staticMesh.Tag	=	entity;

				staticMeshes[i] =	staticMesh;
	
				physicsManager.PhysSpace.Add( staticMesh );
			}

		}



		public void Update ()
		{
			var worldMatrix = entity.GetWorldMatrix( 1 );

			foreach ( var sm in staticMeshes ) {
				if (sm==null) {
					continue;
				}
				var q = MathConverter.Convert( entity.Rotation );
				var p = MathConverter.Convert( entity.Position );
				sm.WorldTransform = new BEPUTransform( q, p );
			}
		}
		


		/// <summary>
		/// 
		/// </summary>
		public void Destroy ()
		{
			foreach ( var sm in staticMeshes ) {
				if (sm==null) {
					continue;
				}
				physicsManager.PhysSpace.Remove( sm );
			}
		}
	}
}
