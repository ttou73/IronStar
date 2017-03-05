
/*-----------------------------------------------------------------------------
	Clustered lighting rendering :
-----------------------------------------------------------------------------*/

#include "brdf.fxi"

//
//	ComputeClusteredLighting
//	
float3 ComputeClusteredLighting ( PSInput input, Texture3D<uint2> clusterTable, float2 vpSize, float3 baseColor, float3 worldNormal, float roughness, float metallic )
{
	float3 result		=	float3(0,0,0);
	float slice			= 	1 - exp(-input.ProjPos.w*0.03);
	int3 loadUVW		=	int3( input.Position.xy/vpSize*float2(16,8), slice * 24 );
	
	uint2	data		=	clusterTable.Load( int4(loadUVW,0) ).rg;
	uint	index		=	data.r;
	uint 	lightCount	=	data.g & 0xFFFF;
	uint 	decalCount	=	data.g >> 16;
	
	float3 totalLight	=	0;

	float3 	worldPos	= 	input.WorldPos.xyz;
	float3 	normal 		=	normalize( worldNormal );
	float3	diffuse 	=	lerp( baseColor, float3(0,0,0), metallic );
	float3	specular  	=	lerp( float3(0.04f,0.04f,0.04f), baseColor, metallic );
	
	float3	viewDir		=	Batch.ViewPos.xyz - worldPos.xyz;
	float	viewDistance=	length( viewDir );
	float3	viewDirN	=	normalize( viewDir );
	
	[loop]
	for (uint i=0; i<lightCount; i++) {
		uint idx = LightIndexTable.Load( index + i );
		float3 position		=	LightDataTable[idx].PositionRadius.xyz;
		float  radius		=	LightDataTable[idx].PositionRadius.w;
		float3 intensity	=	LightDataTable[idx].IntensityFar.rgb;
		
		float3 lightDir		= 	position - worldPos.xyz;
		float  falloff		= 	LinearFalloff( length(lightDir), radius );
		float  nDotL		= 	saturate( dot(normal, normalize(lightDir)) );
		
		totalLight.rgb 		+= 	falloff * Lambert ( normal.xyz,  lightDir, intensity, diffuse );
		totalLight.rgb 		+= 	falloff * nDotL * CookTorrance( normal.xyz, viewDirN, lightDir, intensity, specular, roughness );
	}
	
	return totalLight;
}

