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
using System.IO;
using Fusion.Engine.Graphics.Ubershaders;


namespace Fusion.Engine.Graphics {
	[RequireShader("lighting", true)]
	[ShaderSharedStructure(typeof(Particle))]
	internal partial class LightRenderer : RenderComponent {

		[ShaderDefine]		const int	BlockSizeX		=	16;
		[ShaderDefine]		const int	BlockSizeY		=	16;
		[ShaderDefine]		const int	VoxelSize		=	64;
		[ShaderDefine]		const int	VoxelCount		=	VoxelSize * VoxelSize * VoxelSize;


		DepthStencil2D		spotDepth		;
		RenderTarget2D		spotColor		;

		CascadedShadowMap	cascadedShadowMap;
		UserTexture			envLut;


		OMNILIGHT[]		omniLightData;
		SPOTLIGHT[]		spotLightData;
		ENVLIGHT[]		envLightData;
		DECAL[]			decalData;

		StructuredBuffer omniLightBuffer;
		StructuredBuffer spotLightBuffer;
		StructuredBuffer envLightBuffer;
		StructuredBuffer decalBuffer;


		
		/*[ShaderResource(ShaderResourceType.StructuredBuffer, typeof(OMNILIGHT))]
		StructuredBuffer OmniLightBuffer  { get { return omniLightBuffer; } }

		[ShaderResource(ShaderResourceType.StructuredBuffer, typeof(SPOTLIGHT))]
		StructuredBuffer SpotLightBuffer  { get { return spotLightBuffer; } }

		[ShaderResource(ShaderResourceType.StructuredBuffer, typeof(ENVLIGHT))]
		StructuredBuffer EnvLightBuffer   { get { return envLightBuffer;  } }

		[ShaderResource(ShaderResourceType.StructuredBuffer, typeof(DECAL))]
		StructuredBuffer DecalBuffer      { get { return decalBuffer;     } }*/
		//*/

		internal RenderTarget2D	SpotColor { get { return spotColor; } }
		internal DepthStencil2D	SpotDepth { get { return spotDepth; } }

		internal CascadedShadowMap CascadedShadowMap { get { return cascadedShadowMap; } }


		enum LightingFlags {
			SOLIDLIGHTING	=	0x0001,
			PARTICLES		=	0x0002,
		}


		enum VoxelFlags {
			DEBUG_DRAW_VOXEL		=	0x0001,
			COPY_BUFFER_TO_VOXEL	=	0x0002,
			CLEAR_VOXEL				=	0x0004,
		}

		[ShaderStructure]
		[StructLayout(LayoutKind.Explicit, Size=656)]
		struct LightingParams {
			[FieldOffset(  0)] public Matrix	View;
			[FieldOffset( 64)] public Matrix	Projection;
			[FieldOffset(128)] public Matrix	InverseViewProjection;
			[FieldOffset(192)] public Vector4	FrustumVectorTR;
			[FieldOffset(208)] public Vector4	FrustumVectorBR;
			[FieldOffset(224)] public Vector4	FrustumVectorBL;
			[FieldOffset(240)] public Vector4	FrustumVectorTL;
			[FieldOffset(256)] public Matrix	CSMViewProjection0;
			[FieldOffset(320)] public Matrix	CSMViewProjection1;
			[FieldOffset(384)] public Matrix	CSMViewProjection2;
			[FieldOffset(448)] public Matrix	CSMViewProjection3;
			[FieldOffset(512)] public Vector4	ViewPosition;
			[FieldOffset(528)] public Vector4	DirectLightDirection;
			[FieldOffset(544)] public Vector4	DirectLightIntensity;
			[FieldOffset(560)] public Vector4	ViewportSize;
			[FieldOffset(576)] public Vector4	CSMFilterRadius;
			[FieldOffset(592)] public Color4	AmbientColor;
			[FieldOffset(608)] public Vector4	Viewport;
			[FieldOffset(624)] public float		ShowCSLoadOmni;
			[FieldOffset(628)] public float		ShowCSLoadEnv;
			[FieldOffset(632)] public float		ShowCSLoadSpot;
			[FieldOffset(636)] public int		CascadeCount;
			[FieldOffset(640)] public float		CascadeScale;
			[FieldOffset(644)] public float		FogDensity;

		}


