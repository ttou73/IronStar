using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Engine.Graphics;
using Fusion.Core.Mathematics;
using Fusion.Engine.Common;
using Fusion;

namespace IronStar.Editor2 {
	public class EditorCamera {

		readonly RenderSystem rs;
		readonly Game game;
		readonly MapEditor editor;


		public Vector3	Target		=	Vector3.Zero;
		public float	Distance	=	30;
		public float	Yaw			=	45;
		public float	Pitch		=	-30;
		public float	Fov			=	60;

		Manipulation	manipulation	=	Manipulation.None;
		Point			startPoint;
		float			addYaw;
		float			addPitch;
		float			addZoom = 1;


		public Manipulation Manipulation {
			get {
				return manipulation;
			}
		}


		/// <summary>
		/// 
		/// </summary>
		public EditorCamera ( MapEditor editor )
		{
			this.rs		=	editor.Game.RenderSystem;
			this.game	=	editor.Game;
			this.editor	=	editor;
		}



		public float PixelToWorldSize ( Vector3 point, float pixelSize )
		{
			var view	=	GetViewMatrix();
			var tpoint	=	Vector3.TransformCoordinate( point, view );
			var fovTan	=	(float)Math.Tan(MathUtil.DegreesToRadians(Fov/2));
			var vp		=	rs.DisplayBounds;

			return 2 * pixelSize / vp.Height * fovTan * Math.Abs(tpoint.Z);
		}



		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		Matrix GetViewMatrix ()
		{
			var yaw		=	MathUtil.DegreesToRadians( Yaw + addYaw );
			var pitch	=	MathUtil.DegreesToRadians( MathUtil.Clamp(Pitch + addPitch, -85, 85) );

			var offset	=	Matrix.RotationYawPitchRoll( yaw, pitch, 0 );

			var view	=	Matrix.LookAtRH( Target + offset.Backward * Distance * addZoom, Target, Vector3.Up );

			return view;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update ( GameTime gameTime )
		{
			var view	=	GetViewMatrix();

			var vp		=	rs.DisplayBounds;

			var fovr	=	MathUtil.DegreesToRadians(Fov);

			var aspect	=	vp.Width / (float)vp.Height;

			rs.RenderWorld.Camera.SetupCameraFov( view, fovr, 0.125f, 4096, 1, 0, aspect );
		}



		public Ray PointToRay ( int x, int y )
		{
			var vp	=	rs.DisplayBounds;
			float fx = (x) / (float)(vp.Width);
			float fy = (y) / (float)(vp.Height);

			//Log.Message("{0} {1}", fx, fy );

			//var ray = new 
			var c = rs.RenderWorld.Camera.Frustum.GetCorners();

			var n0	=	Vector3.Lerp( c[1], c[0], fy );
			var n1	=	Vector3.Lerp( c[2], c[3], fy );
			var n	=	Vector3.Lerp( n1, n0, fx );

			var f0	=	Vector3.Lerp( c[5], c[4], fy );
			var f1	=	Vector3.Lerp( c[6], c[7], fy );
			var f	=	Vector3.Lerp( f1, f0, fx );

			return new Ray( n, (f - n ).Normalized() );
		}



		public void StartManipulation ( int x, int y, Manipulation manipulation )
		{
			this.startPoint		=	new Point( x, y );
			this.manipulation	=	manipulation;
		}


		public void UpdateManipulation ( int x, int y )
		{
			if (manipulation==Manipulation.Rotating) {
				addYaw		=	(startPoint.X - x) / 2.5f;
				addPitch	=	(startPoint.Y - y) / 2.5f;
			} else if (manipulation==Manipulation.Zooming) {
				addZoom		=	(float)Math.Pow( 2, (startPoint.Y - y + startPoint.X - x)/320.0 );
			}
		}


		public void StopManipulation ( int x, int y )
		{
			manipulation	=	Manipulation.None;

			Yaw			=	Yaw + addYaw;
			Pitch		=	MathUtil.Clamp(Pitch + addPitch, -85, 85);
			Distance	=	Distance * addZoom;

			addYaw		=	0;
			addPitch	=	0;	
			addZoom		=	1;
		}

	}
}
