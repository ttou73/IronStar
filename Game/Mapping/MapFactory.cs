using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Fusion.Core.Mathematics;
using IronStar.Core;
using Fusion.Engine.Graphics;

namespace IronStar.Mapping {
	public class MapFactory {

		/// <summary>
		/// 
		/// </summary>
		public MapFactory ()
		{
			Transform	=	new MapTransform();
			Factory		=	null;
		}


		/// <summary>
		/// 
		/// </summary>
		public Guid Guid { get; set; }


		/// <summary>
		/// 
		/// </summary>
		[TypeConverter(typeof(ExpandableObjectConverter))]		
		public MapTransform Transform { get; set; }

		/// <summary>
		/// Entity factory
		/// </summary>
		[TypeConverter(typeof(ExpandableObjectConverter))]		
		public EntityFactory Factory { get; set; }


		/// <summary>
		/// 
		/// </summary>
		/// <param name="dr"></param>
		public void Draw ( DebugRender dr )
		{
			dr.DrawBasis( Transform.World, 1 );
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return "[" + Factory.ToString() + "] ";
		}
	}
}
