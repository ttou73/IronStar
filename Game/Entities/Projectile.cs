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

namespace IronStar.Entities {

	public class Projectile : EntityController {

		Random rand = new Random();

		readonly float	velocity;
		readonly float	hitImpulse;
		readonly short	hitDamage;
		readonly float	hitRadius;
		readonly string	explosionFX;
		readonly string	trailFX;

		float	lifeTime;

		readonly float totalLifeTime;

		readonly short trailFXAtom;
		readonly Space space;
		GameWorld world;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="game"></param>
		/// <param name="space"></param>
		public Projectile ( Entity entity, GameWorld world, ProjectileFactory factory ) : base(entity,world)
		{
			this.space	=	world.PhysSpace;
			this.world	=	world;

			var atoms	=	world.Atoms;

			totalLifeTime		=	factory.LifeTime;

			this.velocity		=	factory.Velocity	;	
			this.hitImpulse		=	factory.Impulse	;	
			this.hitDamage		=	factory.Damage	;	
			this.lifeTime		=	factory.LifeTime	;	
			this.hitRadius      =   factory.Radius   ;   
			this.explosionFX	=	factory.ExplosionFX	;
			this.trailFX		=	factory.TrailFX		;

			trailFXAtom			=	atoms[ trailFX ]; 

			//	step projectile forward compensating server latency
			UpdateProjectile( entity, 1.0f / Game.GameServer.TargetFrameRate );
		}


		public override void Reset()
		{
			lifeTime = totalLifeTime;
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

			projEntity.Sfx	=	trailFXAtom;

			lifeTime -= elapsedTime;

			Vector3 hitNormal, hitPoint;
			Entity  hitEntity;

			var parent	=	world.GetEntity( projEntity.ParentID );


			if ( lifeTime <= 0 ) {
				world.Kill( projEntity.ID );
			}

			if ( world.RayCastAgainstAll( origin, target, out hitNormal, out hitPoint, out hitEntity, parent ) ) {

				//	inflict damage to hit object:
				world.InflictDamage( hitEntity, projEntity.ParentID, hitDamage, dir * hitImpulse, hitPoint, DamageType.RocketExplosion );

				Explode( explosionFX, projEntity.ID, hitEntity, hitPoint, hitNormal, hitRadius, hitDamage, hitImpulse, DamageType.RocketExplosion );

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
