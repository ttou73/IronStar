﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Fusion;
using Fusion.Core;
using Fusion.Core.Content;
using Fusion.Core.Mathematics;
using Fusion.Core.Extensions;
using Fusion.Engine.Common;
using Fusion.Engine.Input;
using Fusion.Engine.Client;
using Fusion.Engine.Server;
using Fusion.Engine.Graphics;
using IronStar.Core;
using BEPUphysics;
using BEPUphysics.Character;


namespace IronStar.Entities {
	public partial class Character : EntityController {

		Random rand = new Random();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="deltaTime"></param>
		void UpdateWeaponState ( Entity entity, short deltaTime )
		{
			var attack		=	entity.UserCtrlFlags.HasFlag( UserCtrlFlags.Attack );
			var cooldown	=	entity.GetItemCount( Inventory.WeaponCooldown );

			cooldown		=   (short)Math.Max( 0, cooldown - deltaTime );

			entity.SetItemCount( Inventory.WeaponCooldown, cooldown );

			//	weapon is too hot :
			if (cooldown>0) {
				return;
			}

			if (entity.UserCtrlFlags.HasFlag(UserCtrlFlags.Machinegun		)) entity.ActiveItem = Inventory.Machinegun		;
			if (entity.UserCtrlFlags.HasFlag(UserCtrlFlags.Shotgun			)) entity.ActiveItem = Inventory.Shotgun		;
			if (entity.UserCtrlFlags.HasFlag(UserCtrlFlags.SuperShotgun		)) entity.ActiveItem = Inventory.SuperShotgun	;
			if (entity.UserCtrlFlags.HasFlag(UserCtrlFlags.GrenadeLauncher	)) entity.ActiveItem = Inventory.GrenadeLauncher;
			if (entity.UserCtrlFlags.HasFlag(UserCtrlFlags.RocketLauncher	)) entity.ActiveItem = Inventory.RocketLauncher	;
			if (entity.UserCtrlFlags.HasFlag(UserCtrlFlags.HyperBlaster		)) entity.ActiveItem = Inventory.HyperBlaster	;
			if (entity.UserCtrlFlags.HasFlag(UserCtrlFlags.Chaingun			)) entity.ActiveItem = Inventory.Chaingun		;
			if (entity.UserCtrlFlags.HasFlag(UserCtrlFlags.Railgun			)) entity.ActiveItem = Inventory.Railgun		;
			if (entity.UserCtrlFlags.HasFlag(UserCtrlFlags.BFG				)) entity.ActiveItem = Inventory.BFG			;

			var world = World;

			if (attack) {
				switch (entity.ActiveItem) {
					case Inventory.Machinegun		:	FireBullet(world, entity, 5, 5, 100, 0.03f); break;
					case Inventory.Shotgun			:	FireShot( world, entity, 10,10, 5.0f, 1000, 0.12f); break;
					case Inventory.SuperShotgun		:	break;
					case Inventory.GrenadeLauncher	:	break;
					case Inventory.RocketLauncher	:	FireRocket(world, entity, 800); break;
					case Inventory.HyperBlaster		:	FirePlasma(world, entity, 100); break;
					case Inventory.Chaingun			:	break;
					case Inventory.Railgun			:	FireRail(world, entity, 80, 100, 1500 ); break;
					case Inventory.BFG				:	break;
					default: 
						entity.ActiveItem = Inventory.Machinegun;
						break;
				}
			}
		}



