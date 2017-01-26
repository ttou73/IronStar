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
	public class RotateTool : Manipulator {


		/// <summary>
		/// 
		/// </summary>
		public RotateTool ( MapEditor editor ) : base(editor)
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
			var ray			= editor.camera.PointToRay( x, y );


			if (!manipulating) {
				var hitX	=	IntersectRing( target.Transform.Translation, Vector3.UnitX, mp );
				var hitY	=	IntersectRing( target.Transform.Translation, Vector3.UnitY, mp );
				var hitZ	=	IntersectRing( target.Transform.Translation, Vector3.UnitZ, mp );

				DrawRing( dr, ray, origin, Vector3.UnitX,  hitX.Hit ? SelectColor : Color.Red  );
				DrawRing( dr, ray, origin, Vector3.UnitY,  hitY.Hit ? SelectColor : Color.Lime );
				DrawRing( dr, ray, origin, Vector3.UnitZ,  hitZ.Hit ? SelectColor : Color.Blue );
			} else {

				DrawRing( dr, ray, origin, direction, SelectColor );

				var vecSize	=	editor.camera.PixelToWorldSize(origin, 110);

				dr.DrawLine( origin, origin + vector0 * vecSize, SelectColor, SelectColor, 2, 2 );
				dr.DrawLine( origin, origin + vector1 * vecSize, SelectColor, SelectColor, 2, 2 );
			}
		}




		public override bool IsManipulating {
			get {
				return manipulating;
			}
		}


		public override string ManipulationText {
			get {
				return string.Format("{0:0.00}", MathUtil.RadiansToDegrees( angle ));
			}
		}



		bool	manipulating;
		Vector3 direction;
		Vector3 initialPoint;
		Vector3 currentPoint;

		Vector3 vector0, vector1;
		float	angle;

		SnapMode	snapMode;
		float		snapValue;


		MapFactory[] targets  = null;
		Quaternion[] initRots = null;


		public override bool StartManipulation ( int x, int y )
		{
			if (!editor.Selection.Any()) {
				return false;
			}

			snapMode	=	editor.Config.RotateToolSnapMode;
			snapValue	=	editor.Config.RotateToolSnapValue;
			angle		=	0;
			vector0		=	Vector3.Zero;
			vector1		=	Vector3.Zero;

			targets		=	editor.GetSelection();
			initRots	=	targets.Select( t => t.Transform.Rotation ).ToArray();

			var origin	=	targets.Last().Transform.Translation;
			var mp		=	new Point( x, y );


			var intersectX	=	IntersectRing( origin, Vector3.UnitX, mp );
			var intersectY	=	IntersectRing( origin, Vector3.UnitY, mp );
			var intersectZ	=	IntersectRing( origin, Vector3.UnitZ, mp );

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

				var origin	=	targets.Last().Transform.Translation;
				var mp		=	new Point( x, y );

				var result	=	IntersectRing( origin, direction, mp );
				
				currentPoint	=	result.HitPoint;

				vector0			=	(initialPoint - origin).Normalized();
				vector1			=	(currentPoint - origin).Normalized();

				var sine		=	Vector3.Dot( direction, Vector3.Cross( vector0, vector1 ) );
				var cosine		=	Vector3.Dot( vector0, vector1 );

				angle			=	(float)Math.Atan2( sine, cosine );

				if (snapMode==SnapMode.Relative || snapMode==SnapMode.Absolute) {
					angle		=	Snap( angle, MathUtil.DegreesToRadians( editor.Config.RotateToolSnapValue ) );
				}

				for ( int i=0; i<targets.Length; i++) {
					var target	=	targets[i];
					var rot		=	initRots[i];

					var addRot	=	Quaternion.RotationAxis( direction, angle );

					target.Transform.Rotation = addRot * rot;
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
