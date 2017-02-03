using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEPUphysics;
using BEPUphysics.BroadPhaseEntries;
using BEPUVector3 = BEPUutilities.Vector3;
using BEPUTransform = BEPUutilities.AffineTransform;
using IronStar.Core;
using Fusion.Engine.Common;
using IronStar.SFX;
using Fusion.Engine.Graphics;
using Fusion.Core.Mathematics;

namespace IronStar.Physics {
	public class PhysicsManager {

		Space physSpace = new BEPUphysics.Space();

		LinkedList<StaticCollisionModel> staticModels = new LinkedList<StaticCollisionModel>();

		readonly Game	Game;
		readonly GameWorld World;


		public Space PhysSpace {
			get {
				return physSpace;
			}
		}
		

		public float Gravity {
			get {
				return -physSpace.ForceUpdater.Gravity.Y;
			}
			set {
				physSpace.ForceUpdater.Gravity = new BEPUVector3(0, -value, 0);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public PhysicsManager ( GameWorld world, float gravity )
		{
			this.World	=	world;
			Game		=	world.Game;

			Gravity		=	gravity;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="dt"></param>
		public void Update ( float elapsedTime )
		{
			foreach ( var sm in staticModels ) {
				sm.Update();
			}

			if (elapsedTime==0) {
				physSpace.TimeStepSettings.MaximumTimeStepsPerFrame = 1;
				physSpace.TimeStepSettings.TimeStepDuration = 0;
				physSpace.Update(0);
				return;
			}

			var dt	=	1 / Game.GameServer.TargetFrameRate;
			physSpace.TimeStepSettings.MaximumTimeStepsPerFrame = 6;
			physSpace.TimeStepSettings.TimeStepDuration = 1.0f/60.0f;
			physSpace.Update(dt);
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="modelAtom"></param>
		public StaticCollisionModel AddStaticCollisionModel ( short modelAtom, Entity entity )
		{
			var modelName	=	World.Atoms[modelAtom];

			var modelDesc	=	World.Content.Load<ModelDescriptor>( @"models\" + modelName );

			var scene		=	World.Content.Load<Scene>( modelDesc.ScenePath );

			var model		=	new StaticCollisionModel( this, modelDesc, scene, entity ); 

			staticModels.AddLast( model );

			return model;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="staticModel"></param>
		/// <returns></returns>
		public bool Remove ( StaticCollisionModel staticModel )
		{
			staticModel.Destroy();
			return staticModels.Remove( staticModel );
		}
	}
}
