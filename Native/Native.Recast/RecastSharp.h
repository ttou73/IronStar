#pragma once

#ifndef RECAST
#define RECAST

#include "Recast.h"
#include <string.h>
#include "RecastStructs.h"
using uint = unsigned int;
using ushort = unsigned short;
using uchar = unsigned char;


namespace Native {
	namespace Recast {

		ref struct Configuration;
		ref struct BuildContext;

		public ref class RecastMesh
		{
		public:
			array<Vector3^ >^ Vertices;
			array<int>^ Indices;
		};


		//TODO : move to another file. 
		//TODO : virtual
		public ref class BuildContext {
		public:
			BuildContext(bool state) {
				nativeContext = new rcContext(state);
			}

			~BuildContext() {
				this->!BuildContext();
			}

			!BuildContext() {
				if (nativeContext != nullptr) {
					delete nativeContext;
					nativeContext = nullptr;
				}
			}
		internal:
			rcContext* nativeContext;
		};
			
		public ref struct Configuration {
		public:
			Configuration() {
				nativeConfig = new rcConfig();
				memset(nativeConfig, 0, sizeof(*nativeConfig));
			}

			~Configuration() {
				this->!Configuration();
			}

			!Configuration() {
				if (nativeConfig != nullptr) {
					delete nativeConfig;
					nativeConfig = nullptr;
				}
			}

			///The width of the field along the x-axis. [Limit: >= 0] [Units: vx]
			property int Width{
				void set(int value) {
					nativeConfig->width = value;
				}
				int get() {
					return nativeConfig->width;
				}
			}

			/// The height of the field along the z-axis. [Limit: >= 0] [Units: vx]
			property int Height {
				void set(int value) {
					nativeConfig->height = value;
				}
				int get() {
					return nativeConfig->height;
				}
			}

			/// The width/height size of tile's on the xz-plane. [Limit: >= 0] [Units: vx]
			property int TileSize {
				void set(int value) {
					nativeConfig->tileSize = value;
				}
				int get() {
					return nativeConfig->tileSize;
				}
			}

			/// The size of the non-navigable border around the heightfield. [Limit: >=0] [Units: vx]
			property int BorderSize {
				void set(int value) {
					nativeConfig->borderSize = value;
				}
				int get() {
					return nativeConfig->borderSize;
				}
			}

			/// The xz-plane cell size to use for fields. [Limit: > 0] [Units: wu] 
			property float CS {
				void set(float value) {
					nativeConfig->cs = value;
				}
				float get() {
					return nativeConfig->cs;
				}
			}

			/// The y-axis cell size to use for fields. [Limit: > 0] [Units: wu]
			property float CH {
				void set(float value) {
					nativeConfig->ch = value;
				}
				float get() {
					return nativeConfig->ch;
				}
			}

			/// The minimum bounds of the field's AABB. [(x, y, z)] [Units: wu]
			property Vector3^ Bmin {
				void set(Vector3^ vertices) {
					nativeConfig->bmin[0] = vertices->X;
					nativeConfig->bmin[1] = vertices->Y;
					nativeConfig->bmin[2] = vertices->Z;
				}
				Vector3^ get() {
					Vector3^ rv = gcnew Vector3(nativeConfig->bmin[0], nativeConfig->bmin[1], nativeConfig->bmin[2]);
					return rv;
				}
			}


			/// The maximum bounds of the field's AABB. [(x, y, z)] [Units: wu]
			property Vector3^ Bmax {
				void set(Vector3^ vertices) {
					nativeConfig->bmax[0] = vertices->X;
					nativeConfig->bmax[1] = vertices->Y;
					nativeConfig->bmax[2] = vertices->Z;
				}
				Vector3^ get() {
					Vector3^ rv = gcnew Vector3(nativeConfig->bmax[0], nativeConfig->bmax[1], nativeConfig->bmax[2]);
					return rv;
				}
			}

			/// The maximum slope that is considered walkable. [Limits: 0 <= value < 90] [Units: Degrees] 
			property float WalkableSlopeAngle {
				void set(float value) {
					nativeConfig->walkableSlopeAngle = value;
				}
				float get() {
					return nativeConfig->walkableSlopeAngle;
				}
			}

			/// Minimum floor to 'ceiling' height that will still allow the floor area to 
			/// be considered walkable. [Limit: >= 3] [Units: vx] 
			property int WalkableHeight {
				void set(int value) {
					nativeConfig->walkableHeight = value;
				}
				int get() {
					return nativeConfig->walkableHeight;
				}
			}

			/// Maximum ledge height that is considered to still be traversable. [Limit: >=0] [Units: vx] 
			property int WalkableClimb {
				void set(int value) {
					nativeConfig->walkableClimb = value;
				}
				int get() {
					return nativeConfig->walkableClimb;
				}
			}

			/// The distance to erode/shrink the walkable area of the heightfield away from 
			/// obstructions.  [Limit: >=0] [Units: vx] 
			property int WalkableRadius {
				void set(int value) {
					nativeConfig->walkableRadius = value;
				}
				int get() {
					return nativeConfig->walkableRadius;
				}
			}

			/// The maximum allowed length for contour edges along the border of the mesh. [Limit: >=0] [Units: vx] 
			property int MaxEdgeLen {
				void set(int value) {
					nativeConfig->maxEdgeLen = value;
				}
				int get() {
					return nativeConfig->maxEdgeLen;
				}
			}

			/// The maximum distance a simplfied contour's border edges should deviate 
			/// the original raw contour. [Limit: >=0] [Units: vx]
			property float MaxSimplificationError {
				void set(float value) {
					nativeConfig->maxSimplificationError = value;
				}
				float get() {
					return nativeConfig->maxSimplificationError;
				}
			}

			/// The minimum number of cells allowed to form isolated island areas. [Limit: >=0] [Units: vx] 
			property int MinRegionArea {
				void set(int value) {
					nativeConfig->minRegionArea = value;
				}
				int get() {
					return nativeConfig->minRegionArea;
				}
			}

			/// Any regions with a span count smaller than this value will, if possible, 
			/// be merged with larger regions. [Limit: >=0] [Units: vx] 
			property int MergeRegionArea {
				void set(int value) {
					nativeConfig->mergeRegionArea = value;
				}
				int get() {
					return nativeConfig->mergeRegionArea;
				}
			}

			/// The maximum number of vertices allowed for polygons generated during the 
			/// contour to polygon conversion process. [Limit: >= 3] 
			property int MaxVertsPerPoly {
				void set(int value) {
					nativeConfig->maxVertsPerPoly = value;
				}
				int get() {
					return nativeConfig->maxVertsPerPoly;
				}
			}

			/// Sets the sampling distance to use when generating the detail mesh.
			/// (For height detail only.) [Limits: 0 or >= 0.9] [Units: wu] 
			property float DetailSampleDist {
				void set(float value) {
					nativeConfig->detailSampleDist = value;
				}
				float get() {
					return nativeConfig->detailSampleDist;
				}
			}

			/// The maximum distance the detail mesh surface should deviate from heightfield
			/// data. (For height detail only.) [Limit: >=0] [Units: wu] 
			property float DetailSampleMaxError {
				void set(float value) {
					nativeConfig->detailSampleMaxError = value;
				}
				float get() {
					return nativeConfig->detailSampleMaxError;
				}
			}
		internal:
			rcConfig* nativeConfig;
		};

		public ref struct HeightFieldCreateException : public System::Exception {

		};


		public ref struct CompactHeightFieldBuildException : public System::Exception {

		};
		public ref struct CompactHeightFieldErodeException : public System::Exception {

		};

		public ref struct CompactHeightFieldPartitionException : public System::Exception {

		};
		public ref struct BuildContourSetException : public System::Exception {

		};

		public ref struct BuildPolyMeshException : public System::Exception {

		};

		public ref struct BuildPolyMeshDetailException : public System::Exception {

		};
	}
}
#endif // !RECAST
