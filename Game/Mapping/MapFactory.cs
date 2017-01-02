using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Fusion.Core.Mathematics;
using IronStar.Core;

namespace IronStar.Mapping {
	public class MapFactory {

		/// <summary>
		/// Node path
		/// </summary>
		public string NodePath { get; set; }

		/// <summary>
		/// Entity factory
		/// </summary>
		[TypeConverter(typeof(ExpandableObjectConverter))]		
		public EntityFactory Factory { get; set; }


		public override string ToString()
		{
			return "[" + Factory.GetType().Name + "] " + NodePath;
		}
	}
}
