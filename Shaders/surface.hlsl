

static const float 	VTVirtualPageCount	= 1024;
//static const float 	VTPhysicalPageCount	= 7;
static const int 	VTPageSize			= 128;
static const int 	VTMaxMip	  		= 6;
//static const float	VTPageScale			= 1024/VTPageSize;

struct BATCH {
	float4x4	Projection		;
	float4x4	View			;
	float4x4	World			;
	float4		ViewPos			;
	float4		BiasSlopeFar	;
	float4		Color;
	float4		ViewBounds		;
	float		VTPageScaleRCP	;
};


struct SUBSET {
	float4	Rectangle;
};



struct VSInput {
	float3 Position : POSITION;
	float3 Tangent 	: TANGENT;
	float3 Binormal	: BINORMAL;
	float3 Normal 	: NORMAL;
	float4 Color 	: COLOR;
	float2 TexCoord : TEXCOORD0;
#ifdef SKINNED
    int4   BoneIndices  : BLENDINDICES0;
    float4 BoneWeights  : BLENDWEIGHTS0;
#endif	
};

struct PSInput {
	float4 	Position 	: SV_POSITION;
	float4 	Color 		: COLOR;
	float2 	TexCoord 	: TEXCOORD0;
	float3	Tangent 	: TEXCOORD1;
	float3	Binormal	: TEXCOORD2;
	float3	Normal 		: TEXCOORD3;
	float4	ProjPos		: TEXCOORD4;
	float3 	WorldPos	: TEXCOORD5;
};

struct GBuffer {
	float4	hdr		 	: SV_Target0;
	float4	gbuffer0 	: SV_Target1;
	float4	gbuffer1 	: SV_Target2;
	float4	feedback	: SV_Target3;
};

cbuffer 		CBBatch 			: 	register(b0) { BATCH    Batch      : packoffset( c0 ); }	
cbuffer 		CBLayer 			: 	register(b1) { SUBSET	Subset     : packoffset( c0 ); }	
cbuffer 		CBBatch 			: 	register(b3) { float4x4 Bones[128] : packoffset( c0 ); }	
SamplerState	SamplerLinear		: 	register(s0);
SamplerState	SamplerPoint		: 	register(s1);
SamplerState	SamplerAnisotropic	: 	register(s2);
Texture2D		Textures[6]			: 	register(t0);

#ifdef _UBERSHADER
$ubershader GBUFFER RIGID|SKINNED
$ubershader SHADOW RIGID|SKINNED
#endif


 
/*-----------------------------------------------------------------------------
	Vertex shader :
	Note on prefixes:
		s - means skinned
		w - means world
		v - means view
		p - means projected
-----------------------------------------------------------------------------*/

float4x3 ToFloat4x3 ( float4x4 m )
{
	return float4x3( m._m00_m10_m20_m30, 
					 m._m01_m11_m21_m31, 
					 m._m02_m11_m22_m32 );
}

float4x4 AccumulateSkin( float4 boneWeights, int4 boneIndices )
{
	float4x4 result = boneWeights.x * Bones[boneIndices.x];
	result = result + boneWeights.y * Bones[boneIndices.y];
	result = result + boneWeights.z * Bones[boneIndices.z];
	result = result + boneWeights.w * Bones[boneIndices.w];
	// float4x3 result = boneWeights.x * ToFloat4x3( Bones[boneIndices.x] );
	// result = result + boneWeights.y * ToFloat4x3( Bones[boneIndices.y] );
	// result = result + boneWeights.z * ToFloat4x3( Bones[boneIndices.z] );
	// result = result + boneWeights.w * ToFloat4x3( Bones[boneIndices.w] );
	return result;
}

float4 TransformPosition( int4 boneIndices, float4 boneWeights, float3 inputPos )
{
	float4 position = 0; 
	
	float4x4 xform  = AccumulateSkin(boneWeights, boneIndices); 
	position = mul( float4(inputPos,1), xform );
	
	return position;
}


