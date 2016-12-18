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
using Fusion.Engine.Audio;
using IronStar.Views;

namespace IronStar.SFX {
	public class ModelInstance {

		readonly Matrix preTransform;
		readonly Color4 color;
		readonly ModelManager modelManager;
		readonly Entity entity;
		readonly Scene scene;

		Matrix[] globalTransforms;
		MeshInstance[] meshInstances;

		readonly int nodeCount;

		public bool Killed {
			get; private set;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="modelManager"></param>
		/// <param name="preTransform"></param>
		/// <param name="meshInstance"></param>
		public ModelInstance ( ModelManager modelManager, ModelDescriptor descriptor, Scene scene, Entity entity )
		{
			this.modelManager   =   modelManager;
			this.preTransform   =   descriptor.ComputePreTransformMatrix();
			this.scene			=   scene;
			this.entity			=	entity;
			this.color			=	descriptor.Color;

			nodeCount			=	scene.Nodes.Count;

			globalTransforms	=	new Matrix[ scene.Nodes.Count ];
			scene.ComputeAbsoluteTransforms( globalTransforms );

			meshInstances		=	new MeshInstance[ scene.Nodes.Count ];

			for ( int i=0; i<nodeCount; i++ ) {
				var meshIndex = scene.Nodes[i].MeshIndex;
				
				if (meshIndex>=0) {
					meshInstances[i] = new MeshInstance( modelManager.rs, scene, scene.Meshes[meshIndex] );
					modelManager.rw.Instances.Add( meshInstances[i] );
				} else {
					meshInstances[i] = null;
				}
			}

		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="lerpFactor"></param>
		public void Update ( float dt, float lerpFactor )
		{
			var worldMatrix = entity.GetWorldMatrix( lerpFactor );

			for ( int i = 0; i<nodeCount; i++ ) {
				if (meshInstances[i]!=null) {
					meshInstances[i].World = preTransform * globalTransforms[i] * worldMatrix;
					meshInstances[i].Color = color;
				}
			}
		}



		/// <summary>
		/// Marks current model instance to remove.
		/// </summary>
		public void Kill ()
		{
			for ( int i = 0; i<nodeCount; i++ ) {
				if (meshInstances[i]!=null) {
					modelManager.rw.Instances.Remove( meshInstances[i] );
				}
			}
			Killed = true;
		}


	}
}
