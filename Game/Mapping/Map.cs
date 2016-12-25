using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IronStar.Mapping {
	[XmlInclude( typeof( MapModel ) )]
	[XmlInclude( typeof( MapEntity ) )]
	[XmlInclude( typeof( MapNode ) )]
	public class Map {

		public List<MapNode> Elements { get; set; } = new List<MapNode>();

		public Map ()
		{
			Elements.Add( new MapEnvironment() );
		}
	}
}
