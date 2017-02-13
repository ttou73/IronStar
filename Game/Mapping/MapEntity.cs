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



		public override void SpawnEntity( GameWorld world )
		{
			Entity = world.Spawn( Factory, 0,0, Position, Rotation );
		}



		public override void ActivateEntity()
		{
			Entity?.Controller?.Activate( null );
		}



		public override void Draw( DebugRender dr, Color color, bool selected )
		{
			dr.DrawBasis( WorldMatrix, 1 );
			Factory.Draw( dr, WorldMatrix, color );
		}



		public override void ResetEntity( GameWorld world )
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
				Entity.Controller?.Reset();
			} else {
				HardResetEntity(world);
			}
		}



		public override void HardResetEntity( GameWorld world )
		{
			KillEntity( world );
			SpawnEntity( world );
		}



		public override void KillEntity( GameWorld world )
		{
			if (Entity!=null) {
				world.Kill( Entity.ID );
			}
		}


		public override MapNode Duplicate()
		{
			var newNode = (MapEntity)MemberwiseClone();
			newNode.Factory = Factory.Duplicate();
			newNode.Entity  = null;
			return newNode;
		}
	}
}
