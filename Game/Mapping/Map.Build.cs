using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Reflection;
using Native.Fbx;
using IronStar.Entities;
using Fusion.Core.Content;
using System.IO;
using IronStar.Core;
using Fusion.Engine.Storage;
using Fusion.Core.Extensions;
using Fusion.Engine.Graphics;
using BEPUphysics.BroadPhaseEntries;
using Fusion.Core.Mathematics;
using Fusion;
using Native.Recast;

namespace IronStar.Mapping {
	public partial class Map {

		[XmlIgnore]
		public Native.Recast.RCConfig NavConfig { get; set; } = new RCConfig();


		[XmlIgnore]
		public PolyMesh NavigationMesh;

		/// <summary>
		/// 
		/// </summary>
		public void BuildNavigationMesh ()
		{
			NavConfig.CellSize				= 0.3f;
            NavConfig.CellHeight			= 0.2f;
            NavConfig.WalkableSlopeAngle	= 45f;
            NavConfig.WalkableHeight		= (int)Math.Ceiling((2f / NavConfig.CellHeight));
            NavConfig.WalkableClimb			= (int)Math.Floor(0.9f / NavConfig.CellHeight);
            NavConfig.WalkableRadius		= (int)Math.Ceiling(0.6f / NavConfig.CellSize);
            NavConfig.MaxEdgeLen			= (int)(12f / NavConfig.CellSize);
            NavConfig.MaxSimplificationError = 1.3f;
            NavConfig.MinRegionArea			= (int)8 * 8;
            NavConfig.MergeRegionArea		= (int)20 * 20;
            NavConfig.DetailSampleDist		= 6f;
            NavConfig.DetailSampleMaxError	= 1f;
            NavConfig.MaxVertsPerPoly		= 3;		


            var mesh = new RecastMesh();
            mesh.Indices = new int[3] { 0, 1, 2 };
            mesh.Vertices = new Vector3[3] { new Vector3(-100, 0, -100), new Vector3(-100, 0, 100), new Vector3(0, 0, 100) };
            var context = new BuildContext(false);
 
            PolyMesh navigationMesh = RecastBuilder.BuildNavigationMesh( mesh, NavConfig, context );

		}
	}
}
