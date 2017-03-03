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

	[RequireShader("surface", true)]
	internal partial class SceneRenderer : RenderComponent {

		enum SurfaceFlags {
			FORWARD	=	1 << 0,
			SHADOW	=	1 << 1,
			RIGID	=	1 << 4,
			SKINNED	=	1 << 5,
		}


		[ShaderDefine]	const int VTVirtualPageCount	=	VTConfig.VirtualPageCount;
		[ShaderDefine]	const int VTPageSize			=	VTConfig.PageSize;
		[ShaderDefine]	const int VTMaxMip				=	VTConfig.MaxMipLevel;

		[ShaderDefine]	const int LightTypeOmni			=	1;
		[ShaderDefine]	const int LightTypeOmniShadow	=	2;
		[ShaderDefine]	const int LightTypeSpotShadow	=	3;


		[ShaderStructure]
		[StructLayout(LayoutKind.Explicit, Size=320)]
		struct BATCH {
			[FieldOffset(  0)] public Matrix	Projection;
			[FieldOffset( 64)] public Matrix	View;
			[FieldOffset(128)] public Matrix	World;
			[FieldOffset(192)] public Vector4	ViewPos			;
			[FieldOffset(208)] public Vector4	BiasSlopeFar	;
			[FieldOffset(224)] public Color4	Color;
			[FieldOffset(240)] public Vector4	ViewBounds;
			[FieldOffset(256)] public float		VTPageScaleRCP;
		}


		[ShaderStructure]
		struct SUBSET {
			public Vector4 Rectangle;
		}


		[ShaderStructure]
		[StructLayout(LayoutKind.Explicit, Size=656)]
		struct LIGHTDATA {
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
		[StructLayout(LayoutKind.Sequential, Size=256)]
		public struct LIGHT {	
			public Matrix	WorldMatrix;
			public Matrix	ViewProjection;
			public Vector4	PositionRadius;
			public Vector4	IntensityFar;
			public Vector4	MaskScaleOffset;
			public Vector4	ShadowScaleOffset;
			public int		LightType;
		}



		[ShaderStructure]
		public struct DECAL {
			public Matrix	DecalMatrixInv;
			public Vector4 	BasisX;
			public Vector4 	BasisY;
			public Vector4 	BasisZ;
			public Vector4 	EmissionRoughness;
			public Vector4	BaseColorMetallic;
			public Vector4	ImageScaleOffset;
			public float	ColorFactor;
			public float	SpecularFactor;
			public float	NormalMapFactor;
			public float	FalloffFactor;
		}
	}
}
