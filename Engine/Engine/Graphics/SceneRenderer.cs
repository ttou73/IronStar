using System;
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
using Fusion.Engine.Graphics.Ubershaders;

namespace Fusion.Engine.Graphics {

	internal partial class SceneRenderer : RenderComponent {

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

			constBuffer			=	new ConstantBuffer( Game.GraphicsDevice, typeof(BATCH) );
			constBufferBones	=	new ConstantBuffer( Game.GraphicsDevice, typeof(Matrix), MaxBones );
			constBufferSubset	=	new ConstantBuffer( Game.GraphicsDevice, typeof(SUBSET) );


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
			surfaceShader	=	Game.Content.Load<Ubershader>("surface");
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

			if (flags.HasFlag( SurfaceFlags.SHADOW )) {
				ps.RasterizerState = RasterizerState.CullNone;
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
		internal void RenderForward ( GameTime gameTime, StereoEye stereoEye, Camera camera, HdrFrame frame, RenderWorld rw, bool staticOnly )
		{		
			using ( new PixEvent("RenderForward") ) {

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

				var cbData			=	new BATCH();
				var cbDataSubset	=	new SUBSET();

				var hdr			=	frame.HdrBuffer.Surface;
				var depth		=	frame.DepthBuffer.Surface;
				var feedback	=	frame.FeedbackBuffer.Surface;

				device.ResetStates();

				device.SetTargets( depth, hdr, feedback );
				device.PixelShaderSamplers[0]	= SamplerState.LinearPointClamp ;
				device.PixelShaderSamplers[1]	= SamplerState.PointClamp;
				device.PixelShaderSamplers[2]	= SamplerState.AnisotropicClamp;

				var instances	=	rw.Instances;

				if (instances.Any()) {
					device.PixelShaderResources[0]	= rs.VTSystem.PageTable;
					device.PixelShaderResources[1]	= rs.VTSystem.PhysicalPages0;
					device.PixelShaderResources[2]	= rs.VTSystem.PhysicalPages1;
					device.PixelShaderResources[3]	= rs.VTSystem.PhysicalPages2;
					device.PixelShaderResources[4]	= rs.LightManager.LightGrid.GridTexture;
					device.PixelShaderResources[5]	= rs.LightManager.LightGrid.IndexDataGpu;
					device.PixelShaderResources[6]	= rs.LightManager.LightGrid.LightDataGpu;
				}

				//#warning INSTANSING!
				foreach ( var instance in instances ) {

					if (!instance.Visible) {
						continue;
					}

					if ( staticOnly && !instance.Static ) {
						continue;
					}

					cbData.View				=	view;
					cbData.Projection		=	projection;
					cbData.World			=	instance.World;
					cbData.ViewPos			=	viewPosition;
					cbData.Color			=	instance.Color;
					cbData.ViewBounds		=	new Vector4( hdr.Width, hdr.Height, hdr.Width, hdr.Height );
					cbData.VTPageScaleRCP	=	rs.VTSystem.PageScaleRCP;

					constBuffer.SetData( cbData );

					device.PixelShaderConstants[0]	=	constBuffer ;
					device.VertexShaderConstants[0]	=	constBuffer ;
					device.PixelShaderConstants[1]	=	constBufferSubset;

					device.SetupVertexInput( instance.vb, instance.ib );

					if (instance.IsSkinned) {
						constBufferBones.SetData( instance.BoneTransforms );
						device.VertexShaderConstants[3]	= constBufferBones ;
					}

					try {

						device.PipelineState	=	factory[ (int)( SurfaceFlags.FORWARD | SurfaceFlags.RIGID ) ];


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
				rs.VTSystem.Update( feedbackBuffer, gameTime );
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

				var cbData			= new BATCH();

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
