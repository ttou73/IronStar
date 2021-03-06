﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Core.Mathematics;
using Fusion.Drivers.Graphics;
using Fusion.Drivers.Graphics.Display;
using Fusion.Drivers.Input;
using Fusion.Engine.Common;

namespace Fusion.Engine.Graphics {
	public class Camera {
		
		private Matrix	cameraMatrix	;
		private Matrix	cameraMatrixL	;
		private Matrix	cameraMatrixR	;
		private Matrix	viewMatrix		;
		private Matrix	viewMatrixL		;
		private Matrix	viewMatrixR		;
		private Matrix	projMatrix		;
		private Matrix	projMatrixL		;
		private Matrix	projMatrixR		;

		private float	projZNear;
		private float	projZFar;


		/// <summary>
		/// 
		/// </summary>
		public Camera ()
		{
			SetupCameraFov(Matrix.Identity, MathUtil.DegreesToRadians(90), 0.125f, 1024f, 1, 0, 1 );
		}



		/// <summary>
		/// Sets camera up.
		/// </summary>
		/// <param name="viewMatrix">View matrix. The left-eye and right-eye view matricies will be constructed from this matrix.</param>
		/// <param name="height">Frustum with at near plane.</param>
		/// <param name="width">Frustum height ar near place.</param>
		/// <param name="near">Camera near clipping plane distance.</param>
		/// <param name="far">Camera far clipping plane distance.</param>
		/// <param name="convergenceDistance">Stereo convergence distance. </param>
		/// <param name="separation">Stereo separation or distance between eyes.</param>
		public void SetupCamera ( Matrix viewMatrix, float height, float width, float near, float far, float convergence, float separation )
		{
			if (convergence<=0) {	
				throw new ArgumentOutOfRangeException("convergence must be > 0");
			}

			projZNear			=	near;
			projZFar			=	far;

			float offset		=	separation / convergence * near / 2;
			float nearHeight	=	height;
			float nearWidth		=	width;

			//	Projection :
			this.projMatrix		=	Matrix.PerspectiveOffCenterRH( -nearWidth/2, nearWidth/2, -nearHeight/2, nearHeight/2, near, far );
			this.projMatrixR	=	Matrix.PerspectiveOffCenterRH( -nearWidth/2 - offset, nearWidth/2 - offset, -nearHeight/2, nearHeight/2, near, far );
			this.projMatrixL	=	Matrix.PerspectiveOffCenterRH( -nearWidth/2 + offset, nearWidth/2 + offset, -nearHeight/2, nearHeight/2, near, far );
																		
			//	View :
			this.viewMatrix		=	viewMatrix;
			this.viewMatrixL	=	viewMatrix	*	Matrix.Translation( Vector3.UnitX * separation / 2 );
			this.viewMatrixR	=	viewMatrix	*	Matrix.Translation( -Vector3.UnitX * separation / 2 );

			//	Camera :
			this.cameraMatrix	=	Matrix.Invert( viewMatrix );
			this.cameraMatrixL	=	Matrix.Invert( viewMatrixL );
			this.cameraMatrixR	=	Matrix.Invert( viewMatrixR );


			#if OCULUS
			if (Game.Instance.GraphicsDevice.Display is OculusRiftDisplay) {
				
				if (OculusRiftSensors.LeftEye != null && OculusRiftSensors.RightEye != null) {
					projMatrixL = OculusRiftSensors.LeftEye.Projection;
					projMatrixR = OculusRiftSensors.RightEye.Projection;


					var leftEyePos	= new Vector3(OculusRiftSensors.LeftEye.Position.X, -OculusRiftSensors.LeftEye.Position.Y, -OculusRiftSensors.LeftEye.Position.Z)	;
					var rightEyePos = new Vector3(OculusRiftSensors.RightEye.Position.X, -OculusRiftSensors.RightEye.Position.Y, -OculusRiftSensors.RightEye.Position.Z);

					var leftRot		= OculusRiftSensors.LeftEye.Rotation;	leftRot.Invert();	leftRot.Normalize();
					var rightRot	= OculusRiftSensors.RightEye.Rotation;	rightRot.Invert();	rightRot.Normalize();

					viewMatrixL = viewMatrix * Matrix.RotationQuaternion(leftRot)	* Matrix.Translation(leftEyePos);
					viewMatrixR = viewMatrix * Matrix.RotationQuaternion(rightRot)	* Matrix.Translation(rightEyePos);
				}
			}
			#endif
		}



		/// <summary>
		/// Setups camera.
		/// </summary>
		/// <param name="origin">Camera's origin</param>
		/// <param name="target">Vector directed toward the target</param>
		/// <param name="up">Camera's up vector</param>
		/// <param name="velocity">Camera current velocity</param>
		/// <param name="fov">Camera FOV in radians</param>
		/// <param name="near">Near Z-clipping plane distance</param>
		/// <param name="far">Fat Z-clipping place distance</param>
		/// <param name="convergence">Stereo convergence distance</param>
		/// <param name="separation">Stereo camera separation</param>
		/// <param name="aspectRatio">Viewport width divided by viewport height</param>
		public void SetupCameraFov ( Vector3 origin, Vector3 target, Vector3 up, float fov, float near, float far, float convergence, float separation, float aspectRatio )
		{
			var nearWidth	=	near * (float)Math.Tan( fov/2 ) * 2;
			var nearHeight	=	nearWidth / aspectRatio;
			var view		=	Matrix.LookAtRH( origin, target, up );

			SetupCamera( view, nearHeight, nearWidth, near, far, convergence, separation );
		}



