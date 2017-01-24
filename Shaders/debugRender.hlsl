/*-----------------------------------------------------------------------------
	Debug render shader :
-----------------------------------------------------------------------------*/

#if 0
$ubershader SOLID|GHOST
#endif

struct BATCH {
	float4x4	View;
	float4x4	Projection;
	float4		ViewPosition;
	float4		PixelSize;
};

cbuffer CBBatch : register(b0) { BATCH Batch : packoffset( c0 ); }

struct VS_IN {
	float4 pos : POSITION;
	float4 col : COLOR;
};

struct GS_IN {
	float4 pos : POSITION;
	float4 col : COLOR;
	float  wth : TEXCOORD0;
};

struct PS_IN {
	float4 pos : SV_POSITION;
	float4 col : COLOR;
};


GS_IN VSMain( VS_IN input )
{
	GS_IN output = (GS_IN)0;
	
	output.pos = float4(input.pos.xyz,1);
	output.col = input.col;
	output.wth = input.pos.w;
	
	return output;
}


void ClipLine ( float znear, inout float3 a, inout float3 b ) 
{
	if ( a.z <= znear && b.z <= znear ) {
		return;
	}
	if ( a.z >= znear && b.z >= znear ) {
		return;
	}

	float	factor	=	( znear - a.z ) / ( b.z - a.z );
	float3 	cpoint	=	lerp( a, b, factor );
	
	if ( a.z > znear ) a = cpoint;
	if ( b.z > znear ) b = cpoint;
}


[maxvertexcount(6)]
void GSMain( line GS_IN inputPoint[2], inout TriangleStream<PS_IN> outputStream )
{
	PS_IN p0, p1, p2, p3;

	float4 color0	=	inputPoint[0].col;
	float4 color1	=	inputPoint[1].col;
	
	float4 vpos0	=	mul( float4(inputPoint[0].pos.xyz,1), Batch.View );
	float4 vpos1	=	mul( float4(inputPoint[1].pos.xyz,1), Batch.View );
	
	if (vpos1.z * vpos0.z < 0) {
		ClipLine( 0, vpos0.xyz, vpos1.xyz );
	}

	float4 ppos0  	=	mul( vpos0, Batch.Projection );
	float4 ppos1  	=	mul( vpos1, Batch.Projection );

	float3 	nppos0	=	ppos0.xyz / ppos0.w;
	float3 	nppos1	=	ppos1.xyz / ppos1.w;
	
	float2 	dir		=	normalize( (nppos1.xy - nppos0.xy) * Batch.PixelSize.wz );
	
	if (abs(dir.x)>abs(dir.y)) {
		dir = float2(1,0);
	} else {
		dir = float2(0,1);
	}
	
	float4	side	=	float4( dir.y, dir.x, 0, 0 ) * Batch.PixelSize.zwxx;

	float 	sz0		=	max(1,inputPoint[0].wth);
	float 	sz1		=	max(1,inputPoint[1].wth);

	float4 	pos0	=	ppos1 - ( side * ppos1.w * sz1 );
	float4 	pos1	=	ppos0 - ( side * ppos0.w * sz0 );
	float4 	pos2	=	ppos0 + ( side * ppos0.w * sz0 );
	float4 	pos3	=	ppos1 + ( side * ppos1.w * sz1 );
	
	
	p0.pos = pos0;
	p0.col = color1;
	
	p1.pos = pos1;
	p1.col = color0;
	
	p2.pos = pos2;
	p2.col = color0;
	
	p3.pos = pos3;
	p3.col = color1;

	
	outputStream.Append(p0);
	outputStream.Append(p1);
	outputStream.Append(p2);
	
	outputStream.RestartStrip();

	outputStream.Append(p0);
	outputStream.Append(p2);
	outputStream.Append(p3);

	outputStream.RestartStrip();
}



float4 PSMain( PS_IN input, float4 vpos : SV_Position ) : SV_Target
{
	#ifdef SOLID
	return float4(input.col.rgb,1);
	#endif
	#ifdef GHOST
		clip((vpos.x+vpos.y)%2-1);
		return float4(input.col.rgb,1);
	#endif
}




