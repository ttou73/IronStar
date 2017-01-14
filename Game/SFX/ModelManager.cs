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
	public class ModelManager {

		LinkedList<ModelInstance> models;

		readonly Game			game;
		public readonly RenderSystem rs;
		public readonly RenderWorld	rw;
		public readonly SoundWorld	sw;
		public readonly GameWorld world;

		public ModelManager ( GameWorld world )
		{
			this.world	=	world;
			this.game	=	world.Game;
			this.rs		=	game.RenderSystem;
			this.rw		=	game.RenderSystem.RenderWorld;
			this.sw		=	game.SoundSystem.SoundWorld;

			Game_Reloading(this, EventArgs.Empty);
			game.Reloading +=	Game_Reloading;

			models	=	new LinkedList<ModelInstance>();
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Game_Reloading( object sender, EventArgs e )
		{
		}



		/// <summary>
		/// 
		/// </summary>
		public void Shutdown ()
		{
			KillAllModels();
			game.Reloading -= Game_Reloading;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="modelAtom"></param>
		/// <param name="entity"></param>
		/// <returns></returns>
		public ModelInstance AddModel ( short modelAtom, Entity entity )
		{
			var modelName	=	world.Atoms[modelAtom];

			var modelDesc	=	world.Content.Load<ModelDescriptor>( @"models\" + modelName );

			var scene		=	world.Content.Load<Scene>( modelDesc.ScenePath );

			var model		=	new ModelInstance( this, modelDesc, scene, entity );

			models.AddLast(model);

			return model;
		}



		/// <summary>
		/// 
		/// </summary>
		public void KillAllModels ()
		{
			foreach ( var model in models ) {
				model.Kill();
			}
			models.Clear();
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="elapsedTime"></param>
		/// <param name="lerpFactor"></param>
		public void Update ( float elapsedTime, float lerpFactor )
		{	
			models.RemoveAll( m => m.Killed );

			foreach ( var model in models ) {
				model.Update( elapsedTime, lerpFactor );
			}
		}

	}
}
