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
using System.Diagnostics;
using Fusion.Core.Extensions;
using Fusion.Engine.Graphics.Ubershaders;

namespace Fusion.Engine.Graphics {

	[RequireShader("sky")]
	internal class Sky : RenderComponent {
		[Flags]
		enum SkyFlags : int
		{
			SKY		= 1 << 0,
			FOG		= 1 << 1,
			SRGB	= 1 << 2,
			CIERGB	= 1 << 3,
		}

		//	row_major float4x4 MatrixWVP;      // Offset:    0 Size:    64 [unused]
		//	float3 SunPosition;                // Offset:   64 Size:    12
		//	float4 SunColor;                   // Offset:   80 Size:    16
		//	float Turbidity;                   // Offset:   96 Size:     4 [unused]
		//	float3 Temperature;                // Offset:  100 Size:    12
		//	float SkyIntensity;                // Offset:  112 Size:     4
		[StructLayout(LayoutKind.Explicit, Size=160)]
		struct SkyConsts {
			[FieldOffset(  0)] public Matrix 	MatrixWVP;
			[FieldOffset( 64)] public Vector3	SunPosition;
			[FieldOffset( 80)] public Color4	SunColor;
			[FieldOffset( 96)] public float		Turbidity;
			[FieldOffset(100)] public Vector3	Temperature; 
			[FieldOffset(112)] public float		SkyIntensity; 
			[FieldOffset(116)] public Vector3	Ambient;
			[FieldOffset(128)] public float		Time;
			[FieldOffset(132)] public Vector3	ViewPos;
			[FieldOffset(136)] public float		SunAngularSize;

		}


		struct SkyVertex {
			[Vertex("POSITION")]
			public Vector4 Vertex;
		}

		VertexBuffer	skyVB;
		Ubershader		sky;
		ConstantBuffer	skyConstsCB;
		SkyConsts		skyConstsData;
		StateFactory	factory;

		public Vector3	SkyAmbientLevel { get; protected set; }

		Vector3[]		randVectors;

		public RenderTargetCube	SkyCube { get { return skyCube; } }
		RenderTargetCube	skyCube;

		Random	rand = new Random();



		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="rs"></param>
		public Sky ( RenderSystem rs ) : base( rs )
		{
		}



		/// <summary>
		/// 
		/// </summary>
		public override void Initialize() 
		{
			skyCube		=	new RenderTargetCube( device, ColorFormat.Rgba16F, 128, true );
			skyConstsCB	=	new ConstantBuffer( device, typeof(SkyConsts) );

			LoadContent();

			Game.Reloading += (s,e) => LoadContent();

			
			var skySphere	=	SkySphere.GetVertices(4).Select( v => new SkyVertex{ Vertex = v } ).ToArray();
			skyVB			=	new VertexBuffer( Game.GraphicsDevice, typeof(SkyVertex), skySphere.Length );
			skyVB.SetData( skySphere );

			
			randVectors	=	new Vector3[64];

			for (int i=0; i<randVectors.Length; i++) {
				Vector3 randV;
				do {
					randV = rand.NextVector3( -Vector3.One, Vector3.One );
				} while ( randV.Length()>1 && randV.Y < 0 );

				randVectors[i] = randV.Normalized();
			}
		}



		/// <summary>
		/// 
		/// </summary>
		void LoadContent ()
		{
			sky			=	Game.Content.Load<Ubershader>("sky");
			factory		=	sky.CreateFactory( typeof(SkyFlags), (ps,i) => EnumFunc(ps, (SkyFlags)i) );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="ps"></param>
		/// <param name="flags"></param>
		void EnumFunc ( PipelineState ps, SkyFlags flags )
		{
			ps.VertexInputElements	=	VertexInputElement.FromStructure<SkyVertex>();

			//	do not cull triangles for both for RH and LH coordinates 
			//	for direct view and cubemaps.
			ps.RasterizerState		=	RasterizerState.CullNone; 
			ps.BlendState			=	BlendState.Opaque;
			ps.DepthStencilState	=	flags.HasFlag(SkyFlags.FOG) ? DepthStencilState.None : DepthStencilState.Readonly;
		}



		/// <summary>
		/// 
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing ) {
				SafeDispose( ref skyVB );
				SafeDispose( ref skyCube );
				SafeDispose( ref skyConstsCB );
			}
			base.Dispose( disposing );
		}



