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
	public abstract class Manipulator {

		readonly protected RenderSystem rs;
		readonly protected Game game;
		readonly protected MapEditor editor;

		readonly protected Color SelectColor	=	new Color(255,211,149);
		readonly protected Color GridColor		=	new Color(64,64,64);

		protected const float ArrowSize = 5;

		protected float Scaling {
			get { return editor.camera.ManipulatorScaling; }
		}

		public abstract bool IsManipulating { get; }


		protected class IntersectResult {
			public IntersectResult ( bool hit, float fraction, float distance, Vector3 hitPoint ) 
			{
				Hit = hit; 
				Fraction = fraction; 
				Distance = distance;
				HitPoint = hitPoint;
			}
			public readonly bool Hit;
			public readonly Vector3 HitPoint;
			public readonly float Fraction;
			public readonly float Distance;
		}


		/// <summary>
		/// Constrcutor
		/// </summary>
		public Manipulator ( MapEditor editor )
		{
			this.rs		=	editor.Game.RenderSystem;
			this.game	=	editor.Game;
			this.editor	=	editor;
		}


		public abstract bool StartManipulation ( int x, int y );
		public abstract void UpdateManipulation ( int x, int y );
		public abstract void StopManipulation ( int x, int y );
		public abstract void Update ( GameTime gameTime, int x, int y );



		/// <summary>
		/// 
		/// </summary>
		/// <param name="dr"></param>
		/// <param name="dir"></param>
		/// <param name="color"></param>
		/// <param name="length"></param>
		/// <param name="scale"></param>
		protected void DrawArrow ( DebugRender dr, Ray pickRay, Vector3 origin, Vector3 dir, Color color )
		{
			var p0 = origin;
			var p1 = p0 + dir * Scaling * (ArrowSize - 0.75f);
			var p2 = p1 + dir * Scaling * (0.75f);

			var mp = game.Mouse.Position;

			var r = IntersectArrow( p0, dir, mp );

			if ( r.Hit ) {
				color = SelectColor;
				dr.DrawPoint( r.HitPoint, Scaling, color );
			} 

			dr.DrawLine(p0,p1, color, color, 2,2 );
			dr.DrawLine(p1,p2, color, color, 9,1 );
		}

		

		/// <summary>
		/// 
		/// </summary>
		/// <param name="origin"></param>
		/// <param name="dir"></param>
		/// <param name="pickPoint"></param>
		/// <param name="hitPoint"></param>
		/// <returns></returns>
		protected IntersectResult IntersectArrow ( Vector3 origin, Vector3 dir, Point pickPoint )
		{
			var arrowRay	=	new Ray( origin, dir * ArrowSize);
			var pickRay		=	editor.camera.PointToRay( pickPoint.X, pickPoint.Y );

			Vector3 temp, hitPoint;
			float t1, t2;

			var dist = Utils.RayIntersectsRay(ref pickRay, ref arrowRay, out temp, out hitPoint, out t1, out t2 );

			if ( (dist < 0.3f * Scaling) && (t2 > 0) && (t2 < 1) && (t1 > 0)) {
				return new IntersectResult( true, t1, dist, hitPoint );
			} else {
				return new IntersectResult( false, t1, dist, hitPoint );
			}
		}



		protected int PollIntersections ( params IntersectResult[] intersectionResults )
		{
			int index = -1;
			IntersectResult result = null;
						

			for ( int i=0; i<intersectionResults.Length; i++ ) {
				var intersection = intersectionResults[i];

				if (intersection.Hit) {

					if (result==null) {
						index  = i;
						result = intersection;
					} else {
						if (intersection.Fraction<result.Fraction) {
							result = intersection;
							index = i;
						}
					}
				}
			}

			return index;
		}
	}
}
