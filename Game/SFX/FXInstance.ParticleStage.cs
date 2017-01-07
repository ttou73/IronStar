using System;
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
using Fusion.Engine.Audio;


namespace IronStar.SFX {

	/// <summary>
	/// 
	/// </summary>
	public partial class FXInstance {

		public class ParticleStage : Stage {

			private bool	looped;
			readonly int    spriteIndex;
			private bool	stopped;
			private float	time		= 0;
			private int		emitCount	= 0;

			FXParticleStage	stage;
			

			/// <summary>
			/// 
			/// </summary>
			/// <param name="fxEvent"></param>
			/// <param name="spriteIndex"></param>
			/// <param name="delay"></param>
			/// <param name="period"></param>
			/// <param name="sleep"></param>
			/// <param name="count"></param>
			/// <param name="emit"></param>
			public ParticleStage ( FXInstance instance, FXParticleStage stageDesc, FXEvent fxEvent, bool looped ) : base(instance)
			{
				this.stage			=	stageDesc;
				this.looped			=	looped;
				this.spriteIndex	=	instance.fxPlayback.GetSpriteIndex( stageDesc.Sprite );
			}



			public override bool IsExhausted ()
			{
				return stopped;
			}



			public override void Stop ( bool immediate )
			{
				if (immediate) {
					stopped	=	true;
					looped	=	true;
				} else {
					looped	=	true;
				}
			}



			public override void Kill ()
			{
			}



			/// <summary>
			/// 
			/// </summary>
			/// <param name="p"></param>
			/// <param name="fxEvent"></param>
			void Emit ( ref Particle p, FXEvent fxEvent )
			{
				p.Effects		=	stage.Effect;

				p.ImageIndex	=	spriteIndex;

				p.Color0        =   stage.Color0;
				p.Color1        =   stage.Color1;

				p.LifeTime		=	stage.Lifetime.GetLifetime(rand);

				p.FadeIn		=	stage.Timing.FadeIn;
				p.FadeOut		=	stage.Timing.FadeOut;

				float a, b;
				stage.Shape.GetAngles( rand, p.LifeTime, out a, out b );
				p.Rotation0     =   a;
				p.Rotation1     =   b;

				p.Size0         =   stage.Shape.Size0;
				p.Size1         =   stage.Shape.Size1;

				p.Position		=	stage.Position.GetPosition(fxEvent, rand);

				p.Velocity		=	stage.Velocity.GetVelocity(fxEvent, rand);

				var turbulence	=	rand.GaussRadialDistribution(0, stage.Acceleration.Turbulence);
				p.Acceleration	=	stage.Acceleration.DragForce * p.Velocity + turbulence;
				p.Damping		=	stage.Acceleration.Damping;
				p.Gravity		=	stage.Acceleration.GravityFactor;
			}



			/// <summary>
			/// 
			/// </summary>
			/// <param name="dt"></param>
			public override void Update ( float dt, FXEvent fxEvent )
			{	
				var old_time	=	time;
				var new_time	=	time + dt;
				var fxOrigin	=	fxEvent.Origin;

				
				if ( !stopped ) {

					for ( int part=emitCount; true; part++ ) {
	
						float prt_time	= GetParticleEmitTime( part );
						float prt_dt	= prt_time - old_time;

		
						if (prt_time <= new_time) {

							float addTime	=	new_time - prt_time;

							fxEvent.Origin	=	fxOrigin - fxEvent.Velocity * addTime; 
							
							var p = new Particle();
							p.TimeLag		=	addTime;
							p.ImageIndex	=	spriteIndex;
							p.Position		=	fxEvent.Origin;

							if (looped || emitCount < stage.Count) {
								Emit( ref p, fxEvent );
								fxInstance.rw.ParticleSystem.InjectParticle( p );
							} else {
								stopped = true;
								break;
							}

							emitCount++;
			
						} else {
							break;
						}
					}

					//if ( !looped && ( emitCount >= stage.Count ) ) {
					//	stopped = true;
					//}

					time += dt;
				}

				fxEvent.Origin	=	fxOrigin;
			}


			/// <summary>
			/// 
			/// </summary>
			/// <param name="index"></param>
			/// <returns></returns>
			float GetParticleEmitTime( int index )
			{
				float	period				=	stage.Period;
				float	delay				=	stage.Timing.Delay;
				float	bunch				=	stage.Timing.Bunch;
				int		count				=	stage.Count;

				int		indexInPeriod		=	index % count;
				int		periodCount			=	index / count;
				float	interval			=	period * bunch / count;

				return	period * periodCount 
					+	period * delay
					+	interval * indexInPeriod;
			}
		}

	}
}
