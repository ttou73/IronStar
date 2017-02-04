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

namespace IronStar.Entities {

	public class FuncFX : EntityController {
		
		static Random rand = new Random();

		readonly string fx;
		readonly FuncFXMode fxMode;
		readonly bool once;
		readonly int minInterval;
		readonly int maxInterval;
		readonly bool start;
		readonly short atom;

		int activationCount = 0;
		int timer = 0;
		bool enabled;

		public FuncFX( Entity entity, GameWorld world, FuncFXFactory factory ) : base(entity, world)
		{
			fx			=	factory.FX;
			fxMode		=	factory.FXMode;
			once		=	factory.Once;
			minInterval	=	factory.MinInterval;
			maxInterval	=	factory.MinInterval;
			start		=	factory.Start;

			atom		=	world.Atoms[ fx ];

			Reset();
		}


		public override void Activate( Entity activator )
		{
			if (once && activationCount>0) {
				return;
			}

			activationCount ++;

			if (fxMode==FuncFXMode.Trigger) {
				World.SpawnFX( fx, 0, Entity.Position, Entity.LinearVelocity, Entity.Rotation );
			} else {
				enabled = !enabled;
			}
		}


		public override void Reset()
		{
			activationCount = rand.Next( minInterval, maxInterval );
			enabled			= start;
		}


		public override void Update( float elapsedTime )
		{
			if (fxMode==FuncFXMode.Persistent) {
				Entity.Sfx = enabled ? atom : (short)0;
			}

			if (fxMode==FuncFXMode.AutoTrigger) {
				
				if (enabled) {
					timer -= (int)(elapsedTime*1000);

					if (timer<0) {
						World.SpawnFX( fx, 0, Entity.Position, Entity.LinearVelocity, Entity.Rotation );
						timer = rand.Next( minInterval, maxInterval );
					}
				} else {
					timer = 0;
				}
			}

			if (fxMode==FuncFXMode.Trigger) {
				
			}
		}
	}



	public enum FuncFXMode {
		Persistent,
		AutoTrigger,
		Trigger,
	}



	/// <summary>
	/// https://www.iddevnet.com/quake4/Entity_FuncFX
	/// </summary>
	public class FuncFXFactory : EntityFactory {

		[Category("FX")]
		[Description("Name of the FX object")]
		public string FX { get; set; } = "";

		[Category("FX")]
		[Description("FX mode")]
		public FuncFXMode FXMode { get; set; }

		[Category("FX")]
		[Description("Indicated that given effect could be trigerred only once")]
		public bool Once { get; set; }

		[Category("FX")]
		[Description("Indicated that given effect is enabled by default")]
		public bool Start { get; set; }

		[Category("FX")]
		[Description("Min interval (msec) between auto-triggered events")]
		public int MinInterval { get; set; } = 1;

		[Category("FX")]
		[Description("Max interval (msec) between auto-triggered events")]
		public int MaxInterval { get; set; } = 1;


		public override EntityController Spawn( Entity entity, GameWorld world )
		{
			return new FuncFX( entity, world, this );
		}
	}
}
