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
using Fusion;
using IronStar.Physics;

namespace IronStar.Entities {

	public class FuncDoor : EntityController {
		
		static Random rand = new Random();

		readonly string fxStart;
		readonly string fxMove;
		readonly string fxStop;
		readonly FuncDoorMode doorMode;
		readonly bool once;
		readonly GameWorld world;

		readonly short model;

		KinematicModel kinematic;

		readonly int framesPerSecond	;
		readonly int openingStartFrame	;
		readonly int openingEndFrame	;
		readonly int closingStartFrame	;
		readonly int closingEndFrame	;

		int activationCount = 0;
		float timer = 0;
		bool enabled;

		enum DoorState {
			Closed,
			Openning,
			Waiting,
			Closing,
		}


		public FuncDoor( Entity entity, GameWorld world, FuncDoorFactory factory ) : base(entity, world)
		{
			this.world	=	world;

			fxStart	=	factory.FXStart;
			fxMove	=	factory.FXMove;
			fxStop	=	factory.FXStop;

			model	=	world.Atoms[ factory.Model ];

			kinematic			=	world.Physics.AddKinematicModel( model, entity );

			once				=	factory.Once;
			doorMode			=	factory.DoorMode;

			framesPerSecond		=	factory.FramesPerSecond;
			openingStartFrame	=	factory.OpeningStartFrame;
			openingEndFrame		=	factory.OpeningEndFrame;
			closingStartFrame	=	factory.ClosingStartFrame;
			closingEndFrame		=	factory.CloseEndFrame;

			Reset();
		}



		public override void Killed()
		{
			world.Physics.Remove( kinematic );
		}


		public override void Activate( Entity activator )
		{
			if (once && activationCount>0) {
				return;
			}

		}


		public override void Reset()
		{
			activationCount	= 0;
		}


		public override void Update( float elapsedTime )
		{
			Log.Warning("FUNC DOOR NOT IMPLEMENTED");
		}
	}



	public enum FuncDoorMode {
		OpenAndCloseAfterDelay,
		ToggleOpenAndClose,
	}



	/// <summary>
	/// 
	/// </summary>
	public class FuncDoorFactory : EntityFactory {

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
		public FuncDoorMode DoorMode { get; set; } = FuncDoorMode.OpenAndCloseAfterDelay;

		[Category("Movement")]
		[Description("Min interval (msec) before door closes")]
		public int CloseDelay { get; set; } = 500;


		[Category("Animation")]
		[Description("Animation frame rate")]
		public int FramesPerSecond { get; set; } = 30;

		[Category("Animation")]
		[Description("Opening animation start inclusive frame")]
		public int OpeningStartFrame { get; set; } = 0;

		[Category("Animation")]
		[Description("Opening animation end inclusive frame")]
		public int OpeningEndFrame { get; set; } = 30;

		[Category("Animation")]
		[Description("Closing animation start inclusive frame")]
		public int ClosingStartFrame { get; set; } = 30;

		[Category("Animation")]
		[Description("Closing animation end inclusive frame")]
		public int CloseEndFrame { get; set; } = 0;


		public override EntityController Spawn( Entity entity, GameWorld world )
		{
			return new FuncDoor( entity, world, this );
		}
	}
}