		/// <summary>
		/// Setups camera.
		/// </summary>
		/// <param name="view"></param>
		/// <param name="velocity"></param>
		/// <param name="fov"></param>
		/// <param name="near"></param>
		/// <param name="far"></param>
		/// <param name="convergence"></param>
		/// <param name="separation"></param>
		public void SetupCameraFov ( Matrix view, float fov, float near, float far, float convergence, float separation, float aspectRatio )
		{
			var nearWidth	=	near * (float)Math.Tan( fov/2 ) * 2;
			var nearHeight	=	nearWidth / aspectRatio;

			SetupCamera( view, nearHeight, nearWidth, near, far, convergence, separation );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="origin"></param>
		/// <param name="rightHanded"></param>
		public void SetupCameraCubeFace ( Vector3 origin, CubeFace cubeFace, float near, float far )
		{
			Matrix view = Matrix.Identity;
			Matrix proj = Matrix.Identity;

			switch (cubeFace) {
				case CubeFace.FacePosX : view = Matrix.LookAtLH( origin, -Vector3.UnitX + origin, Vector3.UnitY ); break;
				case CubeFace.FaceNegX : view = Matrix.LookAtLH( origin,  Vector3.UnitX + origin, Vector3.UnitY ); break;
				case CubeFace.FacePosY : view = Matrix.LookAtLH( origin, -Vector3.UnitY + origin,-Vector3.UnitZ ); break;
				case CubeFace.FaceNegY : view = Matrix.LookAtLH( origin,  Vector3.UnitY + origin, Vector3.UnitZ ); break;
				case CubeFace.FacePosZ : view = Matrix.LookAtLH( origin, -Vector3.UnitZ + origin, Vector3.UnitY ); break;
				case CubeFace.FaceNegZ : view = Matrix.LookAtLH( origin,  Vector3.UnitZ + origin, Vector3.UnitY ); break;
			}
			
			SetupCamera( view, 2*near, 2*near, near, far, 1, 0 );
		}



		/// <summary>
		/// Gets view matrix for given stereo eye.
		/// </summary>
		/// <param name="stereoEye"></param>
		/// <returns></returns>
		public Matrix GetViewMatrix ( StereoEye stereoEye )
		{
			if (stereoEye==StereoEye.Mono) return viewMatrix;
			if (stereoEye==StereoEye.Left) return viewMatrixL;
			if (stereoEye==StereoEye.Right) return viewMatrixR;
			throw new ArgumentException("stereoEye");
		}


		/// <summary>
		/// Gets projection matrix for given stereo eye.
		/// </summary>
		/// <param name="stereoEye"></param>
		/// <returns></returns>
		public Matrix GetProjectionMatrix ( StereoEye stereoEye )
		{
			if (stereoEye==StereoEye.Mono) return projMatrix;
			if (stereoEye==StereoEye.Left) return projMatrixL;
			if (stereoEye==StereoEye.Right) return projMatrixR;
			throw new ArgumentException("stereoEye");
		}



		/// <summary>
		/// Gets camera matrix for given stereo eye.
		/// </summary>
		/// <param name="stereoEye"></param>
		/// <returns></returns>
		public Matrix GetCameraMatrix ( StereoEye stereoEye )
		{
			if (stereoEye==StereoEye.Mono) return cameraMatrix;
			if (stereoEye==StereoEye.Left) return cameraMatrixL;
			if (stereoEye==StereoEye.Right) return cameraMatrixR;
			throw new ArgumentException("stereoEye");
		}


		/// <summary>
		/// Returns camera position as Vector3
		/// </summary>
		/// <param name="stereoEye"></param>
		/// <returns></returns>
		public Vector3 GetCameraPosition ( StereoEye stereoEye )
		{
			if (stereoEye==StereoEye.Mono)  return cameraMatrix .TranslationVector;
			if (stereoEye==StereoEye.Left)  return cameraMatrixL.TranslationVector;
			if (stereoEye==StereoEye.Right) return cameraMatrixR.TranslationVector;
			throw new ArgumentException("stereoEye");
		}


		/// <summary>
		/// Returns camera position as Vector4
		/// </summary>
		/// <param name="stereoEye"></param>
		/// <returns></returns>
		public Vector4 GetCameraPosition4 ( StereoEye stereoEye )
		{
			if (stereoEye==StereoEye.Mono)  return new Vector4(cameraMatrix .TranslationVector, 1);
			if (stereoEye==StereoEye.Left)  return new Vector4(cameraMatrixL.TranslationVector, 1);
			if (stereoEye==StereoEye.Right) return new Vector4(cameraMatrixR.TranslationVector, 1);
			throw new ArgumentException("stereoEye");
		}


		/// <summary>
		///	Gets scale coefficient for linear Z-value reconstruction.
		///	To reconstruct Z-value use the following formula:
		/// 1 / (depth * scale + bias)
		/// </summary>
		internal float LinearizeDepthScale {
			get {
				return 1 / projZFar - 1 / projZNear;
			}
		}


		/// <summary>
		///	Gets bias coefficient for linear Z-value reconstruction.
		///	To reconstruct Z-value use the following formula:
		/// 1 / (depth * scale + bias)
		/// </summary>
		internal float LinearizeDepthBias {
			get {
				return 1 / projZNear;
			}
		}


		/// <summary>
		/// Gets bounding frustum for camera
		/// </summary>
		/// <returns></returns>
		public BoundingFrustum Frustum {
			get {
				return new BoundingFrustum( viewMatrix * projMatrix );
			}
		}
	}
}