float4 TransformNormal( int4 boneIndices, float4 boneWeights, float3 inputNormal )
{
    float4 normal = 0;

	float4x4 xform  = AccumulateSkin(boneWeights, boneIndices); 
	normal = mul( float4(inputNormal,0), xform );
	
	return float4(normal.xyz,0);	// force w to zero
}



PSInput VSMain( VSInput input )
{
	PSInput output;

	#if RIGID
		float4 	pos			=	float4( input.Position, 1 );
		float4	wPos		=	mul( pos,  Batch.World 		);
		float4	vPos		=	mul( wPos, Batch.View 		);
		float4	pPos		=	mul( vPos, Batch.Projection );
		float4	normal		=	mul( float4(input.Normal,0),  Batch.World 		);
		float4	tangent		=	mul( float4(input.Tangent,0),  Batch.World 		);
		float4	binormal	=	mul( float4(input.Binormal,0),  Batch.World 	);
	#endif
	#if SKINNED
		float4 	sPos		=	TransformPosition	( input.BoneIndices, input.BoneWeights, input.Position	);
		float4  sNormal		=	TransformNormal		( input.BoneIndices, input.BoneWeights, input.Normal	);
		float4  sTangent	=	TransformNormal		( input.BoneIndices, input.BoneWeights, input.Tangent	);
		float4  sBinormal	=	TransformNormal		( input.BoneIndices, input.BoneWeights, input.Binormal	);
		
		float4	wPos		=	mul( sPos, Batch.World 		);
		float4	vPos		=	mul( wPos, Batch.View 		);
		float4	pPos		=	mul( vPos, Batch.Projection );
		float4	normal		=	mul( sNormal,  Batch.World 	);
		float4	tangent		=	mul( sTangent,  Batch.World 	);
		float4	binormal	=	mul( sBinormal,  Batch.World 	);
	#endif
	
	output.Position 	= 	pPos;
	output.ProjPos		=	pPos;
	output.Color 		= 	Batch.Color;
	output.TexCoord		= 	input.TexCoord;
	output.Normal		= 	normal.xyz;
	output.Tangent 		=  	tangent.xyz;
	output.Binormal		=  	binormal.xyz;
	output.WorldPos		=	wPos.xyz;
	
	return output;
}


 
/*-----------------------------------------------------------------------------
	Pixel shader :
-----------------------------------------------------------------------------*/

//	https://www.marmoset.co/toolbag/learn/pbr-theory	
//	This means that in theory conductors will not show any evidence of diffuse light. 
//	In practice however there are often oxides or other residues on the surface of a 
//	metal that will scatter some small amounts of light.

//	Blend mode refernce:
//	http://www.deepskycolors.com/archivo/2010/04/21/formulas-for-Photoshop-blending-modes.html	



float MipLevel( float2 uv );

//	https://www.opengl.org/discussion_boards/showthread.php/171485-Texture-LOD-calculation-(useful-for-atlasing)
float MipLevel( float2 uv )
{
	float2 dx = ddx( uv * VTPageSize*VTVirtualPageCount );
	float2 dy = ddy( uv * VTPageSize*VTVirtualPageCount );
	float d = max( dot( dx, dx ), dot( dy, dy ) );

	// Clamp the value to the max mip level counts
	const float rangeClamp = pow(2, (VTMaxMip - 1) * 2);
	d = clamp(d, 1.0, rangeClamp);

	float mipLevel = 0.5 * log2(d);

	return mipLevel;
}



