

#if 0
$ubershader		(TONEMAPPING LINEAR|REINHARD|FILMIC)|MEASURE_ADAPT
#endif

struct PARAMS {
	float 	AdaptationRate;
	float 	LuminanceLowBound;
	float	LuminanceHighBound;
	float	KeyValue;
	float	BloomAmount;
	float	DirtMaskLerpFactor;
	float	DirtAmount;
	float	Saturation;
	float	Maximum;
	float	Minimum;
};

Texture2D		SourceHdrImage 		: register(t0);
Texture2D		MasuredLuminance	: register(t1);
Texture2D		BloomTexture		: register(t2);
Texture2D		BloomMask1			: register(t3);
Texture2D		BloomMask2			: register(t4);
Texture2D		NoiseTexture		: register(t5);
SamplerState	LinearSampler		: register(s0);
	
cbuffer PARAMS 		: register(b0) { 
	PARAMS Params 	: packoffset(c0); 
};


/*
**	FSQuad
*/
float4 FSQuad( uint VertexID ) 
{
	return float4((VertexID == 0) ? 3.0f : -1.0f, (VertexID == 2) ? 3.0f : -1.0f, 1.0f, 1.0f);
}

float2 FSQuadUV ( uint VertexID )
{
	return float2((VertexID == 0) ? 2.0f : -0.0f, 1-((VertexID == 2) ? 2.0f : -0.0f));
}

/*-----------------------------------------------------------------------------
	Luminance Measurement and Adaptation:
	Assumed 128x128 input image.
-----------------------------------------------------------------------------*/
#if MEASURE_ADAPT

float4 VSMain(uint VertexID : SV_VertexID) : SV_POSITION
{
	return FSQuad( VertexID );
}


float4 PSMain(float4 position : SV_POSITION) : SV_Target
{
	float sumLum = 0;
	const float3 lumVector = float3(0.213f, 0.715f, 0.072f );
	
	float oldLum = MasuredLuminance.Load(int3(0,0,0)).r;
	
	for (int x=0; x<32; x++) {
		for (int y=0; y<32; y++) {
			
			sumLum += log( dot( lumVector, SourceHdrImage.Load(int3(x,y,3)).rgb ) + 0.0001f );
		}
	}
	sumLum = clamp( exp(sumLum / 1024.0f), Params.LuminanceLowBound, Params.LuminanceHighBound );
	
	return lerp( oldLum, max(0.5,min(100,sumLum)), Params.AdaptationRate );
}

#endif


/*-----------------------------------------------------------------------------
	Tonemapping and Final Composing :
-----------------------------------------------------------------------------*/

#ifdef TONEMAPPING

float4 VSMain(uint VertexID : SV_VertexID, out float2 uv : TEXCOORD) : SV_POSITION
{
	uv = FSQuadUV( VertexID );
	return FSQuad( VertexID );
}

static const float dither[4][4] = {{1,9,3,11},{13,5,15,7},{4,12,2,10},{16,8,14,16}};


float3 Dither ( int xpos, int ypos, float3 color )
{
	color += dither[(xpos+ypos/7)%4][(ypos+xpos/7)%4]/256.0f/5;
	color -= dither[(ypos+xpos/7)%4][(xpos+ypos/7)%4]/256.0f/5;//*/
	return color;
}


float4 PSMain(float4 position : SV_POSITION, float2 uv : TEXCOORD0 ) : SV_Target
{
	uint width;
	uint height;
	uint xpos = position.x;
	uint ypos = position.y;
	SourceHdrImage.GetDimensions( width, height );

	//
	//	Read images :
	//
	float3	hdrImage	=	SourceHdrImage.SampleLevel( LinearSampler, uv, 0 ).rgb;
	float3	bloom0		=	BloomTexture  .SampleLevel( LinearSampler, uv, 0 ).rgb;
	float3	bloom1		=	BloomTexture  .SampleLevel( LinearSampler, uv, 1 ).rgb;
	float3	bloom2		=	BloomTexture  .SampleLevel( LinearSampler, uv, 2 ).rgb;
	float3	bloom3		=	BloomTexture  .SampleLevel( LinearSampler, uv, 3 ).rgb;
	float4	bloomMask1	=	BloomMask1.SampleLevel( LinearSampler, uv, 0 );
	float4	bloomMask2	=	BloomMask2.SampleLevel( LinearSampler, uv, 0 );
	float4	bloomMask	=	lerp( bloomMask1, bloomMask2, Params.DirtMaskLerpFactor );
	float	luminance	=	MasuredLuminance.Load(int3(0,0,0)).r;
	float	noiseDither	=	NoiseTexture.Load( int3(xpos%64,ypos%64,0) ).r;

	float3	bloom		=	( bloom0 * 1.000f  
							+ bloom1 * 2.000f  
							+ bloom2 * 3.000f  
							+ bloom3 * 4.000f )/10.000f;//*/
					
	bloom	*=	bloomMask.rgb;
	
	hdrImage			=	lerp( hdrImage * bloomMask.rgb, bloom, saturate(bloomMask.a * Params.DirtAmount + Params.BloomAmount));
	

	//
	//	Tonemapping :
	//	
	float3	exposured	=	Params.KeyValue * hdrImage / luminance;

	#ifdef LINEAR
		float3 	tonemapped	=	pow( abs(exposured), 1/2.2f );
	#endif
	
	#ifdef REINHARD
		float3 tonemapped	=	pow( abs(exposured / (1+exposured)), 1/2.2f );
	#endif
	
	#ifdef FILMIC
		float3 x = max(0,exposured-0.004);
		float3 tonemapped = (x*(6.2*x+.5))/(x*(6.2*x+1.7)+0.06);
	#endif

	float desaturated	=	dot( tonemapped, float3(0.3f,0.6f,0.2f));
	
	tonemapped		=	lerp( float3(desaturated,desaturated,desaturated), tonemapped, Params.Saturation );
	
	tonemapped.r	=	lerp( Params.Minimum, Params.Maximum, tonemapped.r );
	tonemapped.g	=	lerp( Params.Minimum, Params.Maximum, tonemapped.g );
	tonemapped.b	=	lerp( Params.Minimum, Params.Maximum, tonemapped.b );
	
	//tonemapped		=	noiseDither;
	tonemapped		+=	(noiseDither*2-1)*3/256.0;
	//tonemapped	=	Dither( xpos, ypos, tonemapped );

	return  float4( tonemapped, desaturated );
}

#endif











