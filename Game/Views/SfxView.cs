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
using ShooterDemo.SFX;


namespace ShooterDemo.Views {
	public class SfxView : EntityView {

		SfxInstance sfx;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="game"></param>
		public SfxView ( Entity entity, World world, string sfxName ) : base(entity, world)
		{
			var fxID	=	World.GameClient.Atoms[ sfxName ];
			var	fxEvent	=	new FXEvent(fxID, entity.ID, entity.Position, entity.LinearVelocity, entity.Rotation);
			
			sfx = World.RunFX( fxEvent ); 
		}




		/// <summary>
		/// Updates visible meshes
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update ( float elapsedTime, float lerpFactor )
		{
			var p	=	Entity.LerpPosition( lerpFactor );
			var q   =	Entity.LerpRotation( lerpFactor );
			var v	=	Entity.LinearVelocity;
			sfx.Move( p,v,q );
		}



		/// <summary>
		/// Removes entity
		/// </summary>
		/// <param name="id"></param>
		public override void Killed ()
		{	
			sfx.Kill();
		}

	}
}
