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
	public class EdCamera {

		readonly RenderSystem rs;
		readonly Game game;


		public Vector3	Target		=	Vector3.Zero;
		public float	Distance	=	7.5f;
		public float	Yaw			=	45;
		public float	Pitch		=	45;
		public float	Fov			=	60;

		Manipulation	manipulation	=	Manipulation.None;
		Point			startPoint;
		float			addYaw;
		float			addPitch;
		float			addZoom = 1;



		/// <summary>
		/// 
		/// </summary>
		public EdCamera ( RenderSystem rs )
		{
			this.rs		=	rs;
			this.game	=	rs.Game;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update ( GameTime gameTime )
		{
			var yaw		=	MathUtil.DegreesToRadians( Yaw + addYaw );
			var pitch	=	MathUtil.DegreesToRadians( MathUtil.Clamp(Pitch + addPitch, -85, 85) );

			var offset	=	Matrix.RotationYawPitchRoll( yaw, pitch, 0 );

			var view	=	Matrix.LookAtRH( Target + offset.Backward * Distance * addZoom, Target, Vector3.Up );

			var vp		=	rs.DisplayBounds;

			var fovr	=	MathUtil.DegreesToRadians(Fov);

			var aspect	=	vp.Width / (float)vp.Height;

			rs.RenderWorld.Camera.SetupCameraFov( view, fovr, 0.125f, 4096, 1, 0, aspect );
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