		[ShaderStructure]
		struct OMNILIGHT {
			public Vector4	PositionRadius;
			public Vector4	Intensity;
			public Vector4	ExtentMin;	// x,y, depth
			public Vector4	ExtentMax;	// x,y, depth
		}


		[ShaderStructure]
		struct ENVLIGHT {
			public Vector4	Position;
			public Vector4	Dimensions;
			public Vector4	ExtentMin;	// x,y, depth
			public Vector4	ExtentMax;	// x,y, depth
		}


		[ShaderStructure]
		struct SPOTLIGHT {
			public Matrix	ViewProjection;
			public Vector4	PositionRadius;
			public Vector4	IntensityFar;
			public Vector4	ExtentMin;	// x,y, depth
			public Vector4	ExtentMax;	// x,y, depth
			public Vector4	MaskScaleOffset;
			public Vector4	ShadowScaleOffset;
		}


		[ShaderStructure]
		struct DECAL {
			public Matrix	DecalMatrixInv;
			public Vector4 	BasisX;
			public Vector4 	BasisY;
			public Vector4 	BasisZ;
			public Vector4 	EmissionRoughness;
			public Vector4	BaseColorMetallic;
			public Vector4	ImageScaleOffset;
			public Vector4	ExtentMin;	// x,y, depth
			public Vector4	ExtentMax;	// x,y, depth
			public float	ColorFactor;
			public float	SpecularFactor;
			public float	NormalMapFactor;
			public float	FalloffFactor;
		}


		Ubershader		lightingShader;
		StateFactory	factory;

		ConstantBuffer	lightingCB;


		/*[ShaderConstantBuffer(typeof(LightingParams), 0)]
		ConstantBuffer	LightingCB { get { return lightingCB; } }*/

		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="game"></param>
		public LightRenderer( RenderSystem rs ) : base(rs)
		{
			SetDefaults();
		}



		/// <summary>
		/// 
		/// </summary>
		public override void Initialize ()
		{
			lightingCB			=	new ConstantBuffer( Game.GraphicsDevice, typeof(LightingParams) );
			omniLightBuffer		=	new StructuredBuffer( Game.GraphicsDevice, typeof(OMNILIGHT), RenderSystem.MaxOmniLights, StructuredBufferFlags.None );
			spotLightBuffer		=	new StructuredBuffer( Game.GraphicsDevice, typeof(SPOTLIGHT), RenderSystem.MaxSpotLights, StructuredBufferFlags.None );
			envLightBuffer		=	new StructuredBuffer( Game.GraphicsDevice, typeof(ENVLIGHT),  RenderSystem.MaxEnvLights, StructuredBufferFlags.None );
			decalBuffer			=	new StructuredBuffer( Game.GraphicsDevice, typeof(DECAL),	 RenderSystem.MaxDecals,	StructuredBufferFlags.None );

			using ( var ms = new MemoryStream( Properties.Resources.envLut ) ) {
				envLut    =   UserTexture.CreateFromTga( rs, ms, false );
			}

			CreateShadowMaps();

			LoadContent();
			Game.Reloading += (s,e) => LoadContent();
		}



		/// <summary>
		/// 
		/// </summary>
		void LoadContent ()
		{
			lightingShader	=	Game.Content.Load<Ubershader>("lighting");
			factory			=	lightingShader.CreateFactory( typeof(LightingFlags), Primitive.TriangleList, VertexInputElement.Empty );
		}



		void EnumVoxel ( PipelineState ps, VoxelFlags flags )
		{
			ps.Primitive			=	Primitive.TriangleList;
			ps.RasterizerState		=	RasterizerState.CullNone;
			ps.BlendState			=	BlendState.Opaque;
			ps.DepthStencilState	=	DepthStencilState.None;

			//ps.VertexInputElements	=	VertexInputElement.Empty;

			if (flags==VoxelFlags.DEBUG_DRAW_VOXEL) {
				ps.RasterizerState		=	RasterizerState.CullCW;
				ps.DepthStencilState	=	DepthStencilState.Default;
				ps.BlendState			=	BlendState.Opaque;
				ps.Primitive			=	Primitive.PointList;
			}
		}



