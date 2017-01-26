using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Fusion;
using Fusion.Core.Mathematics;
using Fusion.Engine.Common;
using Fusion.Core.Content;
using Fusion.Engine.Server;
using Fusion.Engine.Client;
using Fusion.Core.Extensions;
using IronStar.SFX;
using Fusion.Core.IniParser.Model;
using IronStar.Views;
using Fusion.Engine.Graphics;
using IronStar.Mapping;
using IronStar.Editors;
using Fusion.Engine.Frames;
using Fusion.Engine.Input;

namespace IronStar.Editor2 {

	/// <summary>
	/// World represents entire game state.
	/// </summary>
	public partial class MapEditor : IEditorInstance {

		/// <summary>
		/// 
		/// </summary>
		void SetupUI ()
		{
			var rootFrame = Game.Frames.RootFrame;

			rootFrame.MouseDown += RootFrame_MouseDown;
			rootFrame.MouseMove	+= RootFrame_MouseMove;
			rootFrame.MouseUp	+= RootFrame_MouseUp;

			rootFrame.Click      +=RootFrame_Click;

			Game.Keyboard.KeyDown +=Keyboard_KeyDown;

		}

		private void Keyboard_KeyDown( object sender, KeyEventArgs e )
		{
			if (e.Key==Keys.F) {
				Focus();
			}
			if (!manipulator.IsManipulating) {
				if (e.Key==Keys.Q) {
					manipulator = new NullTool(this);
				}
				if (e.Key==Keys.W) {
					manipulator = new MoveTool(this);
				}
				if (e.Key==Keys.E) {
					manipulator = new RotateTool(this);
				}
				if (e.Key==Keys.R) {
					manipulator = new MoveTool(this);
				}
			}
		}


		private void RootFrame_Click( object sender, Frame.MouseEventArgs e )
		{
			if (camera.Manipulation==Manipulation.None && !manipulator.IsManipulating) {
				var shift =	Game.Keyboard.IsKeyDown(Keys.LeftShift) || Game.Keyboard.IsKeyDown(Keys.RightShift);
				Select( e.X, e.Y, shift );
			}
		}

		private void RootFrame_MouseDown( object sender,  Frame.MouseEventArgs e )
		{
			if (Game.Keyboard.IsKeyDown(Keys.LeftAlt)) {
				if (e.Key==Keys.LeftButton) {
					camera.StartManipulation( e.X, e.Y, Manipulation.Rotating );
				} else
				if (e.Key==Keys.RightButton) {
					camera.StartManipulation( e.X, e.Y, Manipulation.Zooming );
				} else 
				if (e.Key==Keys.MiddleButton) {
					camera.StartManipulation( e.X, e.Y, Manipulation.Translating );
				} else {
					camera.StartManipulation( e.X, e.Y, Manipulation.None );
				}
			} else {
				if (!manipulator.StartManipulation( e.X, e.Y )) {
					StartMarqueeSelection( e.X, e.Y );
				}
			}
		}

		private void RootFrame_MouseMove( object sender, Frame.MouseEventArgs e )
		{
			camera.UpdateManipulation( e.X, e.Y );
			manipulator.UpdateManipulation( e.X, e.Y );
			UpdateMarqueeSelection( e.X, e.Y );
		}

		private void RootFrame_MouseUp( object sender, Frame.MouseEventArgs e )
		{
			camera.StopManipulation( e.X, e.Y );
			manipulator.StopManipulation( e.X, e.Y );
			StopMarqueeSelection( e.X, e.Y );
		}
	}
}
