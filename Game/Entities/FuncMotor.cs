using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Core.Content;
using Fusion.Core.Mathematics;
using Fusion.Engine.Common;
using Fusion.Core.Extensions;
using IronStar.Core;
using IronStar.SFX;
using System.ComponentModel;
using Fusion.Development;
using System.Drawing.Design;
using IronStar.Physics;

namespace IronStar.Entities {

	public class FuncMotor : EntityController {
		
		static Random rand = new Random();

		readonly string fxStart;
		readonly string fxMove;
		readonly string fxStop;
		readonly bool once;

		readonly short model;
		readonly short startFrame;
		readonly short endFrame;
		readonly short animLength;

		readonly KinematicModel kinematic;
		readonly int framesPerSecond	;

		int activationCount = 0;
		float timer = 0;
		bool enabled;

		float frameCounter;


		public FuncMotor( Entity entity, GameWorld world, FuncMotorFactory factory ) : base(entity, world)
		{
			fxStart	=	factory.FXStart;
			fxMove	=	factory.FXMove;
			fxStop	=	factory.FXStop;

			model		=	world.Atoms[ factory.Model ];

			kinematic	=	world.Physics.AddKinematicModel( model, entity );

			once				=	factory.Once;
			enabled				=	factory.Start;

			startFrame			=	factory.StartFrame;
			endFrame			=	factory.EndFrame;
			animLength			=	(short)(endFrame - startFrame);

			framesPerSecond		=	factory.FramesPerSecond;

			Reset();
		}


		public override void Killed()
		{
			World.Physics.Remove( kinematic );
		}


		public override void Activate( Entity activator )
		{
			if (once && activationCount>0) {
				return;
			}

			enabled	=	!enabled;
		}


		public override void Reset()
		{
			activationCount	= 0;
		}


		public override void Update( float elapsedTime )
		{
			if (enabled) {
				frameCounter += framesPerSecond * elapsedTime;

				if (frameCounter>=animLength) {
					frameCounter = 0;
				}
			}

			var frame = frameCounter + startFrame;

			Entity.AnimFrame	= frame;
			Entity.Model		= model;
		}
	}



	/// <summary>
	/// 
	/// </summary>
	public class FuncMotorFactory : EntityFactory {

		[Category("Appearance")]
		[Description("Name of the model")]
		public string Model  { get; set; } = "";

		[Category("Effects")]
		[Description("FX to play when door starts moving")]
		public string FXStart { get; set; } = "";

		[Category("Effects")]
		[Description("FX to play when door is moving")]
		public string FXMove { get; set; } = "";

		[Category("Effects")]
		[Description("FX to play when door stops moving")]
		public string FXStop { get; set; } = "";


		[Category("Movement")]
		[Description("Indicated that door could be trigerred only once")]
		public bool Once { get; set; }

		[Category("Movement")]
		[Description("Door operation mode")]
		public bool Start { get; set; } = false;

		[Category("Animation")]
		[Description("Animation frame rate")]
		public int FramesPerSecond { get; set; } = 30;

		[Category("Animation")]
		[Description("Animation frame rate")]
		public short StartFrame { get; set; } = 0;

		[Category("Animation")]
		[Description("Animation frame rate")]
		public short EndFrame { get; set; } = 30;


		public override EntityController Spawn( Entity entity, GameWorld world )
		{
			return new FuncMotor( entity, world, this );
		}
	}
}