		/// <summary>
		/// 
		/// </summary>
		void CreateShadowMaps ()
		{
			SafeDispose( ref cascadedShadowMap );

			SafeDispose( ref spotColor );
			SafeDispose( ref spotDepth );

			cascadedShadowMap	=	new CascadedShadowMap( Game.GraphicsDevice, CSMCascadeSize, CSMCascadeCount );

			spotColor			=	new RenderTarget2D( Game.GraphicsDevice, ColorFormat.R32F,  SpotShadowSize * 4, SpotShadowSize * 4 );
			spotDepth			=	new DepthStencil2D( Game.GraphicsDevice, DepthFormat.D24S8, SpotShadowSize * 4, SpotShadowSize * 4 );

		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose ( bool disposing )
		{
			if (disposing) {
				SafeDispose( ref spotDepth );
				SafeDispose( ref spotColor );

				SafeDispose( ref cascadedShadowMap );

				SafeDispose( ref lightingCB );

				SafeDispose( ref omniLightBuffer );
				SafeDispose( ref spotLightBuffer );
				SafeDispose( ref envLightBuffer );
				SafeDispose( ref decalBuffer );

				SafeDispose( ref envLut );
			}

			base.Dispose( disposing );
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		/// <param name="projection"></param>
		internal void RenderLighting ( StereoEye stereoEye, Camera camera, HdrFrame hdrFrame, RenderWorld viewLayer, RenderTargetCube envLight )
		{
			using ( new PixEvent("TiledLighting") ) {
				var view		=	camera.GetViewMatrix( stereoEye );
				var projection	=	camera.GetProjectionMatrix( stereoEye );

				var device = Game.GraphicsDevice;
				device.ResetStates();

				var width	=	hdrFrame.HdrBuffer.Width;
				var height	=	hdrFrame.HdrBuffer.Height;


				ICSMController csmCtrl	=	viewLayer.LightSet.DirectLight.CSMController ?? csmController;

				int activeCascadeCount	=	Math.Min( cascadedShadowMap.CascadeCount, csmCtrl.GetActiveCascadeCount() );


				//
				//	Setup compute shader parameters and states :
				//
				try {

					var cbData	=	new LightingParams();
					var invView	=	Matrix.Invert( view );
					var invVP	=	Matrix.Invert( view * projection );
					var viewPos	=	invView.TranslationVector;

					cbData.DirectLightDirection		=	new Vector4( viewLayer.LightSet.DirectLight.Direction, 0 );
					cbData.DirectLightIntensity		=	viewLayer.LightSet.DirectLight.Intensity.ToVector4();
					cbData.Projection				=	projection;

					cbData.CSMViewProjection0		=	csmCtrl.GetShadowViewMatrix(0) * csmCtrl.GetShadowProjectionMatrix(0);
					cbData.CSMViewProjection1		=	csmCtrl.GetShadowViewMatrix(1) * csmCtrl.GetShadowProjectionMatrix(1);
					cbData.CSMViewProjection2		=	csmCtrl.GetShadowViewMatrix(2) * csmCtrl.GetShadowProjectionMatrix(2);
					cbData.CSMViewProjection3		=	csmCtrl.GetShadowViewMatrix(3) * csmCtrl.GetShadowProjectionMatrix(3);

					cbData.View						=	view;
					cbData.ViewPosition				=	new Vector4(viewPos,1);
					cbData.InverseViewProjection	=	invVP;
					cbData.CSMFilterRadius			=	new Vector4( CSMFilterSize );

					cbData.AmbientColor				=	viewLayer.LightSet.AmbientLevel;
					cbData.Viewport					=	new Vector4( 0, 0, width, height );
					cbData.ShowCSLoadOmni			=	ShowOmniLightTileLoad ? 1 : 0;
					cbData.ShowCSLoadEnv			=	ShowEnvLightTileLoad  ? 1 : 0;
					cbData.ShowCSLoadSpot			=	ShowSpotLightTileLoad ? 1 : 0;

					cbData.CascadeCount				=	activeCascadeCount;
					cbData.CascadeScale				=	1.0f / (float)cascadedShadowMap.CascadeCount;

					cbData.FogDensity				=	viewLayer.FogSettings.Density;


					ComputeOmniLightsTiles	( view, projection, viewLayer.LightSet );
					ComputeSpotLightsTiles	( view, projection, viewLayer.LightSet );
					ComputeEnvLightsTiles	( view, projection, viewLayer.LightSet );
					ComputeDecalTiles		( view, projection, viewLayer.LightSet );


					//
					//	set states :
					//
					device.SetTargets( null, hdrFrame.HdrBuffer.Surface );

					lightingCB.SetData( cbData );

					device.ComputeShaderSamplers[0]	=	SamplerState.PointClamp;
					device.ComputeShaderSamplers[1]	=	SamplerState.LinearClamp;
					device.ComputeShaderSamplers[2]	=	SamplerState.ShadowSampler;
					device.ComputeShaderSamplers[3]	=	SamplerState.LinearPointWrap;

					device.ComputeShaderResources[0]	=	hdrFrame.GBuffer0;
					device.ComputeShaderResources[1]	=	hdrFrame.GBuffer1;
					device.ComputeShaderResources[2]	=	rs.Sky.SkyCube;
					device.ComputeShaderResources[4]	=	hdrFrame.DepthBuffer;
					device.ComputeShaderResources[5]	=	cascadedShadowMap.ColorBuffer;
					device.ComputeShaderResources[6]	=	spotColor;
					device.ComputeShaderResources[7]	=	viewLayer.LightSet.SpotAtlas==null ? rs.WhiteTexture.Srv : viewLayer.LightSet.SpotAtlas.Texture.Srv;
					device.ComputeShaderResources[8]	=	omniLightBuffer;
					device.ComputeShaderResources[9]	=	spotLightBuffer;
					device.ComputeShaderResources[10]	=	envLightBuffer;
					device.ComputeShaderResources[11]	=	rs.SsaoFilter.OcclusionMap;
					device.ComputeShaderResources[12]	=	viewLayer.RadianceCache;
					device.ComputeShaderResources[13]	=	viewLayer.ParticleSystem.SimulatedParticles;
					device.ComputeShaderResources[14]	=	cascadedShadowMap.ParticleShadow;
					device.ComputeShaderResources[15]	=	envLut.Srv;
					device.ComputeShaderResources[16]	=	decalBuffer;
					device.ComputeShaderResources[17]	=	viewLayer.LightSet.DecalAtlas==null ? rs.WhiteTexture.Srv : viewLayer.LightSet.DecalAtlas.Texture.Srv;

					device.ComputeShaderConstants[0]	=	lightingCB;

					device.SetCSRWTexture( 0, hdrFrame.LightAccumulator.Surface );
					device.SetCSRWBuffer(  1, viewLayer.ParticleSystem.ParticleLighting );

					//
					//	Dispatch solids :
					//
					using (new PixEvent("Solid Lighting")) {
						device.PipelineState	=	factory[ (int)LightingFlags.SOLIDLIGHTING ];
						device.Dispatch( MathUtil.IntDivUp( width, BlockSizeX ), MathUtil.IntDivUp( height, BlockSizeY ), 1 );
					}

					//
					//	Dispatch particles :
					//
					using (new PixEvent("Particle Lighting")) {
						if (stereoEye!=StereoEye.Right && !rs.SkipParticles) {
							int threadGroupCount	=	MathUtil.IntDivUp( ParticleSystem.MaxSimulatedParticles, ParticleSystem.BlockSize );
							device.PipelineState	=	factory[ (int)LightingFlags.PARTICLES ];
							device.Dispatch( threadGroupCount, 1, 1 );
						}
					}
	
				} catch ( UbershaderException e ) {
					Log.Warning("{0}", e.Message );
				}


				//
				//	Add accumulated light  :
				//
				rs.Filter.OverlayAdditive( hdrFrame.HdrBuffer.Surface, hdrFrame.LightAccumulator );

				//	Uncomment to enable SSS :
				#if false
				rs.Filter.GaussBlur( hdrFrame.SSSAccumulator, hdrFrame.LightAccumulator, 5, 0 ); 
				rs.Filter.OverlayAdditive( hdrFrame.HdrBuffer.Surface, hdrFrame.SSSAccumulator );
				#endif

				device.ResetStates();


				if (rs.ShowLightCounters) {
					var ls = viewLayer.LightSet;
					Log.Message("lights: {0,5} omni {1,5} spot {2,5} env", ls.OmniLights.Count, ls.SpotLights.Count, ls.EnvLights.Count );
				}
			}
		}
	}
}
