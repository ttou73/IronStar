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
		}


		/// <summary>
		/// for editor use only
		/// </summary>
		[XmlIgnore]
		public Entity Entity = null;


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
		public Vector3 Position { get; set; }

		[Category("Transform")]
		[Description("Node rotation quaternion")]
		public Quaternion Rotation { get; set; } = Quaternion.Identity;

		[Category("Transform")]
		[Description( "Node object scaling" )]
		public float Scaling { get; set; } = 1;


		/// <summary>
		/// Entity factory
		/// </summary>
		[Browsable(false)]
		public EntityFactory Factory { get; set; }


		/// <summary>
		/// Gets 
		/// </summary>
		[Browsable(false)]
		public Matrix WorldMatrix {
			get {
				return Matrix.RotationQuaternion( Rotation ) 
					* Matrix.Scaling( Scaling )
					* Matrix.Translation( Position );
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Entity SpawnEntity ( GameWorld world )
		{
			Entity = world.Spawn( Factory, 0,0, Position, Rotation );
			return Entity;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="world"></param>
		public void ResetEntity ( GameWorld world )
		{
			if (Entity==null) {
				HardResetEntity(world);
				return;
			}
			if (world.IsAlive(Entity.ID)) {
				Entity.Position = Entity.PositionOld = Position;
				Entity.Rotation = Entity.RotationOld = Rotation;
				Entity.LinearVelocity = Vector3.Zero;
				Entity.AngularVelocity = Vector3.Zero;
				Entity.Controller?.EditorReset( Entity );
			} else {
				HardResetEntity(world);
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="world"></param>
		public void HardResetEntity ( GameWorld world )
		{
			KillEntity( world );
			SpawnEntity( world );
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="world"></param>
		public void KillEntity ( GameWorld world )
		{
			if (Entity!=null) {
				world.Kill( Entity.ID );
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
