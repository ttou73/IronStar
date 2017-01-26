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
	public class MoveTool : Manipulator {


		/// <summary>
		/// 
		/// </summary>
		public MoveTool ( MapEditor editor ) : base(editor)
		{
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update ( GameTime gameTime, int x, int y )
		{
			var dr = rs.RenderWorld.Debug;
			var mp = game.Mouse.Position;

			if (!editor.Selection.Any()) {
				return;
			}

			var target		= editor.Selection.Last();
			var origin		= target.Transform.Translation;

			var linerSize	= editor.camera.PixelToWorldSize( origin, 5 );

			var ray = editor.camera.PointToRay( x, y );

			if (manipulating) {
				DrawArrow( dr, ray, origin, direction, SelectColor  );

				dr.DrawPoint(initialPoint, linerSize, GridColor);
				dr.DrawPoint(currentPoint, linerSize, GridColor);
				dr.DrawLine(initialPoint, currentPoint, GridColor);
			} else {
				DrawArrow( dr, ray, origin, Vector3.UnitX, Color.Red  );
				DrawArrow( dr, ray, origin, Vector3.UnitY, Color.Lime );
				DrawArrow( dr, ray, origin, Vector3.UnitZ, Color.Blue );
			}
		}


		public override bool IsManipulating {
			get {
				return manipulating;
			}
		}


		bool	manipulating;
		Vector3 direction;
		Vector3 initialPoint;
		Vector3 currentPoint;

		SnapMode	snapMode;
		float		snapValue;

		MapFactory[] targets = null;
		Vector3[] initPos = null;


		public override bool StartManipulation ( int x, int y )
		{
			if (!editor.Selection.Any()) {
				return false;
			}

			snapMode	=	editor.Config.MoveToolSnapMode;
			snapValue	=	editor.Config.MoveToolSnapValue;

			targets	=	editor.GetSelection();
			initPos	=	targets.Select( t => t.Transform.Translation ).ToArray();

			var origin	=	initPos.Last();
			var mp		=	new Point( x, y );


			var intersectX	=	IntersectArrow( origin, Vector3.UnitX, mp );
			var intersectY	=	IntersectArrow( origin, Vector3.UnitY, mp );
			var intersectZ	=	IntersectArrow( origin, Vector3.UnitZ, mp );

			var index		=	PollIntersections( intersectX, intersectY, intersectZ );

			if (index<0) {
				return false;
			}

			if (index==0) {		
				manipulating	=	true;
				direction		=	Vector3.UnitX;
				initialPoint	=	intersectX.HitPoint;
				currentPoint	=	intersectX.HitPoint;
				return true;
			}

			if (index==1) {		
				manipulating	=	true;
				direction		=	Vector3.UnitY;
				initialPoint	=	intersectY.HitPoint;
				currentPoint	=	intersectY.HitPoint;
				return true;
			}

			if (index==2) {		
				manipulating	=	true;
				direction		=	Vector3.UnitZ;
				initialPoint	=	intersectZ.HitPoint;
				currentPoint	=	intersectZ.HitPoint;
				return true;
			}
			
			return false;
		}


		public override void UpdateManipulation ( int x, int y )
		{
			if (manipulating) {

				var origin	=	initialPoint;
				var mp		=	new Point( x, y );

				var result	=	IntersectArrow( origin, direction, mp );
				
				currentPoint	= result.HitPoint;

				for ( int i=0; i<targets.Length; i++) {
					var target	= targets[i];
					var pos		= initPos[i];

					if (snapMode==SnapMode.Absolute) {
						target.Transform.Translation = Snap( pos + (currentPoint - initialPoint), snapValue );
					} else {
						target.Transform.Translation = pos + (currentPoint - initialPoint);
					}
				}
			}
		}


		public override void StopManipulation ( int x, int y )
		{
			if (manipulating) {
				manipulating	=	false;
			}
		}

	}
}
