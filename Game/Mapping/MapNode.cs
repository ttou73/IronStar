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
using IronStar.SFX;

namespace IronStar.Mapping {
	public class MapNode {

		/// <summary>
		/// 
		/// </summary>
		public MapNode ()
		{
			Factory		=	null;
			Model		=	new ModelDescriptor();
		}


		[Category("Entity")]
		[Description("Entity target name")]
		public string TargetName { get; set; }

		[Category("Display")]
		[Description( "Node object scaling" )]
		public bool Visible { get; set; } = true;

		[Category("Display")]
		[Description( "Node object scaling" )]
		public bool Frozen { get; set; }

		
		[Category("Transform")]
		[Description("Node translation vector")]
		public Vector3 Translation { get; set; }

		[Category("Transform")]
		[Description("Node rotation quaternion")]
		public Quaternion Rotation { get; set; } = Quaternion.Identity;

		[Category("Transform")]
		[Description( "Node object scaling" )]
		public float Scaling { get; set; } = 1;


		/// <summary>
		/// Entity factory
		/// </summary>
		[TypeConverter(typeof(ExpandableObjectConverter))]		
		public EntityFactory Factory { get; set; }

		/// <summary>
		/// Entity factory
		/// </summary>
		[TypeConverter(typeof(ExpandableObjectConverter))]		
		public ModelDescriptor Model { get; set; }


		/// <summary>
		/// Gets 
		/// </summary>
		[Browsable(false)]
		public Matrix WorldMatrix {
			get {
				return Matrix.RotationQuaternion( Rotation ) 
					* Matrix.Scaling( Scaling )
					* Matrix.Translation( Translation );
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="dr"></param>
		public void Draw ( DebugRender dr )
		{
			dr.DrawBasis( WorldMatrix, 1 );
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
