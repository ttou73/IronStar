using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Core.Mathematics;
using IronStar.Core;

namespace IronStar.Mapping {
	public class MapNode {

		
		/// <summary>
		/// List of children
		/// </summary>
		[Browsable(false)]
		public List<MapNode> Children { get; set; }


		[Category("Entity")]
		public string Classname { get; set; }

		[Category("Entity")]
		public string Targetname { get; set; }

		[Category("Entity")]
		public Vector3 Position { get; set; }

		[Category("Entity")]
		public float Yaw { get; set; }

		[Category("Entity")]
		public float Pitch { get; set; }

		[Category("Entity")]
		public float Roll { get; set; }




		/// <summary>
		/// Node's scene path
		/// </summary>
		[Category("Static model")]
		[Description("Path to FBX scene containing given model")]
		public string ScenePath { get; set; }

		/// <summary>
		/// Node's scene path
		/// </summary>
		[Category("Static model")]
		[Description("List of nodes that containes collision meshes. Empty list means including all nodes.")]
		public string CollisionMeshes { get; set; }

		/// <summary>
		/// Node's scene path
		/// </summary>
		[Category("Static model")]
		[Description("List of nodes that containes visible meshes. Empty list means including all nodes.")]
		public string VisibleMeshes { get; set; }


		/// <summary>
		/// 
		/// </summary>
		public MapNode ()
		{
			Children		=	new List<MapNode>();
			CollisionMeshes	=	"";
			VisibleMeshes	=	"";
			ScenePath		=	"";
		}

	}
}
