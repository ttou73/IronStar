using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Core.Mathematics;
using IronStar.Core;

namespace IronStar.Mapping {
	public class MapEntity : MapNode {

		/// <summary>
		/// 
		/// </summary>
		[Category( "Entity" )]
		public string Classname { get; set; } = "";

		/// <summary>
		/// 
		/// </summary>
		[Category( "Entity" )]
		public string Targetname { get; set; } = "";


		/// <summary>
		/// 
		/// </summary>
		public MapEntity ()
		{
		}


		public override string ToString()
		{
			return string.Format("Entity: {0}", Classname );
		}
	}
}
