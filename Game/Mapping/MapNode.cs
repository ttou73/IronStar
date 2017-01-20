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
using System.ComponentModel;

namespace IronStar.Mapping {
	public class MapNode {


		[Category("General")]
		[Description("The name of the node")]
		[ReadOnly(true)]
		public string Name { get; set; }
		
		[Category("Transform")]
		[Description("Node translation vector")]
		public Vector3 Translation { get; set; }

		[Category("Transform")]
		[Description("Node rotation quaternion")]
		public Quaternion Rotation { get; set; }

		[Category("Transform")]
		[Description( "Node object scaling" )]
		public float Scaling { get; set; }
	}
}
