﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Core;
using Fusion.Core.Mathematics;
using Fusion.Core.Configuration;
using Fusion.Engine.Common;
using Fusion.Drivers.Graphics;
using System.Runtime.InteropServices;
using Fusion.Development;


namespace Fusion.Engine.Graphics {

	internal class SceneRenderer : RenderComponent {

		internal const int MaxBones = 128;

		ConstantBuffer	constBuffer;
		ConstantBuffer	constBufferBones;
		ConstantBuffer	constBufferSubset;
		Ubershader		surfaceShader;
		StateFactory	factory;

		Texture2D		defaultDiffuse	;
		Texture2D		defaultSpecular	;
		Texture2D		defaultNormalMap;
		Texture2D		defaultEmission	;


		struct CBMeshInstanceData {
			public Matrix	Projection;
			public Matrix	View;
			public Matrix	World;
			public Vector4	ViewPos			;
			public Vector4	BiasSlopeFar	;
			public Color4	Color;
			public Vector4	ViewBounds;
		}


		struct CBSubsetData {
			public Vector4 Rectangle;
		}


		/// <summary>
		/// Gets pipeline state factory
		/// </summary>
		internal StateFactory Factory {
			get {
				return factory;
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="Game"></param>
		public SceneRenderer ( RenderSystem rs ) : base( rs )
		{
		}


		/// <summary>
		/// 
		/// </summary>
		public override void Initialize ()
		{
			LoadContent();

			constBuffer			=	new ConstantBuffer( Game.GraphicsDevice, typeof(CBMeshInstanceData) );
			constBufferBones	=	new ConstantBuffer( Game.GraphicsDevice, typeof(Matrix), MaxBones );
			constBufferSubset	=	new ConstantBuffer( Game.GraphicsDevice, typeof(CBSubsetData) );


			defaultDiffuse	=	new Texture2D( Game.GraphicsDevice, 4,4, ColorFormat.Rgba8, false );
			defaultDiffuse.SetData( Enumerable.Range(0,16).Select( i => Color.Gray ).ToArray() );

			defaultSpecular	=	new Texture2D( Game.GraphicsDevice, 4,4, ColorFormat.Rgba8, false );
			defaultSpecular.SetData( Enumerable.Range(0,16).Select( i => new Color(0,128,0,255) ).ToArray() );

			defaultNormalMap	=	new Texture2D( Game.GraphicsDevice, 4,4, ColorFormat.Rgba8, false );
			defaultNormalMap.SetData( Enumerable.Range(0,16).Select( i => new Color(128,128,255,255) ).ToArray() );

			defaultEmission	=	new Texture2D( Game.GraphicsDevice, 4,4, ColorFormat.Rgba8, false );
			defaultEmission.SetData( Enumerable.Range(0,16).Select( i => Color.Black ).ToArray() );

			//Ubershader.AddEnumerator( "SceneRenderer", (t

			Game.Reloading += (s,e) => LoadContent();
		}



		/// <summary>
		/// 
		/// </summary>
		void LoadContent ()
		{
			surfaceShader	=	Game.RenderSystem.Shaders.Load("surface");
			factory			=	surfaceShader.CreateFactory( typeof(SurfaceFlags), (ps,i) => Enum(ps, (SurfaceFlags)i ) );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="ps"></param>
		/// <param name="flags"></param>
		void Enum ( PipelineState ps, SurfaceFlags flags )
		{
			ps.RasterizerState	=	RasterizerState.CullCW;

			if (flags.HasFlag( SurfaceFlags.SKINNED )) {
				ps.VertexInputElements	=	VertexColorTextureTBNSkinned.Elements;
			}
			
			if (flags.HasFlag( SurfaceFlags.RIGID )) {
				ps.VertexInputElements	=	VertexColorTextureTBNRigid.Elements;
			}
		}



		


		/// <summary>
		/// 
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose ( bool disposing )
		{
			if (disposing) {
				SafeDispose( ref constBuffer );
				SafeDispose( ref constBufferBones );
				SafeDispose( ref constBufferSubset );

				SafeDispose( ref defaultDiffuse		);
				SafeDispose( ref defaultSpecular	);
				SafeDispose( ref defaultNormalMap	);
				SafeDispose( ref defaultEmission	);
			}

			base.Dispose( disposing );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="camera"></param>
		/// <param name="depthBuffer"></param>
		/// <param name="hdrTarget"></param>
		/// <param name="diffuse"></param>
		/// <param name="specular"></param>
		/// <param name="normals"></param>
		internal void RenderGBuffer ( GameTime gameTime, StereoEye stereoEye, Camera camera, HdrFrame frame, RenderWorld rw )
		{		
			using ( new PixEvent("RenderGBuffer") ) {

				if (surfaceShader==null) {	
					return;
				}

				if (rs.SkipSceneRendering) {
					return;
				}

				var device		=	Game.GraphicsDevice;

				var view			=	camera.GetViewMatrix( stereoEye );
				var projection		=	camera.GetProjectionMatrix( stereoEye );
				var viewPosition	=	camera.GetCameraPosition4( stereoEye );

				var cbData			=	new CBMeshInstanceData();
				var cbDataSubset	=	new CBSubsetData();

				var hdr			=	frame.HdrBuffer.Surface;
				var depth		=	frame.DepthBuffer.Surface;
				var diffuse		=	frame.DiffuseBuffer.Surface;
				var specular	=	frame.SpecularBuffer.Surface;
				var normals		=	frame.NormalMapBuffer.Surface;
				var scattering	=	frame.ScatteringBuffer.Surface;
				var feedback	=	frame.FeedbackBuffer.Surface;

				device.ResetStates();

				device.SetTargets( depth, hdr, diffuse, specular, normals, feedback );
				device.PixelShaderSamplers[0]	= SamplerState.LinearPointWrap ;
				device.PixelShaderSamplers[1]	= SamplerState.PointClamp;
				device.PixelShaderSamplers[2]	= SamplerState.AnisotropicWrap;

				var instances	=	rw.Instances;

				if (instances.Any()) {
					//device.PixelShaderResources[0]	= rs.VirtualTexture.FallbackTexture.Srv;
					device.PixelShaderResources[1]	= rs.VirtualTexture.PageTable;
					device.PixelShaderResources[2]	= rs.VirtualTexture.PhysicalPages0;
					device.PixelShaderResources[3]	= rs.VirtualTexture.PhysicalPages1;
					device.PixelShaderResources[4]	= rs.VirtualTexture.PhysicalPages2;
				}

				//#warning INSTANSING!
				foreach ( var instance in instances ) {

					if (!instance.Visible) {
						continue;
					}

					cbData.View			=	view;
					cbData.Projection	=	projection;
					cbData.World		=	instance.World;
					cbData.ViewPos		=	viewPosition;
					cbData.Color		=	instance.Color;
					cbData.ViewBounds	=	new Vector4( hdr.Width, hdr.Height, hdr.Width, hdr.Height );

					constBuffer.SetData( cbData );

					device.PixelShaderConstants[0]	= constBuffer ;
					device.VertexShaderConstants[0]	= constBuffer ;
					device.PixelShaderConstants[1]	=	constBufferSubset;

					device.SetupVertexInput( instance.vb, instance.ib );

					if (instance.IsSkinned) {
						constBufferBones.SetData( instance.BoneTransforms );
						device.VertexShaderConstants[3]	= constBufferBones ;
					}

					try {

						device.PipelineState	=	factory[ (int)( SurfaceFlags.GBUFFER | SurfaceFlags.RIGID ) ];


						foreach ( var subset in instance.Subsets ) {

							var vt		=	rw.VirtualTexture;
							var rect	=	vt.GetTexturePosition( subset.Name );

							cbDataSubset.Rectangle	=	new Vector4( rect.X, rect.Y, rect.Width, rect.Height );

							constBufferSubset.SetData( cbDataSubset );
							device.PixelShaderConstants[1]	=	constBufferSubset;

							device.DrawIndexed( subset.PrimitiveCount*3, subset.StartPrimitive*3, 0 );
						}

						rs.Counters.SceneDIPs++;
						
					} catch ( UbershaderException e ) {
						Log.Warning( e.Message );					
						ExceptionDialog.Show( e );
					}
				}

				
				//
				//	downsample feedback buffer and readback it to virtual texture :
				//
				rs.Filter.StretchRect( frame.FeedbackBufferRB.Surface, frame.FeedbackBuffer, SamplerState.PointClamp );

				var feedbackBuffer = new VTAddress[ HdrFrame.FeedbackBufferWidth * HdrFrame.FeedbackBufferHeight ];
				frame.FeedbackBufferRB.GetFeedback( feedbackBuffer );
				rs.VirtualTexture.Update( feedbackBuffer, gameTime );
			}
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="material"></param>
		/// <param name="flags"></param>
		/// <returns></returns>
		SurfaceFlags ApplyFlags ( MeshInstance instance, SurfaceFlags flags )
		{
			if (instance.IsSkinned) {
				flags |= SurfaceFlags.SKINNED;
			} else {
				flags |= SurfaceFlags.RIGID;
			}

			return flags;
		}



		#if false
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		internal void RenderFeedbackBuffer ( StereoEye stereoEye, Camera camera, HdrFrame frame, IEnumerable<MeshInstance> instances )
		{
			if (rs.SkipFeedback) {
				return;
			}

			using ( new PixEvent("FeedbackBuffer") ) {
				if (surfaceShader==null) {
					return;
				}

				var device			=	Game.GraphicsDevice;

				var cbData			=	new CBMeshInstanceData();

				var view			=	camera.GetViewMatrix( stereoEye );
				var projection		=	camera.GetProjectionMatrix( stereoEye );
				var viewPosition	=	camera.GetCameraPosition4( stereoEye );

				device.SetTargets( frame.FeedbackBufferDepth.Surface, frame.FeedbackBufferColor.Surface );
				device.SetViewport( 0,0, frame.FeedbackBufferColor.Width, frame.FeedbackBufferColor.Height );

				device.PixelShaderConstants[0]	= constBuffer ;
				device.VertexShaderConstants[0]	= constBuffer ;
				device.PixelShaderSamplers[0]	= SamplerState.AnisotropicWrap ;

				cbData.Projection	=	projection;
				cbData.View			=	view;
				cbData.ViewPos		=	viewPosition;
				cbData.BiasSlopeFar	=	Vector4.Zero;

				//#warning INSTANSING!
				foreach ( var instance in instances ) {

					if (!instance.Visible) {
						continue;
					}

					device.PipelineState	=	factory[ (int)ApplyFlags( instance, SurfaceFlags.FEEDBACK ) ];
					cbData.World			=	instance.World;
					cbData.Color			=	instance.Color;

					constBuffer.SetData( cbData );

					if (instance.IsSkinned) {
						constBufferBones.SetData( instance.BoneTransforms );
						device.VertexShaderConstants[3]	= constBufferBones ;
					}

					device.SetupVertexInput( instance.vb, instance.ib );
					device.DrawIndexed( instance.indexCount, 0, 0 );
				}
			}
		}
		#endif



		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		internal void RenderShadowMapCascade ( ShadowContext shadowRenderCtxt, IEnumerable<MeshInstance> instances )
		{
			using ( new PixEvent("ShadowMap") ) {
				if (surfaceShader==null) {
					return;
				}

				var device			= Game.GraphicsDevice;

				var cbData			= new CBMeshInstanceData();

				var viewPosition	= Matrix.Invert( shadowRenderCtxt.ShadowView ).TranslationVector;

				device.SetTargets( shadowRenderCtxt.DepthBuffer, shadowRenderCtxt.ColorBuffer );
				device.SetViewport( shadowRenderCtxt.ShadowViewport );

				device.PixelShaderConstants[0]	=	constBuffer ;
				device.VertexShaderConstants[0]	=	constBuffer ;
				device.PixelShaderSamplers[0]	=	SamplerState.AnisotropicWrap ;
				device.PixelShaderConstants[1]	=	constBufferSubset;

				cbData.Projection	=	shadowRenderCtxt.ShadowProjection;
				cbData.View			=	shadowRenderCtxt.ShadowView;
				cbData.ViewPos		=	new Vector4( viewPosition, 1 );
				cbData.BiasSlopeFar	=	new Vector4( shadowRenderCtxt.DepthBias, shadowRenderCtxt.SlopeBias, shadowRenderCtxt.FarDistance, 0 );

				//#warning INSTANSING!
				foreach ( var instance in instances ) {

					if (!instance.Visible) {
						continue;
					}

					device.PipelineState	=	factory[ (int)ApplyFlags( instance, SurfaceFlags.SHADOW ) ];
					cbData.World			=	instance.World;
					cbData.Color			=	instance.Color;

					constBuffer.SetData( cbData );

					if (instance.IsSkinned) {
						constBufferBones.SetData( instance.BoneTransforms );
						device.VertexShaderConstants[3]	= constBufferBones ;
					}

					device.SetupVertexInput( instance.vb, instance.ib );
					device.DrawIndexed( instance.indexCount, 0, 0 );

					rs.Counters.ShadowDIPs++;
				}
			}
		}
	}
}