#ifdef GBUFFER
GBuffer PSMain( PSInput input )
{
	GBuffer output;

	float3x3 tbnToWorld	= float3x3(
			input.Tangent.x,	input.Tangent.y,	input.Tangent.z,	
			input.Binormal.x,	input.Binormal.y,	input.Binormal.z,	
			input.Normal.x,		input.Normal.y,		input.Normal.z		
		);
		
	float3	baseColor			=	0.5f;
	float	roughness			=	0.5f;
	float3	localNormal			=	float3(0,0,1);
	float	emission			=	0;
	float 	metallic			=	0;

	input.TexCoord.x	=	mad( input.TexCoord.x, Subset.Rectangle.z, Subset.Rectangle.x );
	input.TexCoord.y	=	mad( input.TexCoord.y, Subset.Rectangle.w, Subset.Rectangle.y );
	
	//---------------------------------
	//	Compute miplevel :
	//---------------------------------
	float mip		=	max(0,floor( MipLevel( input.TexCoord.xy ) ));
	float scale		=	exp2(mip);
	float pageX		=	floor( saturate(input.TexCoord.x) * VTVirtualPageCount / scale );
	float pageY		=	floor( saturate(input.TexCoord.y) * VTVirtualPageCount / scale );
	float dummy		=	1;
	
	float4 feedback	=	 float4( pageX / 1024.0f, pageY / 1024.0f, mip / 1024.0f, dummy / 4.0f );

	//---------------------------------
	//	Virtual texturing stuff :
	//---------------------------------
	float2 vtexTC		=	input.TexCoord;
	float2 atiHack		=	float2(-0.25f/131072, -0.25f/131072) * scale; // <-- float2(0,0) for NVIdia
	//float2 atiHack		=	float2(0,0); // <-- float2(0,0) for NVIdia
	
	float4 fallback		=	float4( 0.5f, 0.5, 0.5f, 1.0f );
	float4 physPageTC	=	Textures[1].SampleLevel( SamplerPoint, input.TexCoord + atiHack, (int)(mip) ).xyzw;
	
	if (physPageTC.w>0) {
		float2 	withinPageTC	=	vtexTC * VTVirtualPageCount / exp2( physPageTC.z );
				withinPageTC	=	frac( withinPageTC );
				withinPageTC	=	withinPageTC * Batch.VTPageScaleRCP;
		
		float2	finalTC			=	physPageTC.xy + withinPageTC;
		
		baseColor	=	Textures[2].Sample( SamplerLinear, finalTC ).rgb;
		localNormal	=	Textures[3].Sample( SamplerLinear, finalTC ).rgb * 2 - 1;
		roughness	=	Textures[4].Sample( SamplerLinear, finalTC ).r;
		metallic	=	Textures[4].Sample( SamplerLinear, finalTC ).g;
		emission	=	Textures[4].Sample( SamplerLinear, finalTC ).b;
	}

	if ( Subset.Rectangle.z==Subset.Rectangle.w && Subset.Rectangle.z==0 ) {
		baseColor	=	0.25f;
		localNormal	=	float3(0,0,1);
		roughness	=	0.5;
		metallic	=	0;
		emission	=	0;
	}

	//---------------------------------
	//	G-buffer output stuff :
	//---------------------------------
	//	NB: Multiply normal length by local normal projection on surface normal.
	//	Shortened normal will be used as Fresnel decay (self occlusion) factor.
	float3 worldNormal 	= 	normalize( mul( localNormal, tbnToWorld ).xyz );
	
	//roughness = 0.5f;
	float3 entityColor	=	input.Color.rgb;
	
	//	Use sRGB texture for better color intensity distribution
	output.hdr			=	float4( emission * entityColor, 0 );		// <-- Multiply on entity color!!!
	output.gbuffer0		=	float4( baseColor, roughness );
	output.gbuffer1 	=	float4( worldNormal * 0.5f + 0.5f, metallic );
	output.feedback		=	feedback;
	
	return output;
}
#endif



#ifdef SHADOW
float4 PSMain( PSInput input ) : SV_TARGET0
{
	float z		= input.ProjPos.z / Batch.BiasSlopeFar.z;

	float dzdx	 = ddx(z);
	float dzdy	 = ddy(z);
	float slope = abs(dzdx) + abs(dzdy);

	return z + Batch.BiasSlopeFar.x + slope * Batch.BiasSlopeFar.y;
}
#endif



