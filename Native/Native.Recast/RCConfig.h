#pragma once

using namespace System::ComponentModel;
using namespace Fusion::Core::Mathematics;

namespace Native {
	namespace Recast {

		public ref class RCConfig {

		public:

			/// The width of the field along the x-axis. [Limit: >= 0] [Units: vx]
			[CategoryAttribute("Field")]
			property int Width;

			/// The height of the field along the z-axis. [Limit: >= 0] [Units: vx]
			[CategoryAttribute("Field")]
			property int Height;

			/// The width/height size of tile's on the xz-plane. [Limit: >= 0] [Units: vx]
			[CategoryAttribute("Field")]
			property int TileSize;

			/// The size of the non-navigable border around the heightfield. [Limit: >=0] [Units: vx]
			[CategoryAttribute("Field")]
			property int BorderSize;

			/// The xz-plane cell size to use for fields. [Limit: > 0] [Units: wu] 
			[CategoryAttribute("Field")]
			property float CellSize;

			/// The y-axis cell size to use for fields. [Limit: > 0] [Units: wu]
			[CategoryAttribute("Field")]
			property float CellHeight;

			/// The minimum bounds of the field's AABB. [(x, y, z)] [Units: wu]
			property Vector3 BMin;

			/// The maximum bounds of the field's AABB. [(x, y, z)] [Units: wu]
			property Vector3 BMax;

			/// The maximum slope that is considered walkable. [Limits: 0 <= value < 90] [Units: Degrees] 
			property float WalkableSlopeAngle;

			/// Minimum floor to 'ceiling' height that will still allow the floor area to 
			/// be considered walkable. [Limit: >= 3] [Units: vx] 
			property int WalkableHeight;

			/// Maximum ledge height that is considered to still be traversable. [Limit: >=0] [Units: vx] 
			property int WalkableClimb;

			/// The distance to erode/shrink the walkable area of the heightfield away from 
			/// obstructions.  [Limit: >=0] [Units: vx] 
			property int WalkableRadius;

			/// The maximum allowed length for contour edges along the border of the mesh. [Limit: >=0] [Units: vx] 
			property int MaxEdgeLen;

			/// The maximum distance a simplfied contour's border edges should deviate 
			/// the original raw contour. [Limit: >=0] [Units: vx]
			property float MaxSimplificationError;

			/// The minimum number of cells allowed to form isolated island areas. [Limit: >=0] [Units: vx] 
			property int MinRegionArea;

			/// Any regions with a span count smaller than this value will, if possible, 
			/// be merged with larger regions. [Limit: >=0] [Units: vx] 
			property int MergeRegionArea;

			/// The maximum number of vertices allowed for polygons generated during the 
			/// contour to polygon conversion process. [Limit: >= 3] 
			property int MaxVertsPerPoly;

			/// Sets the sampling distance to use when generating the detail mesh.
			/// (For height detail only.) [Limits: 0 or >= 0.9] [Units: wu] 
			property float DetailSampleDist;

			/// The maximum distance the detail mesh surface should deviate from heightfield
			/// data. (For height detail only.) [Limit: >=0] [Units: wu] 
			property float DetailSampleMaxError;
		};
	}
}