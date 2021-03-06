
#if 0
$ubershader SOLIDLIGHTING|PARTICLES
#endif

static const float PI = 3.141592f;

#pragma warning(disable:3557)

/*-----------------------------------------------------------------------------
	Lighting headers :
	http://simonstechblog.blogspot.ru/2011/12/microfacet-brdf.html
-----------------------------------------------------------------------------*/

#include "brdf.fxi"
#include "fog.fxi"
//#include "lighting.fxi"
#include "lighting.auto.hlsl"

#include "shadows.fxi"
#include "particles.fxi"


/*-----------------------------------------------------------------------------
	Cook-Torrance lighting model
-----------------------------------------------------------------------------*/

cbuffer CBLightingParams : register(b0) { 
	LightingParams Params : packoffset( c0 ); 
};

cbuffer CBSkyOcclusionMatricies : register(b1) { 
	float4x4 SkyOcclusionMatricies[64] : packoffset( c0 ); 
};

struct PS_IN {
    float4 position : SV_POSITION;
	float4 projPos  : TEXCOORD0;
};


SamplerState			SamplerNearestClamp : register(s0);
SamplerState			SamplerLinearClamp : register(s1);
SamplerComparisonState	ShadowSampler	: register(s2);
SamplerState			SamplerLinearWrap : register(s3);

Texture2D 			GBuffer0 			: register(t0);
Texture2D 			GBuffer1	 		: register(t1);
TextureCube			FogTable			: register(t2);
Texture2D 			GBufferDepth 		: register(t4);
Texture2D 			CSMTexture	 		: register(t5);
Texture2D 			SpotShadowMap 		: register(t6);
Texture2D 			SpotMaskAtlas		: register(t7);
StructuredBuffer<OMNILIGHT>	OmniLights	: register(t8);
StructuredBuffer<SPOTLIGHT>	SpotLights	: register(t9);
StructuredBuffer<ENVLIGHT>	EnvLights	: register(t10);
Texture2D 			OcclusionMap		: register(t11);
TextureCubeArray	EnvMap				: register(t12);
StructuredBuffer<PARTICLE> Particles	: register(t13);
Texture2D 			ParticleShadow		: register(t14);
Texture2D 			EnvLut				: register(t15);
StructuredBuffer<DECAL>	Decals			: register(t16);
Texture2D 			DecalImages			: register(t17);


float DepthToViewZ(float depthValue) {
	return Params.Projection[3][2] / (depthValue + Params.Projection[2][2]);
}

/*-----------------------------------------------------------------------------------------------------
	OMNI light
-----------------------------------------------------------------------------------------------------*/
RWTexture2D<float4> hdrTexture  : register(u0); 
RWStructuredBuffer<float4> ParticleLighting : register(u1);

//#ifdef __COMPUTE_SHADER__

//	warning X3584: race condition writing to shared memory detected, note that threads 
//	will be writing the same value, but performance may be diminished due to contention.
groupshared uint minDepthInt = 0xFFFFFFFF; 
groupshared uint maxDepthInt = 0;
groupshared uint visibleDecalCount = 0; 
groupshared uint visibleLightCount = 0; 
groupshared uint visibleLightCountSpot = 0; 
groupshared uint visibleLightCountEnv = 0; 
groupshared uint visibleLightIndices[1024];
groupshared uint sortCount[1024];


#define OMNI_LIGHT_COUNT 1024
#define DECAL_COUNT 1024
#define SPOT_LIGHT_COUNT 16
#define ENV_LIGHT_COUNT 256


void sortIndices ( int n, int maxValue )
{
	#if 1
	uint swap;
	for (int c = 0 ; c < ( n - 1 ); c++) {
		for (int d = 0 ; d < n - c - 1; d++) {
			if (visibleLightIndices[d] > visibleLightIndices[d+1]) {
				swap       				 = visibleLightIndices[d];
				visibleLightIndices[d]   = visibleLightIndices[d+1];
				visibleLightIndices[d+1] = swap;
			}
		}
	}
	#else
	for (int i = 0; i < n; i++) {
		sortCount[visibleLightIndices[i]]++;
	}

	int curIndex = 0;
	for (int i = 0; i < maxValue; i++) {
		while (sortCount[i] > 0) {
			sortCount[i]--;
			visibleLightIndices[curIndex++] = i;
		}
	}
	#endif
}


