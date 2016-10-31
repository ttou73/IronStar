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

		/// <summary>
		/// Represents SfxInstance stage.
		/// </summary>
		protected abstract class Stage {

			public readonly SfxInstance SfxInstance;
			
			/// <summary>
			/// 
			/// </summary>
			/// <param name="sfxInstance"></param>
			public Stage ( SfxInstance sfxInstance )
			{
				this.SfxInstance	=	sfxInstance;
			}


			/// <summary>
			/// Updates internal stage state.
			/// </summary>
			/// <param name="dt"></param>
			public abstract void Update ( float dt, FXEvent fxEvent );

			/// <summary>
			/// Request stage to be stopped.
			/// If immidiate equals true the stage should be stopped immediatly.
			/// </summary>
			/// <param name="immediate"></param>
			public abstract void Stop ( bool immediate );

			/// <summary>
			/// Indicates whethe stage is worked out.
			/// </summary>
			/// <returns></returns>
			public abstract bool IsExhausted ();

			/// <summary>
			/// Kills all associated with fx stage stuff
			/// </summary>
			public abstract void Kill ();
		}

	}
}
