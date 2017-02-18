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
using Fusion.Core.Extensions;

namespace IronStar.Physics {
	public class PhysicsManager {

		Space physSpace = new Space();

		LinkedList<KinematicModel> kinematics = new LinkedList<KinematicModel>();

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
			foreach ( var k in kinematics ) {
				k.Update();
			}

			if (elapsedTime==0) {
				physSpace.TimeStepSettings.MaximumTimeStepsPerFrame = 1;
				physSpace.TimeStepSettings.TimeStepDuration = 1/1024.0f;
				physSpace.Update(1/1024.0f);
				return;
			}

			var dt	=	elapsedTime;
			physSpace.TimeStepSettings.MaximumTimeStepsPerFrame = 6;
			physSpace.TimeStepSettings.TimeStepDuration = 1.0f/60.0f;
			physSpace.Update(dt);
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="modelAtom"></param>
		public KinematicModel AddKinematicModel ( short modelAtom, Entity entity )
		{
			var modelName	=	World.Atoms[modelAtom];

			var modelDesc	=	World.Content.Load<ModelDescriptor>( @"models\" + modelName );

			var scene		=	World.Content.Load<Scene>( modelDesc.ScenePath );

			var model		=	new KinematicModel( this, modelDesc, scene, entity ); 

			kinematics.AddLast( model );

			return model;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="staticModel"></param>
		/// <returns></returns>
		public bool Remove ( KinematicModel staticModel )
		{
			staticModel.Destroy();
			return kinematics.Remove( staticModel );
		}
	}
}
