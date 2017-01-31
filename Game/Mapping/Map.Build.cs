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

		public Configuration NavConfig { get; set; } = new Configuration();

		/// <summary>
		/// 
		/// </summary>
		public void BuildNavigationMesh ()
		{
			
		}

	}
}
