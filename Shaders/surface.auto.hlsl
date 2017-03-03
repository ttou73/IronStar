static const int VTVirtualPageCount = 1024;
static const int VTPageSize = 128;
static const int VTMaxMip = 5;
static const int LightTypeOmni = 1;
static const int LightTypeOmniShadow = 2;
static const int LightTypeSpotShadow = 3;

// Fusion.Engine.Graphics.SceneRenderer+BATCH
// Marshal.SizeOf = 320
struct BATCH {
	float4x4   Projection;                    // offset:    0
	float4x4   View;                          // offset:   64
	float4x4   World;                         // offset:  128
	float4     ViewPos;                       // offset:  192
	float4     BiasSlopeFar;                  // offset:  208
	float4     Color;                         // offset:  224
	float4     ViewBounds;                    // offset:  240
	float      VTPageScaleRCP;                // offset:  256
};

// Fusion.Engine.Graphics.SceneRenderer+SUBSET
// Marshal.SizeOf = 16
struct SUBSET {
	float4     Rectangle;                     // offset:    0
};

// Fusion.Engine.Graphics.SceneRenderer+LIGHTDATA
// Marshal.SizeOf = 656
struct LIGHTDATA {
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
};

// Fusion.Engine.Graphics.SceneRenderer+LIGHT
// Marshal.SizeOf = 256
struct LIGHT {
	float4x4   WorldMatrix;                   // offset:    0
	float4x4   ViewProjection;                // offset:   64
	float4     PositionRadius;                // offset:  128
	float4     IntensityFar;                  // offset:  144
	float4     MaskScaleOffset;               // offset:  160
	float4     ShadowScaleOffset;             // offset:  176
	int        LightType;                     // offset:  192
};

// Fusion.Engine.Graphics.SceneRenderer+DECAL
// Marshal.SizeOf = 176
struct DECAL {
	float4x4   DecalMatrixInv;                // offset:    0
	float4     BasisX;                        // offset:   64
	float4     BasisY;                        // offset:   80
	float4     BasisZ;                        // offset:   96
	float4     EmissionRoughness;             // offset:  112
	float4     BaseColorMetallic;             // offset:  128
	float4     ImageScaleOffset;              // offset:  144
	float      ColorFactor;                   // offset:  160
	float      SpecularFactor;                // offset:  164
	float      NormalMapFactor;               // offset:  168
	float      FalloffFactor;                 // offset:  172
};

