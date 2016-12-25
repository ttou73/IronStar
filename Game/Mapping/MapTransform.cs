using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IronStar.Mapping {

	[TypeConverter(typeof(ExpandableObjectConverter))]
	public sealed class MapTransform {

		[XmlAttribute]
		[Description("Offset along X coordinate")]
		public float X { get; set; }

		[XmlAttribute]
		[Description( "Offset along Y coordinate (elevation)" )]
		public float Y { get; set; }

		[XmlAttribute]
		[Description( "Offset along Z coordinate" )]
		public float Z { get; set; }

		[XmlAttribute]
		[Description( "Rotation around vertical axis" )]
		public float Angle { get; set; }


		public override string ToString()
		{
			return string.Format("[{0}, {1}, {2}: {3}]", X, Y, Z, Angle);
		}
	}
}
