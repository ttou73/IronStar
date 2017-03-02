#define BlockSizeX       16
#define BlockSizeY       16
#define VoxelSize        64
#define VoxelCount       262144

// Fusion.Engine.Graphics.LightRenderer+LightingParams
// Marshal.SizeOf = 656
struct LightingParams {
	float4x4   View;                          // offset:    0
	float4x4   Projection;                    // offset:   64
	float4x4   InverseViewProjection;         // offset:  128
	float4     FrustumVectorTR;               // offset:  192
	float4     FrustumVectorBR;               // offset:  208
	float4     FrustumVectorBL;               // offset:  224
	float4     FrustumVectorTL;               // offset:  240
	float4x4   CSMViewProjection0;            // offset:  256
	float4x4   CSMViewProjection1;            // offset:  320
	float4x4   CSMViewProjection2;            // offset:  384
	float4x4   CSMViewProjection3;            // offset:  448
	float4     ViewPosition;                  // offset:  512
	float4     DirectLightDirection;          // offset:  528
	float4     DirectLightIntensity;          // offset:  544
	float4     ViewportSize;                  // offset:  560
	float4     CSMFilterRadius;               // offset:  576
	float4     AmbientColor;                  // offset:  592
	float4     Viewport;                      // offset:  608
	float      ShowCSLoadOmni;                // offset:  624
	float      ShowCSLoadEnv;                 // offset:  628
	float      ShowCSLoadSpot;                // offset:  632
	int        CascadeCount;                  // offset:  636
	float      CascadeScale;                  // offset:  640
	float      FogDensity;                    // offset:  644
}

// Fusion.Engine.Graphics.LightRenderer+OmniLightGPU
// Marshal.SizeOf = 64
struct OmniLightGPU {
	float4     PositionRadius;                // offset:    0
	float4     Intensity;                     // offset:   16
	float4     ExtentMin;                     // offset:   32
	float4     ExtentMax;                     // offset:   48
}

// Fusion.Engine.Graphics.LightRenderer+EnvLightGPU
// Marshal.SizeOf = 64
struct EnvLightGPU {
	float4     Position;                      // offset:    0
	float4     Dimensions;                    // offset:   16
	float4     ExtentMin;                     // offset:   32
	float4     ExtentMax;                     // offset:   48
}

// Fusion.Engine.Graphics.LightRenderer+SpotLightGPU
// Marshal.SizeOf = 160
struct SpotLightGPU {
	float4x4   ViewProjection;                // offset:    0
	float4     PositionRadius;                // offset:   64
	float4     IntensityFar;                  // offset:   80
	float4     ExtentMin;                     // offset:   96
	float4     ExtentMax;                     // offset:  112
	float4     MaskScaleOffset;               // offset:  128
	float4     ShadowScaleOffset;             // offset:  144
}

// Fusion.Engine.Graphics.LightRenderer+DecalGPU
// Marshal.SizeOf = 208
struct DecalGPU {
	float4x4   DecalMatrixInv;                // offset:    0
	float4     BasisX;                        // offset:   64
	float4     BasisY;                        // offset:   80
	float4     BasisZ;                        // offset:   96
	float4     EmissionRoughness;             // offset:  112
	float4     BaseColorMetallic;             // offset:  128
	float4     ImageScaleOffset;              // offset:  144
	float4     ExtentMin;                     // offset:  160
	float4     ExtentMax;                     // offset:  176
	float      ColorFactor;                   // offset:  192
	float      SpecularFactor;                // offset:  196
	float      NormalMapFactor;               // offset:  200
	float      FalloffFactor;                 // offset:  204
}

