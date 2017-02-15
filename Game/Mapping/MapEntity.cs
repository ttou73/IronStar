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
	public class MapEntity : MapNode {


		/// <summary>
		/// for editor use only
		/// </summary>
		[XmlIgnore]
		public Entity Entity = null;

		/// <summary>
		/// Entity factory
		/// </summary>
		[Browsable(false)]
		public EntityFactory Factory { get; set; }


		/// <summary>
		/// 
		/// </summary>
		public MapEntity ()
		{
		}



		public override void SpawnNode( GameWorld world )
		{
			Entity = world.Spawn( Factory, 0,0, Position, Rotation );
		}



		public override void ActivateNode()
		{
			Entity?.Controller?.Activate( null );
		}



		public override void DrawNode( DebugRender dr, Color color, bool selected )
		{
			dr.DrawBasis( WorldMatrix, 1 );
			Factory.Draw( dr, WorldMatrix, color );
		}



		public override void ResetNode( GameWorld world )
		{
			if (Entity==null) {
				HardResetNode(world);
				return;
			}
			if (world.IsAlive(Entity.ID)) {
				Entity.Position = Entity.PositionOld = Position;
				Entity.Rotation = Entity.RotationOld = Rotation;
				Entity.LinearVelocity = Vector3.Zero;
				Entity.AngularVelocity = Vector3.Zero;
				Entity.Controller?.Reset();
			} else {
				HardResetNode(world);
			}
		}



		public override void HardResetNode( GameWorld world )
		{
			KillNode( world );
			SpawnNode( world );
		}



		public override void KillNode( GameWorld world )
		{
			if (Entity!=null) {
				world.Kill( Entity.ID );
			}
		}


		public override MapNode DuplicateNode()
		{
			var newNode = (MapEntity)MemberwiseClone();
			newNode.Factory = Factory.Duplicate();
			newNode.Entity  = null;
			return newNode;
		}
	}
}
