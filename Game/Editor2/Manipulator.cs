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

		public abstract bool IsManipulating { get; }


		protected class IntersectResult {
			public IntersectResult ( bool hit, float pickDistance, float distance, Vector3 hitPoint ) 
			{
				Hit = hit; 
				PickDistance = pickDistance; 
				Distance	= distance;
				HitPoint	= hitPoint;
			}
			public readonly bool Hit;
			public readonly Vector3 HitPoint;
			public readonly float PickDistance;
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
		public abstract string ManipulationText { get; }



		/// <summary>
		/// Draw standard arrow.
		/// </summary>
		/// <param name="dr"></param>
		/// <param name="dir"></param>
		/// <param name="color"></param>
		/// <param name="length"></param>
		/// <param name="scale"></param>
		protected void DrawArrow ( DebugRender dr, Ray pickRay, Vector3 origin, Vector3 dir, Color color )
		{
			var p0 = origin;
			var p1 = p0 + dir * editor.camera.PixelToWorldSize( origin, 90 );
			var p2 = p1 + dir * editor.camera.PixelToWorldSize( origin, 20 );

			var mp = game.Mouse.Position;

			var r = IntersectArrow( p0, dir, mp );

			if ( r.Hit ) {
				color = SelectColor;
				var sz = editor.camera.PixelToWorldSize( origin, 10 );
				dr.DrawPoint( r.HitPoint, sz, color );
			} 

			dr.DrawLine(p0,p1, color, color, 2,2 );
			dr.DrawLine(p1,p2, color, color, 9,1 );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="dr"></param>
		/// <param name="pickRay"></param>
		/// <param name="origin"></param>
		/// <param name="axisA"></param>
		/// <param name="axisB"></param>
		/// <param name="color"></param>
		protected void DrawRing ( DebugRender dr, Ray pickRay, Vector3 origin, Vector3 axis, Color color )
		{
				axis	=	Vector3.Normalize( axis );
			var axisA	=	Vector3.Cross( axis, Vector3.Up );	

			if (axisA.LengthSquared()<0.001f) {
				axisA	=	Vector3.Cross( axis, Vector3.Right );
			}

			var axisB	=	Vector3.Cross( axisA, axis );

			int N = 64;
			Vector3[] points = new Vector3[N + 1];

			var radius = editor.camera.PixelToWorldSize(origin, 90);

			for (int i = 0; i <= N; i++)
			{
				var p = origin;
				p += axisA * radius * (float)Math.Cos(Math.PI * 2 * i / N);
				p += axisB * radius * (float)Math.Sin(Math.PI * 2 * i / N);
				points[i] = p;
			}

			for (int i = 0; i < N; i++)
			{
				dr.DrawLine(points[i], points[i + 1], color, color, 2, 2);
			}
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
			var length		=	editor.camera.PixelToWorldSize(origin, 110);
			var tolerance	=	editor.camera.PixelToWorldSize(origin, 7);
			var arrowRay	=	new Ray( origin, dir * length);
			var pickRay		=	editor.camera.PointToRay( pickPoint.X, pickPoint.Y );

			Vector3 temp, hitPoint;
			float t1, t2;

			var dist = Utils.RayIntersectsRay(ref pickRay, ref arrowRay, out temp, out hitPoint, out t1, out t2 );

			var pickDistance = Vector3.Distance( hitPoint, pickRay.Position );

			if ( (dist < tolerance) && (t2 > 0) && (t2 < 1) && (t1 > 0)) {
				return new IntersectResult( true, pickDistance, dist, hitPoint );
			} else {
				return new IntersectResult( false, pickDistance, dist, hitPoint );
			}
		}




		/// <summary>
		/// 
		/// </summary>
		/// <param name="origin"></param>
		/// <param name="dir"></param>
		/// <param name="pickPoint"></param>
		/// <param name="hitPoint"></param>
		/// <returns></returns>
		protected IntersectResult IntersectRing ( Vector3 origin, Vector3 axis, Point pickPoint )
		{
			var radius		=	editor.camera.PixelToWorldSize(origin, 90);
			var tolerance	=	editor.camera.PixelToWorldSize(origin, 7);
			var pickRay		=	editor.camera.PointToRay( pickPoint.X, pickPoint.Y );

			var plane		=	new Plane( origin, axis );

			Vector3 hitPoint;

			if ( plane.Intersects( ref pickRay, out hitPoint ) ) {

				var originHitPointDistance	=	Vector3.Distance( origin, hitPoint );
				var pickDistance			=	Vector3.Distance( hitPoint, pickRay.Position );

				var hitRing	=	(originHitPointDistance > radius - tolerance) && (originHitPointDistance < radius + tolerance);

				return new IntersectResult( hitRing, pickDistance, 0, hitPoint );
				
			} else {
				return new IntersectResult( false, float.PositiveInfinity, float.PositiveInfinity, Vector3.Zero );
			}

		}



		/// <summary>
		/// Gets index of closest intersection.
		/// </summary>
		/// <param name="intersectionResults"></param>
		/// <returns></returns>
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
						if (intersection.PickDistance < result.PickDistance) {
							result = intersection;
							index = i;
						}
					}
				}
			}

			return index;
		}



		public float Snap ( float value, float snapValue )
		{
			return (float)(Math.Round( value / snapValue ) * snapValue);
		}


		public Vector3 Snap ( Vector3 value, float snapValue )
		{
			return new Vector3( Snap( value.X, snapValue ), Snap( value.Y, snapValue ), Snap( value.Z, snapValue ) );
		}
	}
}
