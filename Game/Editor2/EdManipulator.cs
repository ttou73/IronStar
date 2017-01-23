using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Engine.Graphics;
using Fusion.Core.Mathematics;
using Fusion.Engine.Common;
using Fusion;
using IronStar.Mapping;

namespace IronStar.Editor2 {
	public class EdManipulator {

		readonly RenderSystem rs;
		readonly Game game;
		readonly MapEditor editor;


		public ManipulatorMode Mode { get; set; }

		public MapFactory	Target { get; set; }


		/// <summary>
		/// 
		/// </summary>
		public EdManipulator ( RenderSystem rs, MapEditor editor )
		{
			this.rs		=	rs;
			this.game	=	rs.Game;
			this.editor	=	editor;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update ( GameTime gameTime )
		{
			var dr = rs.RenderWorld.Debug;

			if (Target==null) {
				return;
			}

			var scale = editor.edCamera.ManipulatorScaling;

			if (Mode==ManipulatorMode.TranslationGlobal) {
				DrawArrow( dr, Vector3.UnitX, Color.Red , scale );
				DrawArrow( dr, Vector3.UnitY, Color.Lime, scale );
				DrawArrow( dr, Vector3.UnitZ, Color.Blue, scale );
			}
		}



		void DrawArrow ( DebugRender dr, Vector3 dir, Color color, float scale )
		{
			var p0 = Target.Transform.Translation;
			var p1 = p0 + dir * scale;

			dr.DrawLine(p0,p1, color, color, 5,5 );
			dr.DrawLine(p1,p1+dir, color, color, 15,0 );
		}




		public void StartManipulation ( int x, int y )
		{
			//this.startPoint		=	new Point( x, y );
			//this.manipulation	=	manipulation;
		}


		public void UpdateManipulation ( int x, int y )
		{
			//if (manipulation==Manipulation.Rotating) {
			//	addYaw		=	(startPoint.X - x) / 2.5f;
			//	addPitch	=	(startPoint.Y - y) / 2.5f;
			//} else if (manipulation==Manipulation.Zooming) {
			//	addZoom		=	(float)Math.Pow( 2, (startPoint.Y - y + startPoint.X - x)/320.0 );
			//}
		}


		public void StopManipulation ( int x, int y )
		{
			//manipulation	=	Manipulation.None;

			//Yaw			=	Yaw + addYaw;
			//Pitch		=	MathUtil.Clamp(Pitch + addPitch, -85, 85);
			//Distance	=	Distance * addZoom;

			//addYaw		=	0;
			//addPitch	=	0;	
			//addZoom		=	1;
		}

	}
}
