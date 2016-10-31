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
using Fusion.Engine.Common;
using Fusion.Engine.Input;
using Fusion.Engine.Client;
using Fusion.Engine.Server;
using Fusion.Engine.Graphics;
using ShooterDemo.Core;
using Fusion.Engine.Audio;


namespace ShooterDemo.SFX {

	/// <summary>
	/// 
	/// </summary>
	public partial class SfxInstance {

		protected class ParticleStage : Stage {

			bool looped;
			bool stopped;

			readonly int			spriteIndex;
			readonly float 			delay;
			readonly float			period;
			readonly float			sleep;
			readonly int			count;
			readonly EmitFunction	emit;


			protected float			time		= 0;
			protected int			emitCount	= 0;
			

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
			public ParticleStage ( SfxInstance instance, int spriteIndex, float delay, float period, float sleep, int count, bool looped, EmitFunction emit ) : base(instance)
			{
				this.looped			=	false;
				this.spriteIndex	=	spriteIndex	;
				this.delay			=	delay		;
				this.period			=	period		;
				this.sleep			=	sleep		;
				this.count			=	count		;
				this.emit			=	emit		;
				this.looped			=	looped;
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

							emit( ref p, fxEvent );
 
							//SfxInstance.rw.Debug.Trace( p.Position, 0.2f, Color.Yellow );
							SfxInstance.rw.ParticleSystem.InjectParticle( p );

							emitCount++;
			
						} else {
							break;
						}
					}

					if ( !looped && ( time >= delay + period ) ) {
						stopped = true;
					}

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
				float	full_cycle			= delay + period + sleep;
				int		num_cycles			= index / count;
				int		part_ind_in_bunch	= index	% count;
				float	interval			= period / (float)count;
	
				return	full_cycle * num_cycles + 
						delay +
						interval * part_ind_in_bunch;
			}
		}

	}
}
