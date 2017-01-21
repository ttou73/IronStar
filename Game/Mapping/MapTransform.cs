using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Core.Mathematics;
using Fusion;
using System.ComponentModel;

namespace IronStar.Mapping {
	public class MapTransform {

		[Category("General")]
		[Description("The name of the node")]
		[ReadOnly(true)]
		public string Name { get; set; }
		
		[Category("Transform")]
		[Description("Node translation vector")]
		public Vector3 Translation { get; set; }

		[Category("Transform")]
		[Description("Node rotation quaternion")]
		public Quaternion Rotation { get; set; } = Quaternion.Identity;

		[Category("Transform")]
		[Description( "Node object scaling" )]
		public float Scaling { get; set; } = 1;

		[Category("General")]
		[Description( "Node object scaling" )]
		public bool Visible { get; set; } = true;

		[Category("General")]
		[Description( "Node object scaling" )]
		public bool Frozen { get; set; }



		/// <summary>
		/// Gets 
		/// </summary>
		public Matrix World {
			get {
				return Matrix.RotationQuaternion( Rotation ) 
					* Matrix.Scaling( Scaling )
					* Matrix.Translation( Translation );
			}
		}
	}
}
