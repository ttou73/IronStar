using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Core.Mathematics;
using IronStar.Core;

namespace IronStar.Mapping {
	public class MapModel : MapNode {

		#warning MAP MODEL COULD BE PASSIVE (STATIC) OR ACTIVE (DYNAMIC)

		/// <summary>
		/// Node's scene path
		/// </summary>
		[Category( "Model" )]
		[Description( "Path to FBX scene containing given model" )]
		public string ScenePath { get; set; } = "";

		/// <summary>
		/// Node's scene path
		/// </summary>
		[Category( "Model" )]
		[Description( "List of nodes that containes collision meshes. Empty list means including all nodes." )]
		public string CollisionMeshes { get; set; } = "";

		/// <summary>
		/// Node's scene path
		/// </summary>
		[Category( "Model" )]
		[Description( "List of nodes that containes visible meshes. Empty list means including all nodes." )]
		public string VisibleMeshes { get; set; } = "";


		/// <summary>
		/// 
		/// </summary>
		public MapModel ()
		{
		}

	}
}
