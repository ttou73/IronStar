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
using ShooterDemo.Core;
using BEPUphysics;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.EntityStateManagement;
using BEPUphysics.PositionUpdating;

namespace ShooterDemo.Controllers {

	public class Projectiles : EntityController {

		Random rand = new Random();

		public float velocity;
		public float impulse;
		public short damageValue;
		public float lifeTime;
		public string explosionFX;
		public float radius;


		readonly Space space;
		MPWorld world;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="game"></param>
		/// <param name="space"></param>
		public Projectiles ( Entity entity, World world, string explosionFX, float velocity, float radius, short damage, float impulse, float lifeTime ) : base(entity,world)
		{
			this.space	=	((MPWorld)world).PhysSpace;
			this.world	=	(MPWorld)world;

			this.velocity		=	velocity;
			this.impulse		=	impulse;	
			this.damageValue	=	damage;
			this.lifeTime		=	lifeTime;
			this.explosionFX	=	explosionFX;
			this.radius			=	radius;

			//	step projectile forward compensate server latency
			if (world.IsServerSide) {
				UpdateProjectile( entity, 1.0f / world.GameServer.TargetFrameRate );
			}
		}


		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update ( float elapsedTime )
		{
			UpdateProjectile( Entity, elapsedTime );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="projEntity"></param>
		/// <param name="projectile"></param>
		public void UpdateProjectile ( Entity projEntity, float elapsedTime )
		{
			var origin	=	projEntity.Position;
			var dir		=	Matrix.RotationQuaternion( projEntity.Rotation ).Forward;
			var target	=	origin + dir * velocity * elapsedTime;

			lifeTime -= elapsedTime;

			Vector3 hitNormal, hitPoint;
			Entity  hitEntity;

			var parent	=	world.GetEntity( projEntity.ParentID );


			if ( lifeTime <= 0 ) {
				world.Kill( projEntity.ID );
			}

			if ( world.RayCastAgainstAll( origin, target, out hitNormal, out hitPoint, out hitEntity, parent ) ) {

				//	inflict damage to hit object:
				world.InflictDamage( hitEntity, projEntity.ParentID, damageValue, dir * impulse, hitPoint, DamageType.RocketExplosion );

				Explode( explosionFX, projEntity.ID, hitEntity, hitPoint, hitNormal, radius, damageValue, impulse, DamageType.RocketExplosion );

				//world.SpawnFX( projectile.ExplosionFX, projEntity.ParentID, hitPoint, hitNormal );
				projEntity.Move( hitPoint, projEntity.Rotation, dir * velocity );

				world.Kill( projEntity.ID );

			} else {
				projEntity.Move( target, projEntity.Rotation, dir.Normalized() * velocity );
			}
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="origin"></param>
		/// <param name="damage"></param>
		/// <param name="impulse"></param>
		/// <param name="damageType"></param>
		public void Explode ( string sfxName, uint attacker, Entity ignore, Vector3 hitPoint, Vector3 hitNormal, float radius, short damage, float impulse, DamageType damageType )
		{
			if (radius>0) {
				var list = world.WeaponOverlap( hitPoint, radius, ignore );

				foreach ( var e in list ) {
					var delta	= e.Position - hitPoint;
					var dist	= delta.Length() + 0.00001f;
					var ndir	= delta / dist;
					var factor	= MathUtil.Clamp((radius - dist) / radius, 0, 1);
					var imp		= factor * impulse;
					var impV	= ndir * imp;
					var impP	= e.Position + rand.UniformRadialDistribution(0.1f, 0.1f);
					var dmg		= (short)( factor * damage );

					world.InflictDamage( e, attacker, dmg, impV, impP, DamageType.RocketExplosion );
				}
			}

			world.SpawnFX( sfxName, attacker, hitPoint, hitNormal );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		public override void Killed ()
		{
		}
	}
}
