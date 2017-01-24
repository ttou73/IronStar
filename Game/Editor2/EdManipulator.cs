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

		readonly Color SelectColor	=	new Color(255,211,149);
		readonly Color GridColor	=	new Color(64,64,64);

		const float ArrowSize = 5;

		float Scaling {
			get { return editor.edCamera.ManipulatorScaling; }
		}


		public bool IsManipulating {
			get { return manipulating; }
		}


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
			var mp = game.Mouse.Position;

			if (Target==null) {
				return;
			}

			var scale = editor.edCamera.ManipulatorScaling;

			var ray = editor.edCamera.PointToRay( mp.X, mp.Y );

			if (manipulating) {
				DrawArrow( dr, ray, direction, SelectColor  );

				dr.DrawPoint(initialPoint, Scaling * 0.25f, GridColor);
				dr.DrawPoint(currentPoint, Scaling * 0.25f, GridColor);
				dr.DrawLine(initialPoint, currentPoint, GridColor);
			} else {
				DrawArrow( dr, ray, Vector3.UnitX, Color.Red  );
				DrawArrow( dr, ray, Vector3.UnitY, Color.Lime );
				DrawArrow( dr, ray, Vector3.UnitZ, Color.Blue );
			}
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="dr"></param>
		/// <param name="dir"></param>
		/// <param name="color"></param>
		/// <param name="length"></param>
		/// <param name="scale"></param>
		void DrawArrow ( DebugRender dr, Ray pickRay, Vector3 dir, Color color )
		{
			var p0 = Target.Transform.Translation;
			var p1 = p0 + dir * Scaling * (ArrowSize - 0.75f);
			var p2 = p1 + dir * Scaling * (0.75f);

			var mp = game.Mouse.Position;
			Vector3 hitPoint;

			var t = IntersectArrow( p0, dir, mp, out hitPoint );

			if ( t > 0 ) {
				color = SelectColor;
				dr.DrawPoint( hitPoint, Scaling, color );
			} 


			dr.DrawLine(p0,p1, color, color, 2,2 );
			dr.DrawLine(p1,p2, color, color, 9,1 );
		}

		

		float IntersectArrow ( Vector3 origin, Vector3 dir, Point pickPoint, out Vector3 hitPoint )
		{
			var arrowRay	=	new Ray( origin, dir * ArrowSize);
			var pickRay		=	editor.edCamera.PointToRay( pickPoint.X, pickPoint.Y );

			Vector3 temp;
			float t1, t2;

			var dist = Utils.RayIntersectsRay(ref pickRay, ref arrowRay, out temp, out hitPoint, out t1, out t2 );

			if ( (dist < 0.2f * Scaling) && (t2 > 0) && (t2 < 1) && (t1 > 0)) {
				return t1;
			} else {
				return -1;
			}
		}



		void DrawCircle ( DebugRender dr, Color color, float radius, float scale )
		{
			var pos =	Target.Transform.Translation;
			var r	=	radius * scale;
			dr.DrawRing( Target.Transform.World, r, color, 32 );
			dr.DrawRing( Target.Transform.World, r, color, 32 );
		}



		bool	manipulating;
		Vector3 direction;
		Vector3 initialPoint;
		Vector3 currentPoint;
		Vector3 initTranslation;


		public bool StartManipulation ( int x, int y )
		{
			if (Target==null) {
				return false;
			}

			initTranslation	=	Target.Transform.Translation;

			var origin	=	Target.Transform.Translation;
			var mp		=	new Point( x, y );

			Vector3 hpx, hpy, hpz;

			var tx	=	IntersectArrow( origin, Vector3.UnitX, mp, out hpx );
			var ty	=	IntersectArrow( origin, Vector3.UnitY, mp, out hpy );
			var tz	=	IntersectArrow( origin, Vector3.UnitZ, mp, out hpz );

			if (tx>0) {		
				manipulating	=	true;
				direction		=	Vector3.UnitX;
				initialPoint	=	hpx;
				currentPoint	=	initialPoint;
				return true;
			}

			if (ty>0) {		
				manipulating	=	true;
				direction		=	Vector3.UnitY;
				initialPoint	=	hpy;
				currentPoint	=	initialPoint;
				return true;
			}

			if (tz>0) {		
				manipulating	=	true;
				direction		=	Vector3.UnitZ;
				initialPoint	=	hpz;
				currentPoint	=	initialPoint;
				return true;
			}
			
			return false;
		}


		public void UpdateManipulation ( int x, int y )
		{
			if (manipulating) {
				var origin	=	initialPoint;
				var mp		=	new Point( x, y );
				var hit		=	Vector3.Zero;

				IntersectArrow( origin, direction, mp, out hit );
				
				currentPoint = hit;

				Target.Transform.Translation	=	initTranslation + (hit - initialPoint);
			}
		}


		public void StopManipulation ( int x, int y )
		{
			if (manipulating) {
				manipulating	=	false;

				var origin	=	initialPoint;
				var mp		=	new Point( x, y );
				var hit		=	Vector3.Zero;

				IntersectArrow( origin, direction, mp, out hit );
				
				currentPoint = hit;

				Target.Transform.Translation	=	initTranslation + (hit - initialPoint);
			}
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
