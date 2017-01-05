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
using IronStar.Views;


namespace IronStar.SFX {

	/// <summary>
	/// 
	/// </summary>
	public partial class FXInstance {
		
		static protected Random rand = new Random();

		protected readonly FXPlayback fxPlayback;
		protected readonly RenderWorld rw;
		protected readonly SoundWorld sw;

		public delegate void EmitFunction ( ref Particle p, FXEvent fxEvent );

		readonly List<Stage> stages = new List<Stage>();

		private readonly short fxAtom;
		private FXEvent	fxEvent;

		protected readonly bool looped;


		/// <summary>
		/// Indicates thet SFX is looped.
		/// </summary>
		public bool Looped {
			get;
			set;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="sfxSystem"></param>
		/// <param name="fxEvent"></param>
		public FXInstance( FXPlayback sfxSystem, FXEvent fxEvent, FXFactory fxFactory, bool looped )
		{
			this.fxAtom		=	fxEvent.FXAtom;
			this.fxPlayback	=	sfxSystem;
			this.rw			=	sfxSystem.rw;
			this.sw			=	sfxSystem.sw;
			this.fxEvent	=	fxEvent;

			AddParticleStage( fxFactory.ParticleStage1, fxEvent, looped );
			AddParticleStage( fxFactory.ParticleStage2, fxEvent, looped );
			AddParticleStage( fxFactory.ParticleStage3, fxEvent, looped );
			AddParticleStage( fxFactory.ParticleStage4, fxEvent, looped );

			AddLightStage( fxFactory.LightStage, fxEvent, looped );
			AddSoundStage( fxFactory.SoundStage, fxEvent, looped );
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Color4 GetRailColor (float intensity = 1)
		{
			//return new Color4(2000 * intensity,100 * intensity,4000 * intensity,1);
			//return new Color4(4000 * intensity,100 * intensity,100 * intensity,1);
			return new Color4(200 * intensity,200 * intensity,8000 * intensity,1);
			//return new Color4(100 * intensity,4000 * intensity,100 * intensity,1);
		}


		/// <summary>
		/// Immediatly removes all sfx instance stuff, 
		/// like light sources and sounds.
		/// Check IsExhausted before calling Kill to produce smooth animation.
		/// </summary>
		public void Kill ()
		{
			foreach ( var stage in stages ) {
				stage.Kill();
			}
			stages.Clear();
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="elapsedTime"></param>
		public void Update ( float dt, float lerpFactor )
		{
			var ent = fxPlayback.world.GetEntity( fxEvent.EntityID );

			if (ent!=null) {
				fxEvent.Origin		=	ent.LerpPosition( lerpFactor );
				fxEvent.Rotation	=	ent.LerpRotation( lerpFactor );
				fxEvent.Velocity	=	ent.LinearVelocity;
			}

			foreach ( var stage in stages ) {
				stage.Update( dt, fxEvent );
			}
		}



		public void Move ( Vector3 position, Vector3 velocity, Quaternion rotation )
		{
			fxEvent.Origin		=	position;
			fxEvent.Velocity	=	velocity;
			fxEvent.Rotation	=	rotation;
		}


		/// <summary>
		/// 
		/// </summary>
		public bool IsExhausted {
			get {
				
				bool isExhausted = true;

				//	check stages :
				foreach ( var stage in stages ) {
					isExhausted &= stage.IsExhausted();
				}

				return isExhausted;
			}
		}



		/// <summary>
		/// Shakes camera associated with parent entity only.
		/// </summary>
		/// <param name="yaw"></param>
		/// <param name="pitch"></param>
		/// <param name="roll"></param>
		public void ShakeCamera ( float yaw, float pitch, float roll )
		{
			fxPlayback.world.GetView<GameCamera>().Shake( fxEvent.EntityID, yaw, pitch, roll );
		}

		/*-----------------------------------------------------------------------------------------
		 * 
		 *	Stage creation functions :
		 * 
		-----------------------------------------------------------------------------------------*/

		/// <summary>
		/// 
		/// </summary>
		/// <param name="spriteName"></param>
		/// <param name="delay"></param>
		/// <param name="period"></param>
		/// <param name="sleep"></param>
		/// <param name="count"></param>
		/// <param name="emit"></param>
		public void AddParticleStage ( FXParticleStage stageDesc, FXEvent fxEvent, bool looped )
		{
			if ( !stageDesc.Enabled ) {
				return;
			}
			if ( stageDesc.Count==0) {
				return;
			}
			var stage			=	new ParticleStage( this, stageDesc, fxEvent, looped );

			stages.Add( stage );
		}



		public void AddLightStage ( FXLightStage stageDesc, FXEvent fxEvent, bool looped )
		{
			if (!stageDesc.Enabled) {
				return;
			}

			var stage = new LightStage( this, stageDesc, fxEvent, looped );
			stages.Add( stage );
		}


		public void AddSoundStage ( FXSoundStage stageDesc, FXEvent fxEvent, bool looped )
		{
			if ( !stageDesc.Enabled ) {
				return;
			}

			stages.Add( new SoundStage( this, stageDesc, fxEvent, looped ) );
		}

		/*-----------------------------------------------------------------------------------------
		 * 
		 *	Particle helper functions
		 * 
		-----------------------------------------------------------------------------------------*/

		protected static void SetupMotion ( ref Particle p, Vector3 origin, Vector3 velocity, Vector3 accel, float damping=0, float gravity=0 )
		{
			p.Position		=	origin;
			p.Velocity		=	velocity;
			p.Acceleration	=	accel;
			p.Damping		=	damping;
			p.Gravity		=	gravity;
		}


		protected void	SetupTiming	( ref Particle p, float totalTime, float fadeIn, float fadeOut ) 
		{
			p.LifeTime		=	totalTime;
			p.FadeIn		=	fadeIn;
			p.FadeOut		=	fadeOut;
		}

		
		protected void	SetupSize ( ref Particle p, float size0, float size1 ) 
		{
			p.Size0	=	size0;
			p.Size1	=	size1;
		}


		protected void	SetupColor ( ref Particle p, Color4 color0, Color4 color1 ) 
		{
			p.Color0		=	color0;
			p.Color1		=	color1;
		}


		protected void	SetupColor ( ref Particle p, float intensity0, float intensity1, float alpha0, float alpha1 ) 
		{
			intensity0 *= alpha0;
			intensity1 *= alpha1;
			p.Color0		=	new Color4( intensity0, intensity0, intensity0, alpha0 );
			p.Color1		=	new Color4( intensity1, intensity1, intensity1, alpha1 );
		}


		protected void	SetupAngles	( ref Particle p, float angularLength )
		{
			float angle0	=	rand.NextFloat(0,1) * MathUtil.TwoPi;
			float angle1	=	angle0 + MathUtil.DegreesToRadians(angularLength) * (rand.NextFloat(0,1)>0.5 ? 1 : -1);
			p.Rotation0		=	angle0;
			p.Rotation1		=	angle1;
		} 

		protected void	SetupAnglesAbs	( ref Particle p, float angularLength )
		{
			p.Rotation0	=	rand.NextFloat(0,1) * MathUtil.TwoPi;
			p.Rotation1	=	p.Rotation0 + angularLength;
		} 

	}
}