#ifdef SOLIDLIGHTING
#define BLOCK_SIZE_X 16 
#define BLOCK_SIZE_Y 16 
[numthreads(BLOCK_SIZE_X,BLOCK_SIZE_Y,1)] 
void CSMain( 
	uint3 groupId : SV_GroupID, 
	uint3 groupThreadId : SV_GroupThreadID, 
	uint  groupIndex: SV_GroupIndex, 
	uint3 dispatchThreadId : SV_DispatchThreadID) 
{
	//-----------------------------------------------------
	float width			=	Params.Viewport.z;
	float height		=	Params.Viewport.w;

	int3 location		=	int3( dispatchThreadId.x, dispatchThreadId.y, 0 );

	// WARNING : this reduces performance :
	float3	baseColor	=	GBuffer0.Load( location ).rgb;
	float	roughness	=	GBuffer0.Load( location ).a;
	float3	normal		=	GBuffer1.Load( location ).rgb * 2 - 1;
	float	metallic	=	GBuffer1.Load( location ).a;
	float	depth 	 	=	GBufferDepth.Load( location ).r;
	
	//normal	=	normalize( normal );
	
	//	add half pixel to prevent visual detachment of ssao effect:
	float4 	ssao		=	OcclusionMap	.SampleLevel(SamplerLinearClamp, (location.xy + float2(0.5,0.5))/float2(width,height), 0 );
	
	//normal.xyz			=	normalize(normal.xyz);

	float4	projPos		=	float4( (location.x+0.5f)/(float)width*2-1, (location.y+0.5f)/(float)height*(-2)+1, depth, 1 );
	float4	worldPos	=	mul( projPos, Params.InverseViewProjection );
			worldPos	/=	worldPos.w;
			
	float3	viewDir		=	Params.ViewPosition.xyz - worldPos.xyz;
	float	viewDistance=	length( viewDir );
	float3	viewDirN	=	normalize( viewDir );
	
	float4	totalLight	=	0;

	
	
	//-----------------------------------------------------
	//	Common tile-related stuff :
	//-----------------------------------------------------
	
	GroupMemoryBarrierWithGroupSync();

	uint depthInt = asuint(depth); 

	InterlockedMin(minDepthInt, depthInt); 
	InterlockedMax(maxDepthInt, depthInt); 
	GroupMemoryBarrierWithGroupSync(); 
	
	float minGroupDepth = asfloat(minDepthInt); 
	float maxGroupDepth = asfloat(maxDepthInt);	
	
	//-----------------------------------------------------
	//	Apply decals :
	//-----------------------------------------------------
	
	if (1) {
		uint decalCount = DECAL_COUNT;
		
		uint threadCount = BLOCK_SIZE_X * BLOCK_SIZE_Y; 
		uint passCount = (decalCount+threadCount-1) / threadCount;
		
		for (uint passIt = 0; passIt < passCount; passIt++ ) {
		
			uint decalIndex = passIt * threadCount + groupIndex;
			
			DECAL dcl = Decals[decalIndex];
			sortCount[decalIndex] = 0;
			
			float3 tileMin = float3( groupId.x*BLOCK_SIZE_X,    		  groupId.y*BLOCK_SIZE_Y,    			minGroupDepth);
			float3 tileMax = float3( groupId.x*BLOCK_SIZE_X+BLOCK_SIZE_X, groupId.y*BLOCK_SIZE_Y+BLOCK_SIZE_Y, 	maxGroupDepth);
			
			if ( dcl.ExtentMax.x > tileMin.x && tileMax.x > dcl.ExtentMin.x 
			  && dcl.ExtentMax.y > tileMin.y && tileMax.y > dcl.ExtentMin.y 
			  && dcl.ExtentMax.z > tileMin.z && tileMax.z > dcl.ExtentMin.z ) 
			{
				uint offset; 
				InterlockedAdd(visibleDecalCount, 1, offset); 
				visibleLightIndices[offset] = decalIndex;
			}
		}
		
		GroupMemoryBarrierWithGroupSync();
				
		totalLight.rgb += visibleDecalCount * float3(0.5, 0.0, 0.0) * 0;
		
		if (groupThreadId.x==0 && groupThreadId.y==0) {
			sortIndices( visibleDecalCount, DECAL_COUNT);
		}
		
		GroupMemoryBarrierWithGroupSync();

		for (uint i = 0; i < visibleDecalCount; i++) {
		
			uint decalIndex = visibleLightIndices[i];
			DECAL decal = Decals[decalIndex];

			float4x4 decalMatrixI	=	decal.DecalMatrixInv;
			float3	 decalColor		=	decal.BaseColorMetallic.rgb;
			float3	 glowColor		=	decal.EmissionRoughness.rgb;
			float3	 decalR			=	decal.EmissionRoughness.a;
			float3	 decalM			=	decal.BaseColorMetallic.a;
			float4	 scaleOffset	=	decal.ImageScaleOffset;
			float	 falloff		=	decal.FalloffFactor;
			
			float4 decalPos	=	mul(worldPos, decalMatrixI);
			
			if ( abs(decalPos.x)<1 && abs(decalPos.y)<1 && abs(decalPos.z)<1 ) {
			
				//float2 uv			=	mad(mad(decalPos.xy, float2(-0.5,0.5), float2(0.5,0.5), offsetScale.zw, offsetScale.xy); 
				float2 uv			=	mad(decalPos.xy, scaleOffset.xy, scaleOffset.zw); 
			
				float4 decalImage	= 	DecalImages.SampleLevel( SamplerLinearClamp, uv, 0 );
				float3 localNormal  = 	decalImage.xyz * 2 - 1;
				float3 decalNormal	=	localNormal.x * decal.BasisX + localNormal.y * decal.BasisY - localNormal.z * decal.BasisZ;
				float factor		=	decalImage.a * saturate(falloff - abs(decalPos.z)*falloff);
				
				totalLight.rgb	+=	 glowColor * factor;
			
				baseColor 	= lerp( baseColor.rgb, decalColor, decal.ColorFactor * factor );
				roughness 	= lerp( roughness, decalR, decal.SpecularFactor * factor );
				metallic 	= lerp( metallic,  decalM, decal.SpecularFactor * factor );
				//normal		= lerp( normal, decalNormal, decal.NormalMapFactor * factor );

				normal		= normal + decalNormal * decal.NormalMapFactor * factor;
			}
		}
		
	}

	
	//-----------------------------------------------------
	//	Compute diffuse and specular color :
	//-----------------------------------------------------

	normal 				=	normalize( normal );
	float3	diffuse 	=	lerp( baseColor, float3(0,0,0), metallic );
	float3	specular  	=	lerp( float3(0.04f,0.04f,0.04f), baseColor, metallic );
	
	// totalLight.xyz = normal.xyz*0.5+0.5;
	// hdrTexture[dispatchThreadId.xy] = totalLight;
	
	// return;
	
	//-----------------------------------------------------
	//	Direct light :
	//-----------------------------------------------------
	float3 csmFactor	=	ComputeCSM( worldPos, Params, ShadowSampler, SamplerLinearClamp, CSMTexture, ParticleShadow, true );
	float3 lightDir		=	-normalize(Params.DirectLightDirection.xyz);
	float3 lightColor	=	Params.DirectLightIntensity.rgb;
	
	float	nDotL		=	saturate( dot(normal.xyz, lightDir) );
	float3 diffuseTerm	=	Lambert	( normal.xyz,  lightDir, lightColor, float3(1,1,1) );
	float3 diffuseTerm2	=	Lambert	( normal.xyz,  lightDir, lightColor, float3(1,1,1), 1 );
	totalLight.xyz		+=	csmFactor.rgb * diffuseTerm * diffuse.rgb;
	totalLight.xyz		+=	csmFactor.rgb * nDotL * CookTorrance( normal.xyz,  viewDirN, lightDir, lightColor, specular, roughness );

	//-----------------------------------------------------
	//	OMNI LIGHTS :
	//-----------------------------------------------------
	
	if (1) {
		uint lightCount = OMNI_LIGHT_COUNT;
		
		uint threadCount = BLOCK_SIZE_X * BLOCK_SIZE_Y; 
		uint passCount = (lightCount+threadCount-1) / threadCount;
		
		for (uint passIt = 0; passIt < passCount; passIt++ ) {
		
			uint lightIndex = passIt * threadCount + groupIndex;
			
			OMNILIGHT ol = OmniLights[lightIndex];
			
			float3 tileMin = float3( groupId.x*BLOCK_SIZE_X,    		  groupId.y*BLOCK_SIZE_Y,    			minGroupDepth);
			float3 tileMax = float3( groupId.x*BLOCK_SIZE_X+BLOCK_SIZE_X, groupId.y*BLOCK_SIZE_Y+BLOCK_SIZE_Y, 	maxGroupDepth);
			
			if ( ol.ExtentMax.x > tileMin.x && tileMax.x > ol.ExtentMin.x 
			  && ol.ExtentMax.y > tileMin.y && tileMax.y > ol.ExtentMin.y 
			  && ol.ExtentMax.z > tileMin.z && tileMax.z > ol.ExtentMin.z ) 
			{
				uint offset; 
				InterlockedAdd(visibleLightCount, 1, offset); 
				visibleLightIndices[offset] = lightIndex;
			}
		}
		
		GroupMemoryBarrierWithGroupSync();
				
		totalLight.rgb += visibleLightCount * float3(0.5, 0.0, 0.0) * Params.ShowCSLoadOmni;
		
		for (uint i = 0; i < visibleLightCount; i++) {
		
			uint lightIndex = visibleLightIndices[i];
			OMNILIGHT light = OmniLights[lightIndex];

			float3 intensity = light.Intensity.rgb;
			float3 position	 = light.PositionRadius.rgb;
			float  radius    = light.PositionRadius.w;
			float3 lightDir	 = position - worldPos.xyz;
			float  falloff	 = LinearFalloff( length(lightDir), radius );
			float  nDotL	 = saturate( dot(normal, normalize(lightDir)) );
			
			totalLight.rgb += falloff * Lambert ( normal.xyz,  lightDir, intensity, diffuse );
			totalLight.rgb += falloff * nDotL * CookTorrance( normal.xyz, viewDirN, lightDir, intensity, specular, roughness );
		}
	}
	
	//-----------------------------------------------------
	//	ENVIRONMENT LIGHTS / RADIANCE CACHE :
	//-----------------------------------------------------
	
	GroupMemoryBarrierWithGroupSync();

	if (1) {
		uint lightCount = ENV_LIGHT_COUNT;
		
		uint threadCount = BLOCK_SIZE_X * BLOCK_SIZE_Y; 
		uint passCount = (lightCount+threadCount-1) / threadCount;
		
		for (uint passIt = 0; passIt < passCount; passIt++ ) {
		
			uint lightIndex = passIt * threadCount + groupIndex;
			
			ENVLIGHT el = EnvLights[lightIndex];
			
			float3 tileMin = float3( groupId.x*BLOCK_SIZE_X,    		  groupId.y*BLOCK_SIZE_Y,    			minGroupDepth);
			float3 tileMax = float3( groupId.x*BLOCK_SIZE_X+BLOCK_SIZE_X, groupId.y*BLOCK_SIZE_Y+BLOCK_SIZE_Y, 	maxGroupDepth);
			
			if ( el.ExtentMax.x > tileMin.x && tileMax.x > el.ExtentMin.x 
			  && el.ExtentMax.y > tileMin.y && tileMax.y > el.ExtentMin.y 
			  && el.ExtentMax.z > tileMin.z && tileMax.z > el.ExtentMin.z ) 
			{
				uint offset; 
				InterlockedAdd(visibleLightCountEnv, 1, offset); 
				visibleLightIndices[offset] = lightIndex;
			}
		}
		
		GroupMemoryBarrierWithGroupSync();
				
		totalLight.rgb += visibleLightCountEnv * float3(0.0, 0.5, 0.0) * Params.ShowCSLoadEnv;
		
		float4 totalEnvContrib = float4(0,0,0,0.001f);

		for (uint i = 0; i < visibleLightCountEnv; i++) {
		
			uint lightIndex = visibleLightIndices[i];
			ENVLIGHT light = EnvLights[lightIndex];

			float3 position	 = light.Position.rgb;
			float3 dims      = light.Dimensions.xyz;
			float  factor	 = light.Dimensions.w;
			float3 lightDir	 = position.xyz - worldPos.xyz;
			
			float3  falloff3 = abs((position.xyz - worldPos.xyz) / dims.xyz)*2;
			float   falloff  = pow(saturate(1 - max( max(falloff3.x, falloff3.y), falloff3.z )),1);
			
			totalEnvContrib.w 	+= falloff;
			
			//	bad simulation of diffuse light
			totalEnvContrib.xyz	+=	EnvMap.SampleLevel( SamplerLinearClamp, float4(normal.xyz, lightIndex), 4).rgb * diffuse * falloff * ssao.rgb;

			float	NoV = dot(viewDirN, normal.xyz);

			float2 ab	=	EnvLut.SampleLevel( SamplerLinearClamp, float2(roughness, 1-NoV), 0 ).xy;
			float3 env	=	EnvMap.SampleLevel( SamplerLinearClamp, float4(reflect(-viewDir, normal.xyz), lightIndex), sqrt(roughness)*6 ).rgb;

			totalEnvContrib.xyz	+=	env * ( specular * ab.x + ab.y ) * falloff * ssao.rgb;
		}
		
		totalLight.rgb += (totalEnvContrib.xyz / totalEnvContrib.w);
	}

	//-----------------------------------------------------
	//	SPOT LIGHTS :
	//-----------------------------------------------------
	
	GroupMemoryBarrierWithGroupSync();
	
	if (1) {
		uint lightCount = SPOT_LIGHT_COUNT;
		uint lightIndex = groupIndex;
		
		SPOTLIGHT ol = SpotLights[lightIndex];
		
		float3 tileMin = float3( groupId.x*BLOCK_SIZE_X,    		  groupId.y*BLOCK_SIZE_Y,    			minGroupDepth);
		float3 tileMax = float3( groupId.x*BLOCK_SIZE_X+BLOCK_SIZE_X, groupId.y*BLOCK_SIZE_Y+BLOCK_SIZE_Y, 	maxGroupDepth);
		
		if ( lightIndex < 16 
		  && ol.ExtentMax.x > tileMin.x && tileMax.x > ol.ExtentMin.x 
		  && ol.ExtentMax.y > tileMin.y && tileMax.y > ol.ExtentMin.y 
		  && ol.ExtentMax.z > tileMin.z && tileMax.z > ol.ExtentMin.z )
		{
			uint offset; 
			InterlockedAdd(visibleLightCountSpot, 1, offset); 
			visibleLightIndices[offset] = lightIndex;
		}
		
		GroupMemoryBarrierWithGroupSync();
				
		totalLight.rgb += visibleLightCountSpot * float3(0, 0.0, 0.5)  * Params.ShowCSLoadSpot;

		for (uint i = 0; i < visibleLightCountSpot; i++) {
		
			uint lightIndex = visibleLightIndices[i];
			SPOTLIGHT light = SpotLights[lightIndex];

			float3 intensity = light.IntensityFar.rgb;
			float3 position	 = light.PositionRadius.rgb;
			float  radius    = light.PositionRadius.w;
			float3 lightDir	 = position - worldPos.xyz;
			float  falloff	 = LinearFalloff( length(lightDir), radius );
			float  nDotL	 = saturate( dot(normal, normalize(lightDir)) );
			
			float3 shadow	 = ComputeSpotShadow( worldPos, light, ShadowSampler, SamplerLinearClamp, SpotShadowMap, SpotMaskAtlas, Params.CSMFilterRadius.x );
			
			totalLight.rgb += shadow * falloff * Lambert ( normal.xyz,  lightDir, intensity, diffuse.rgb );
			totalLight.rgb += shadow * falloff * nDotL * CookTorrance( normal.xyz, viewDirN, lightDir, intensity, specular, roughness );
		}
	}

	//-----------------------------------------------------
	//	Ambient :
	//-----------------------------------------------------
	
	totalLight.rgb += Params.AmbientColor * diffuse.rgb * ssao.rgb;
	
	float3 fogColor	=	FogTable.SampleLevel( SamplerLinearClamp, -viewDirN, 0 ).rgb;
	
	if (depth<0.9999f) {
		totalLight.rgb = ApplyFogColor( totalLight.rgb, Params.FogDensity, viewDistance, fogColor );
	}
	
	hdrTexture[dispatchThreadId.xy] = totalLight;
}
#endif

