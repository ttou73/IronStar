using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Fusion.Core.Mathematics;
using IronStar.Core;
using Fusion.Engine.Graphics;
using IronStar.SFX;
using Fusion.Development;
using System.Drawing.Design;
using Fusion;
using IronStar.Physics;
using BEPUphysics.BroadPhaseEntries;
using BEPUTransform = BEPUutilities.AffineTransform;  

namespace IronStar.Mapping {

	public class MapModel : MapNode {


		[Category( "Appearance" )]
		[Description( "Path to FBX scene" )]
		[Editor( typeof( FbxFileLocationEditor ), typeof( UITypeEditor ) )]
		public string ScenePath { get; set; } = "";

		[Category( "Appearance" )]
		[Description( "Entire model scale" )]
		public float Scale { get; set; } = 1;

		[Category( "Appearance" )]
		[Description( "Model glow color multiplier" )]
		public Color4 Color { get; set; } = new Color4( 10, 10, 10, 1 );
		



		MeshInstance[] instances = null;
		StaticMesh[] collidables = null;


		/// <summary>
		/// 
		/// </summary>
		public MapModel ()
		{
		}



		public override void SpawnEntity( GameWorld world )
		{
			if (string.IsNullOrWhiteSpace(ScenePath)) {
				return;
			}

			var rs			=	world.Game.RenderSystem;

			var pm			=	world.Physics;

			var scene		=	world.Content.Load<Scene>( ScenePath );

			var transforms	=	new Matrix[ scene.Nodes.Count ];
			instances		=	new MeshInstance[ scene.Nodes.Count ];
			collidables		=	new StaticMesh[ scene.Nodes.Count ];

			scene.ComputeAbsoluteTransforms( transforms );


			//
			//	add static collision mesh :
			//
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

				var q = MathConverter.Convert( Rotation );
				var p = MathConverter.Convert( Position );
				staticMesh.WorldTransform = new BEPUTransform( q, p );

				collidables[i] =	staticMesh;
	
				pm.PhysSpace.Add( staticMesh );
			}


			//
			//	add visible mesh instance :
			//
			for ( int i=0; i<scene.Nodes.Count; i++ ) {
				var meshIndex = scene.Nodes[i].MeshIndex;
				
				if (meshIndex>=0) {
					instances[i] = new MeshInstance( rs, scene, scene.Meshes[meshIndex] );
					instances[i].World = transforms[ i ] * WorldMatrix;
					rs.RenderWorld.Instances.Add( instances[i] );
				} else {
					instances[i] = null;
				}
			}
		}



		public override void ActivateEntity()
		{
		}



		public override void Draw( DebugRender dr, Color color, bool selected )
		{
		}



		public override void ResetEntity( GameWorld world )
		{
			HardResetEntity( world );
		}



		public override void HardResetEntity( GameWorld world )
		{
			KillEntity( world );
			SpawnEntity( world );
		}



		public override void KillEntity( GameWorld world )
		{
			var rs = world.Game.RenderSystem;
			var pm = world.Physics;

			if (instances!=null) {
				foreach ( var instance in instances ) {
					if ( instance!=null ) {
						rs.RenderWorld.Instances.Remove( instance );
					}
				}
			}

			if (collidables!=null) {
				foreach ( var collidable in collidables ) {
					if ( collidable!=null ) {
						pm.PhysSpace.Remove( collidable );
					}
				}
			}

			instances	=	null;
			collidables	=	null;
		}


		public override MapNode Duplicate()
		{
			var newNode = (MapModel)MemberwiseClone();

			instances	=	null;
			collidables	=	null;

			return newNode;
		}
	}
}
