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
using BEPUphysics;
using BEPUphysics.BroadPhaseEntries;
using BEPUVector3 = BEPUutilities.Vector3;
using BEPUTransform = BEPUutilities.AffineTransform;


namespace ShooterDemo {
	public partial class MPWorld {

		Space physSpace;


		/// <summary>
		/// Gets physical space
		/// </summary>
		public Space PhysSpace { 
			get {
				return physSpace;
			}
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="gravity"></param>
		void InitPhysSpace ( float gravity )
		{
			physSpace	=	new BEPUphysics.Space();
			physSpace.ForceUpdater.Gravity	=	BEPUVector3.Down * gravity;
		}



		/// <summary>
		/// Adds static mesh to phys space.
		/// </summary>
		/// <param name="mesh"></param>
		/// <param name="transform"></param>
		void AddStaticCollisionMesh ( Mesh mesh, Matrix transform )
		{
			var indices		=	mesh.GetIndices();
			var vertices	=	mesh.Vertices
								.Select( v1 => Vector3.TransformCoordinate( v1.Position, transform ) )
								.Select( v2 => MathConverter.Convert( v2 ) )
								.ToArray();

			var staticMesh = new StaticMesh( vertices, indices );
			staticMesh.Sidedness = BEPUutilities.TriangleSidedness.Clockwise;
			physSpace.Add( staticMesh );
		}
	}
}
