using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion;
using Fusion.Core;
using Fusion.Core.Content;
using Fusion.Core.Mathematics;
using Fusion.Engine.Common;
using Fusion.Engine.Input;
using Fusion.Engine.Client;
using Fusion.Engine.Server;
using Fusion.Engine.Graphics;
using Fusion.Core.Extensions;
using IronStar.Core;
using BEPUphysics;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.EntityStateManagement;
using BEPUphysics.PositionUpdating;
using Fusion.Core.IniParser.Model;
using System.ComponentModel;

namespace IronStar.Controllers {

	public class ProjectileFactory : EntityFactory {

		[Category("Projectile")]
		[Description("Projectile velocity (m/sec)")]
		public float Velocity { get; set; } = 10;	

		[Category("Projectile")]
		[Description("Projectile damage impulse")]
		public float Impulse	{ get; set; } = 0;

		[Category("Projectile")]
		[Description("Hit damage")]
		public short Damage { get; set; } = 0;

		[Category("Projectile")]
		[Description("Projectile life-time in seconds")]
		public float LifeTime { get; set; } = 10;

		[Category("Projectile")]
		[Description("Hit radius in meters")]
		public float Radius { get; set; } = 0;


		[Category("Visual Effects")]
		public string	ExplosionFX	{ get; set; } = "";

		[Category("Visual Effects")]
		public string	TrailFX		{ get; set; } = "";


		public override EntityController Spawn( Entity entity, GameWorld world )
		{
			return new Projectile( entity, world, this );
		}
	}
}
