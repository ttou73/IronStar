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

namespace IronStar.Controllers {

	public class Projectile : EntityController {

		Random rand = new Random();

		public float Velocity;
		public float HitImpulse;
		public short HitDamage;
		public float HitRadius;
		public float LifeTime;
		public string ExplosionFX;
		public string TrailFX;


		readonly Space space;
		World world;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="game"></param>
		/// <param name="space"></param>
		public Projectile ( Entity entity, World world, KeyDataCollection parameters ) : base(entity,world)
		{
			//string explosionFX, float velocity, float radius, short damage, float impulse, float lifeTime

			this.space	=	world.PhysSpace;
			this.world	=	world;

			this.Velocity		=	parameters.Get<float>	("velocity"		, 0);
			this.HitImpulse		=	parameters.Get<float>	("impulse"		, 0);	
			this.HitDamage		=	parameters.Get<short>	("damage"		, 0);
			this.LifeTime		=	parameters.Get<float>	("lifetime"		, 0)/1000.0f;
			this.ExplosionFX	=	parameters.Get<string>	("explosionFX"	, null);
			this.HitRadius      =   parameters.Get<float>	("radius"		, 0);

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
			var target	=	origin + dir * Velocity * elapsedTime;

			LifeTime -= elapsedTime;

			Vector3 hitNormal, hitPoint;
			Entity  hitEntity;

			var parent	=	world.GetEntity( projEntity.ParentID );


			if ( LifeTime <= 0 ) {
				world.Kill( projEntity.ID );
			}

			if ( world.RayCastAgainstAll( origin, target, out hitNormal, out hitPoint, out hitEntity, parent ) ) {

				//	inflict damage to hit object:
				world.InflictDamage( hitEntity, projEntity.ParentID, HitDamage, dir * HitImpulse, hitPoint, DamageType.RocketExplosion );

				Explode( ExplosionFX, projEntity.ID, hitEntity, hitPoint, hitNormal, HitRadius, HitDamage, HitImpulse, DamageType.RocketExplosion );

				//world.SpawnFX( projectile.ExplosionFX, projEntity.ParentID, hitPoint, hitNormal );
				projEntity.Move( hitPoint, projEntity.Rotation, dir * Velocity );

				world.Kill( projEntity.ID );

			} else {
				projEntity.Move( target, projEntity.Rotation, dir.Normalized() * Velocity );
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
