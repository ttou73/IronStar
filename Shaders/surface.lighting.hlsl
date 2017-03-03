
/*-----------------------------------------------------------------------------
	Clustered lighting rendering :
-----------------------------------------------------------------------------*/

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
	
	return float3(data.rg/100.0f,0);
	
	//result.r	=	omniCount;
	
	//return result;

	/*[loop]
	for (uint i=0; i<omniCount; i++) {
		uint idx = IndexLightTable.Load( index + i );
		float3 position	=	LightParams[idx*2+0].xyz;
		float  radius	=	LightParams[idx*2+0].w;
		float3 color	=	LightParams[idx*2+1].rgb;
		
		result			+=	ComputeLighting( input, surf, wsNormal, position, color, radius, false );
	}//*/
	
	//return result;
}

