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
using BEPUphysics.BroadPhaseEntries;

namespace IronStar.SFX {
	public class DecalManager : DisposableBase {

		LinkedList<DecalInstance> decals = new LinkedList<DecalInstance>();

		readonly Game			game;
		public readonly RenderSystem rs;
		public readonly RenderWorld	rw;
		public readonly SoundWorld	sw;
		public readonly GameWorld world;

		TextureAtlas decalAtlas;


		public DecalManager ( GameWorld world )
		{
			this.world	=	world;
			this.game	=	world.Game;
			this.rs		=	game.RenderSystem;
			this.rw		=	game.RenderSystem.RenderWorld;
			this.sw		=	game.SoundSystem.SoundWorld;

			Game_Reloading(this, EventArgs.Empty);
			game.Reloading +=	Game_Reloading;

			var decal				=	new Decal();

			decal.DecalMatrix		=	Matrix.RotationX( MathUtil.Pi/2 );
			
			decal.Emission			=	new Color4( 500,200,50,1 );
			decal.BaseColor			=	new Color4( 1,0,0,1 );
			
			decal.Metallic			=	0;
			decal.Roughness			=	0.5f;
			decal.ImageRectangle	=	new Rectangle(0,0,1,1);

			decal.ColorFactor		=	1;
			decal.SpecularFactor	=	1;
			decal.NormalMapFactor	=	1;
			decal.FalloffFactor		=	0.5f;

			rw.LightSet.Decals.Add( decal );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Rectangle GetImageRectangleByName ( string name )
		{
			return decalAtlas[name];
		}



		/// <summary>
		/// 
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if (disposing) {
				//KillAllModels();
				game.Reloading -= Game_Reloading;
			}
			base.Dispose( disposing );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Game_Reloading( object sender, EventArgs e )
		{
			decalAtlas				=	world.Content.Load<TextureAtlas>(@"decals\decals");
			rw.LightSet.DecalAtlas	=	decalAtlas;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="decalAtom"></param>
		/// <param name="entity"></param>
		/// <returns></returns>
		public DecalInstance AddDecal ( short decalAtom, Entity entity )
		{
			var decalName	=	world.Atoms[decalAtom];

			var decalFact	=	world.Content.Load<DecalFactory>( @"decals\" + decalName );

			var decal		=	new DecalInstance( this, decalFact, entity );

			decals.AddLast( decal );

			return decal;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="elapsedTime"></param>
		/// <param name="lerpFactor"></param>
		public void Update ( float elapsedTime, float lerpFactor )
		{	
			foreach ( var decal in decals ) {
				decal.Update( elapsedTime, lerpFactor );
			}
		}


	}
}