		/// <summary>
		/// Selects colorspace in ubershader.
		/// </summary>
		/// <param name="flags"></param>
		/// <param name="settings"></param>
		void ApplyColorSpace ( ref SkyFlags flags, SkySettings settings )
		{	
			switch (settings.RgbSpace) {
				case RgbSpace.CIE_RGB	: flags |= SkyFlags.CIERGB;	break;
				case RgbSpace.sRGB		: flags |= SkyFlags.SRGB;	break;
			}
		}



		/// <summary>
		/// Renders fog look-up table
		/// </summary>
		internal void RenderFogTable( SkySettings settings )
		{
			using ( new PixEvent("Fog Table") ) {
				var	sunPos		= settings.SunPosition;
				var sunColor	= settings.SunLightColor;

				var rotation	=	Matrix.Identity;
				var projection	=	MathUtil.ComputeCubemapProjectionMatrixLH( 0.125f, 10.0f );
				var cubeWVPS	=	MathUtil.ComputeCubemapViewMatriciesLH( Vector3.Zero, rotation, projection );

				var flags		=	SkyFlags.FOG;

				ApplyColorSpace( ref flags, settings );
				
				device.PipelineState	=	factory[(int)flags];
	//			rs.DepthStencilState = DepthStencilState.None ;

				skyConstsData.SunPosition	= sunPos;
				skyConstsData.SunColor		= sunColor;
				skyConstsData.Turbidity		= settings.SkyTurbidity;
				skyConstsData.Temperature	= Temperature.Get( settings.SunTemperature ); 
				skyConstsData.SkyIntensity	= settings.SkyIntensity;

				for( int i = 0; i < 6; ++i ) {
					device.SetTargets( null, SkyCube.GetSurface(0, (CubeFace)i ) );

					SkyCube.SetViewport();

					skyConstsData.MatrixWVP = cubeWVPS[i];
	
					skyConstsCB.SetData( skyConstsData );
					device.VertexShaderConstants[0] = skyConstsCB;
					device.PixelShaderConstants[0] = skyConstsCB;


					device.SetupVertexInput( skyVB, null );
					device.Draw( skyVB.Capacity, 0 );
				}

				device.ResetStates();

				SkyCube.BuildMipmaps();
			}
		}



		/// <summary>
		/// Renders sky with specified technique
		/// </summary>
		/// <param name="rendCtxt"></param>
		/// <param name="techName"></param>
		internal void Render( Camera camera, StereoEye stereoEye, HdrFrame frame, SkySettings settings )
		{
			using ( new PixEvent("Sky Rendering") ) {
				var scale		=	Matrix.Scaling( settings.SkySphereSize );
				var rotation	=	Matrix.Identity;

				var	sunPos		=	settings.SunPosition;
				var sunColor	=	settings.SunGlowColor;

				device.ResetStates();

				//rs.DepthStencilState = depthBuffer==null? DepthStencilState.None : DepthStencilState.Default ;

				device.SetTargets( frame.DepthBuffer.Surface, frame.HdrBuffer.Surface );

				var viewMatrix = camera.GetViewMatrix( stereoEye );
				var projMatrix = camera.GetProjectionMatrix( stereoEye );

				skyConstsData.MatrixWVP		= scale * rotation * MathUtil.Transformation( viewMatrix.Right, viewMatrix.Up, viewMatrix.Backward ) * projMatrix;
				skyConstsData.SunPosition	= sunPos;
				skyConstsData.SunColor		= sunColor;
				skyConstsData.Turbidity		= settings.SkyTurbidity;
				skyConstsData.Temperature	= Temperature.Get( settings.SunTemperature ); 
				skyConstsData.SkyIntensity	= settings.SkyIntensity;
	
				skyConstsCB.SetData( skyConstsData );
			
				device.VertexShaderConstants[0] = skyConstsCB;
				device.PixelShaderConstants[0] = skyConstsCB;


				//
				//	Sky :
				//
				SkyFlags flags = SkyFlags.SKY;

				ApplyColorSpace( ref flags, settings );
				
				device.PipelineState	=	factory[(int)flags];
						
				device.SetupVertexInput( skyVB, null );
				device.Draw( skyVB.Capacity, 0 );

				device.ResetStates();
			}
		}
	}
}