		Vector3 AttackPos ( Entity e )
		{
			var c = e.Controller as Character;
			var m = Matrix.RotationQuaternion(e.Rotation);
			return c.GetPOV() + m.Right * 0.1f + m.Down * 0.1f + m.Forward * 0.3f;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="world"></param>
		/// <param name="attacker"></param>
		/// <param name="cooldown"></param>
		void FirePlasma( GameWorld world, Entity attacker, short cooldown )
		{
			if (!attacker.ConsumeItem( Inventory.Cells, 1 )) {
				return;
			}

			var origin = AttackPos(attacker);

			var e = world.Spawn( "plasma", attacker.ID, origin, attacker.Rotation );

			world.SpawnFX( "MZBlaster",	attacker.ID, origin );

			attacker.SetItemCount( Inventory.WeaponCooldown, cooldown );
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="world"></param>
		/// <param name="attacker"></param>
		/// <param name="cooldown"></param>
		void FireRocket( GameWorld world, Entity attacker, short cooldown )
		{
			if (!attacker.ConsumeItem( Inventory.Rockets, 1 )) {
				return;
			}

			var origin = AttackPos(attacker);

			var e = world.Spawn( "rocket", attacker.ID, origin, attacker.Rotation );

			world.SpawnFX( "MZRocketLauncher",	attacker.ID, origin );

			attacker.SetItemCount( Inventory.WeaponCooldown, cooldown );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="damage"></param>
		void FireBullet ( GameWorld world, Entity attacker, int damage, float impulse, short cooldown, float spread )
		{
			if (!attacker.ConsumeItem( Inventory.Bullets, 1 )) {
				return;
			}

			var view	=	Matrix.RotationQuaternion( attacker.Rotation );
			Vector3 n,p;
			Entity e;

			var direction	=	view.Forward + rand.UniformRadialDistribution(0, spread);
			var origin		=	AttackPos( attacker );

			if (world.RayCastAgainstAll( origin, origin + direction * 400, out n, out p, out e, attacker )) {

				world.SpawnFX( "bullet_hit",	0, p, n );
				//world.SpawnFX( "MZMachinegun",	attacker.ID, origin, n );

				world.InflictDamage( e, attacker.ID, (short)damage, view.Forward * impulse, p, DamageType.BulletHit );

			} else {
				world.SpawnFX( "MZMachinegun",	0, origin, n );
			}

			attacker.SetItemCount( Inventory.WeaponCooldown, cooldown );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="damage"></param>
		void FireShot ( GameWorld world, Entity attacker, int damage, int count, float impulse, short cooldown, float spread )
		{
			if (!attacker.ConsumeItem( Inventory.Bullets, 1 )) {
				return;
			}

			var view	=	Matrix.RotationQuaternion( attacker.Rotation );
			Vector3 n,p;
			Entity e;

			var origin		=	AttackPos( attacker );

			world.SpawnFX( "MZShotgun",	attacker.ID, origin );

			for (int i=0; i<count; i++) {
				
				var direction	=	view.Forward + rand.UniformRadialDistribution(0, spread);

				if (world.RayCastAgainstAll( origin, origin + direction * 400, out n, out p, out e, attacker )) {

					world.SpawnFX( "bullet_hit_shot",	0, p, n );

					world.InflictDamage( e, attacker.ID, (short)damage, view.Forward * impulse, p, DamageType.BulletHit );

				} 
			}

			attacker.SetItemCount( Inventory.WeaponCooldown, cooldown );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="damage"></param>
		void FireRail ( GameWorld world, Entity attacker, int damage, float impulse, short cooldown )
		{
			if (!attacker.ConsumeItem( Inventory.Slugs, 1 )) {
				return;
			}

			var view	=	Matrix.RotationQuaternion( attacker.Rotation );
			Vector3 n,p;
			Entity e;

			var direction	=	view.Forward;
			var origin		=	AttackPos( attacker );

			if (world.RayCastAgainstAll( origin, origin + direction * 200, out n, out p, out e, attacker )) {

				//world.SpawnFX( "PlayerDeathMeat", attacker.ID, p, n );
				world.SpawnFX( "RailHit",		0,					p, n );
				world.SpawnFX( "RailMuzzle",	attacker.ID, origin, n );
				world.SpawnFX( "RailTrail",		attacker.ID, origin, p - origin, attacker.Rotation );

				world.InflictDamage( e, attacker.ID, (short)damage, view.Forward * impulse, p, DamageType.RailHit );

			} else {
				world.SpawnFX( "RailMuzzle",	attacker.ID, origin, n );
				world.SpawnFX( "RailTrail",		attacker.ID, origin, direction * 200, attacker.Rotation );
			}

			attacker.SetItemCount( Inventory.WeaponCooldown, cooldown );
		}


	}
}
