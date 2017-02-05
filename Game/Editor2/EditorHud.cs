using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Engine.Graphics;
using Fusion.Core.Mathematics;
using Fusion.Engine.Common;
using Fusion;
using Fusion.Core;

namespace IronStar.Editor2 {
	public class EditorHud : DisposableBase {

		readonly RenderSystem rs;
		readonly Game game;
		readonly MapEditor editor;

		SpriteLayer	spriteLayer;


		/// <summary>
		/// 
		/// </summary>
		/// <param name="rs"></param>
		/// <param name="editor"></param>
		public EditorHud ( MapEditor editor )
		{
			this.rs		=	editor.Game.RenderSystem;
			this.game	=	editor.Game;
			this.editor	=	editor;

			spriteLayer	=	new SpriteLayer( rs, 2048 );
			rs.SpriteLayers.Add( spriteLayer );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose( bool disposing )
		{
			if (disposing) {
				rs.SpriteLayers.Remove( spriteLayer );
				SafeDispose( ref spriteLayer );
			}
			base.Dispose( disposing );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update ( GameTime gameTime )
		{
			spriteLayer.Clear();

			var vp	= rs.DisplayBounds;

			spriteLayer.Draw(null, 0,0,vp.Width,44, new Color(64,64,64,192) );

			RText( 0, Color.Orange, "FPS = {0:0.00}", gameTime.Fps );
			RText( 1, Color.Orange, "RW Instances = {0}", rs.RenderWorld.Instances.Count );
			RText( 2, Color.Orange, "Entities = {0}", editor.World.entities.Count );

			if (editor.EnableSimulation) {
				RText( 3, Color.Red, "SIMULATION MODE" );
			} else {
				RText( 3, Color.Lime, "EDITOR MODE" );
			}

			LText( 0, Color.LightGray, "[F1] - Dashboard     [Q] - Select   ");
			LText( 1, Color.LightGray, "[F2] - Save Map      [W] - Move     ");
			LText( 2, Color.LightGray, "[F5] - Build Content [E] - Rotate   ");
			LText( 3, Color.LightGray, "                     [R] - Scale    ");


			var mp = game.Mouse.Position;

			if (editor.manipulator.IsManipulating) {
				var text = editor.manipulator.ManipulationText;
				var len  = Math.Max(16,text.Length);
				spriteLayer.Draw( null, mp.X, mp.Y - 16 + 48, len*8+16, 16, new Color(0,0,0,128) );
				spriteLayer.DrawDebugString( mp.X+4, mp.Y - 12 + 48, editor.manipulator.ManipulationText, Color.Yellow );
			}

			//spriteLayer.Draw( null, editor.SelectionMarquee, new Color(51,153,255,128) );
			spriteLayer.Draw( null, editor.SelectionMarquee, new Color(82,133,166,128) );
		}



		public void RText ( int line, Color color, string format, params object[] args )
		{
			var vp	= rs.DisplayBounds;
			var s	= string.Format(format, args);
			int x	= vp.Width - s.Length * 8 - 4;
			int y	= line * 9 + 4;
			
			//spriteLayer.DrawDebugString( x+1, y+1, s, Color.Black );
			spriteLayer.DrawDebugString( x+0, y+0, s, color );
		}


		public void LText ( int line, Color color, string format, params object[] args )
		{
			var vp	= rs.DisplayBounds;
			var s	= string.Format(format, args);
			int x	= 4;
			int y	= line * 9 + 4;
			
			//spriteLayer.DrawDebugString( x+1, y+1, s, Color.Black );
			spriteLayer.DrawDebugString( x+0, y+0, s, color );
		}
	}
}
