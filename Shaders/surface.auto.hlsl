static const int VTVirtualPageCount = 1024;
static const int VTPageSize = 128;
static const int VTMaxMip = 5;

// Fusion.Engine.Graphics.SceneRenderer+CBMeshInstanceData
// Marshal.SizeOf = 320
struct CBMeshInstanceData {
	float4x4   Projection;                    // offset:    0
	float4x4   View;                          // offset:   64
	float4x4   World;                         // offset:  128
	float4     ViewPos;                       // offset:  192
	float4     BiasSlopeFar;                  // offset:  208
	float4     Color;                         // offset:  224
	float4     ViewBounds;                    // offset:  240
	float      VTPageScaleRCP;                // offset:  256
};

// Fusion.Engine.Graphics.SceneRenderer+CBSubsetData
// Marshal.SizeOf = 16
struct CBSubsetData {
	float4     Rectangle;                     // offset:    0
};

