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

namespace IronStar.Mapping {
	public abstract class MapNode {

		/// <summary>
		/// 
		/// </summary>
		public MapNode ()
		{
		}


		[Category("Display")]
		[Description( "Node object scaling" )]
		public bool Visible { get; set; } = true;

		[Category("Display")]
		[Description( "Node object scaling" )]
		public bool Frozen { get; set; }

		[Category("Transform")]
		[Description("Node translation vector")]
		public Vector3 Position { get; set; }

		[Category("Transform")]
		[Description("Node rotation quaternion")]
		public Quaternion Rotation { get; set; } = Quaternion.Identity;

		[Category("Transform")]
		[Description( "Node object scaling" )]
		public float Scaling { get; set; } = 1;


		/// <summary>
		/// Gets 
		/// </summary>
		[Browsable(false)]
		public Matrix WorldMatrix {
			get {
				return Matrix.RotationQuaternion( Rotation ) 
					* Matrix.Scaling( Scaling )
					* Matrix.Translation( Position );
			}
		}


		/// <summary>
		/// Creates instance of map object
		/// </summary>
		/// <returns></returns>
		public abstract void SpawnNode ( GameWorld world );

		/// <summary>
		/// Initiates entity activation
		/// </summary>
		public abstract void ActivateNode ();

		/// <summary>
		/// Initiates entity activation
		/// </summary>
		public abstract void UseNode ();

		/// <summary>
		/// Resets entity
		/// </summary>
		/// <param name="world"></param>
		public abstract void ResetNode ( GameWorld world );

		/// <summary>
		/// Performs hard reset of the object
		/// </summary>
		/// <param name="world"></param>
		public abstract void HardResetNode ( GameWorld world );

		/// <summary>
		/// Eliminates object
		/// </summary>
		/// <param name="world"></param>
		public abstract void KillNode ( GameWorld world );

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public abstract MapNode DuplicateNode ();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dr"></param>
		public abstract void DrawNode ( GameWorld world, DebugRender dr, Color color, bool selected );
	}
}