/*-----------------------------------------------------------------------------------------------------
	Direct Light
-----------------------------------------------------------------------------------------------------*/

#ifdef PARTICLES
#define BLOCK_SIZE 256
[numthreads(BLOCK_SIZE,1,1)] 
void CSMain( 
	uint3 groupId : SV_GroupID, 
	uint3 groupThreadId : SV_GroupThreadID, 
	uint  groupIndex: SV_GroupIndex, 
	uint3 dispatchThreadId : SV_DispatchThreadID) 
{
		int id = dispatchThreadId.x;

	#if 1
		float4	worldPos	=	float4(Particles[id].Position, 1);
		float3	viewDir		=	Params.ViewPosition.xyz - worldPos.xyz;
		float	viewDistance=	length( viewDir );
		float3	viewDirN	=	normalize( viewDir );
		
		//
		//	Direct light :
		//
		float3 csmFactor	=	ComputeCSM( worldPos, Params, ShadowSampler, SamplerLinearClamp, CSMTexture, ParticleShadow, false );
		float3 lightDir		=	-normalize(Params.DirectLightDirection.xyz);
		float3 lightColor	=	Params.DirectLightIntensity.rgb;
		
		float3 totalPrtLight	=	lightColor * csmFactor;

		#if 1
		//
		//	Spot lights :
		//
		int i;
		for (i=0; i<SPOT_LIGHT_COUNT; i++) {
			SPOTLIGHT light = SpotLights[i];

			float3 intensity = light.IntensityFar.rgb;
			float3 position	 = light.PositionRadius.rgb;
			float  radius    = light.PositionRadius.w;
			float3 lightDir	 = position - worldPos.xyz;
			float  falloff	 = LinearFalloff( length(lightDir), radius );
			
			float3 shadow	 = ComputeSpotShadow( worldPos, light, ShadowSampler, SamplerLinearClamp, SpotShadowMap, SpotMaskAtlas, Params.CSMFilterRadius.x );
			
			totalPrtLight	+= shadow * falloff * intensity;
		}

		for (i=0; i<OMNI_LIGHT_COUNT; i++) {
			OMNILIGHT light = OmniLights[i];

			float3 intensity = light.Intensity.rgb;
			float3 position	 = light.PositionRadius.rgb;
			float  radius    = light.PositionRadius.w;
			float3 lightDir	 = position - worldPos.xyz;
			float  falloff	 = LinearFalloff( length(lightDir), radius );
			
			totalPrtLight.xyz	+=	intensity * falloff;
		}
		
		//
		//	Ambient light :
		//
		#if 0
		for (i=0; i<ENV_LIGHT_COUNT; i++) {
			ENVLIGHT light = EnvLights[i];

			float3 position	 = light.Position.rgb;
			float  radius    = light.InnerOuterRadius.y;
			float3 lightDir	 = position.xyz - worldPos.xyz;
			float  falloff	 = LinearFalloff( length(lightDir), radius );
			
			float3 	envFactor	=	0;
					envFactor	+=	EnvMap.SampleLevel( SamplerLinearClamp, float4(float3(1,0,0), i), 6).rgb;
					envFactor	+=	EnvMap.SampleLevel( SamplerLinearClamp, float4(float3(0,1,0), i), 6).rgb;
					envFactor	+=	EnvMap.SampleLevel( SamplerLinearClamp, float4(float3(0,0,1), i), 6).rgb;
					envFactor	+=	EnvMap.SampleLevel( SamplerLinearClamp, float4(float3(-1,0,0), i), 6).rgb;
					envFactor	+=	EnvMap.SampleLevel( SamplerLinearClamp, float4(float3(0,-1,0), i), 6).rgb;
					envFactor	+=	EnvMap.SampleLevel( SamplerLinearClamp, float4(float3(0,0,-1), i), 6).rgb;//*/
							  
			totalPrtLight.xyz	+=	envFactor * falloff / 6;
		}
		#endif
		
		#endif
		
		//
		//	Apply fog :
		//
		//float  fogAlpha	=	ApplyFogAlpha( Params.FogDensity, viewDistance );

		ParticleLighting[id] = 	float4( totalPrtLight, 1 );

	#else
		ParticleLighting[id]	=	float4(1,1,1,1);
	#endif
	
}

#endif







