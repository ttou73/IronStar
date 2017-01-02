using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Reflection;
using Native.Fbx;
using IronStar.Entities;

namespace IronStar.Mapping {
	public class Map {

		/// <summary>
		/// Base scene path
		/// </summary>
		public string BaseScene { get; set; }


		/// <summary>
		/// List of nodes
		/// </summary>
		public List<MapNode> Nodes { get; set; }


		/// <summary>
		/// 
		/// </summary>
		public Map ()
		{
		}


	}
}
