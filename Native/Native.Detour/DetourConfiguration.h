#pragma once
#ifndef DetourConfig
#define DetourConfig
#include "DetourNavMeshBuilder.h"
#include "DetourSharp.h"
#include <string.h>


namespace Native {
	namespace Detour {
		public ref class DetourConfiguration {
			public:
			DetourConfiguration() {
				nativeParams = new dtNavMeshCreateParams();
				memset(nativeParams, 0, sizeof(*nativeParams));
			}
			!DetourConfiguration() {
				if (nativeParams != nullptr) {
					delete nativeParams;
					nativeParams = nullptr;
				}
			}
			~DetourConfiguration() {
				this->!DetourConfiguration();
			}



			///The polygon mesh vertices. [(x, y, z) * #vertCount] [Unit: vx]
			property array<ushort>^ Vertices {
				void set(array<ushort>^ vertices) {
					ushort * newArray = new ushort[vertices->Length];
					for (int i = 0; i < vertices->Length; i++) {
						newArray[i] = vertices[i];
					}
					delete[] nativeParams->verts;
					nativeParams->verts = newArray;
				}
				array<ushort>^ get() {

					array<ushort>^ temp = gcnew array<ushort>(nativeParams->vertCount * 3);
					const ushort* temp2 = nativeParams->verts;
					for (int i = 0; i < nativeParams->vertCount * 3; i++) {
						temp[i] = temp2[i];
					}
					return temp;

					//array<ushort>^ temp = gcnew array<ushort>(nativeParams->vertCount);
					//System::Runtime::InteropServices::Marshal::Copy(IntPtr((void *)nativeParams->verts), temp, 0, nativeParams->vertCount);
					//return dynamic_cast<array<ushort>^>(temp);
				}
			}


			///The number vertices in the polygon mesh. [Limit: >= 3]
			property int VerticesCount {
				int get() {
					return nativeParams->vertCount;
				}
				void set(int value) {
					nativeParams->vertCount = value;
				}
			}


			///Number of polygons in the mesh. [Limit: >= 1]
			property int PolyCount {
				int get() {
					return nativeParams->polyCount;
				}
				void set(int value) {
					nativeParams->polyCount = value;
				}
			}

			///Number maximum number of vertices per polygon. [Limit: >= 3]
			property int NVP {
				int get() {
					return nativeParams->nvp;
				}
				void set(int value) {
					nativeParams->nvp = value;
				}
			}


			///The polygon data. [Size: #polyCount * 2 * #nvp]
			property array<ushort>^ Polys {
				void set(array<ushort>^ vertices) {
					ushort * newArray = new ushort[vertices->Length];
					for (int i = 0; i < vertices->Length; i++) {
						newArray[i] = vertices[i];
					}
					delete[] nativeParams->polys;
					nativeParams->polys = newArray;
				}
				array<ushort>^ get() {

					array<ushort>^ temp = gcnew array<ushort>(nativeParams->polyCount * 2 * nativeParams->nvp);
					const ushort* temp2 = nativeParams->polys;
					for (int i = 0; i < nativeParams->polyCount * 2 * nativeParams->nvp; i++) {
						temp[i] = temp2[i];
					}
					return temp;
				}
			}

			///The user defined flags assigned to each polygon. [Size: #polyCount]
			property array<ushort>^ PolyFlags {
				void set(array<ushort>^ vertices) {
					ushort * newArray = new ushort[vertices->Length];
					for (int i = 0; i < vertices->Length; i++) {
						newArray[i] = vertices[i];
					}
					delete[] nativeParams->polyFlags;
					nativeParams->polyFlags = newArray;
				}
				array<ushort>^ get() {

					array<ushort>^ temp = gcnew array<ushort>(nativeParams->polyCount);
					const ushort* temp2 = nativeParams->polyFlags;
					for (int i = 0; i < nativeParams->polyCount; i++) {
						temp[i] = temp2[i];
					}
					return temp;
				}
			}

			///The user defined area ids assigned to each polygon. [Size: #polyCount]
			property array<uchar>^ PolyAreas {
				void set(array<uchar>^ vertices) {
					uchar * newArray = new uchar[vertices->Length];
					for (int i = 0; i < vertices->Length; i++) {
						newArray[i] = vertices[i];
					}
					delete[] nativeParams->polyAreas;
					nativeParams->polyAreas = newArray;
				}
				array<uchar>^ get() {

					array<uchar>^ temp = gcnew array<uchar>(nativeParams->polyCount);
					const uchar* temp2 = nativeParams->polyAreas;
					for (int i = 0; i < nativeParams->polyCount; i++) {
						temp[i] = temp2[i];
					}
					return temp;
				}
			}

			///The height detail sub-mesh data. [Size: 4 * #polyCount]
			property array<uint>^ DetailMeshes {
				void set(array<uint>^ vertices) {
					uint * newArray = new uint[vertices->Length];
					for (int i = 0; i < vertices->Length; i++) {
						newArray[i] = vertices[i];
					}
					delete[] nativeParams->detailMeshes;
					nativeParams->detailMeshes = newArray;
				}
				array<uint>^ get() {

					array<uint>^ temp = gcnew array<uint>(nativeParams->polyCount * 4);
					const uint* temp2 = nativeParams->detailMeshes;
					for (int i = 0; i < nativeParams->polyCount * 4; i++) {
						temp[i] = temp2[i];
					}
					return temp;
				}
			}


			///The number of vertices in the detail mesh.
			property int DetailVertsCount {
				int get() {
					return nativeParams->detailVertsCount;
				}
				void set(int value) {
					nativeParams->detailVertsCount = value;
				}

			}


			///The detail mesh vertices. [Size: 3 * #detailVertsCount] [Unit: wu]
			property array<float>^ DetailVerts {
				void set(array<float>^ vertices) {
					float * newArray = new float[vertices->Length];
					for (int i = 0; i < vertices->Length; i++) {
						newArray[i] = vertices[i];
					}
					delete[] nativeParams->detailVerts;
					nativeParams->detailVerts = newArray;
				}
				array<float>^ get() {

					array<float>^ temp = gcnew array<float>(nativeParams->detailVertsCount * 3);
					const float* temp2 = nativeParams->detailVerts;
					for (int i = 0; i < nativeParams->detailVertsCount * 3; i++) {
						temp[i] = temp2[i];
					}
					return temp;
				}
			}

			///The number of triangles in the detail mesh.
			property int DetailTriCount {
				int get() {
					return nativeParams->detailTriCount;
				}
				void set(int value) {
					nativeParams->detailTriCount = value;
				}
			}


			///The detail mesh triangles. [Size: 4 * #detailTriCount]
			property array<uchar>^ DetailTris {
				void set(array<uchar>^ vertices) {
					uchar * newArray = new uchar[vertices->Length];
					for (int i = 0; i < vertices->Length; i++) {
						newArray[i] = vertices[i];
					}
					delete[] nativeParams->detailTris;
					nativeParams->detailTris = newArray;
				}
				array<uchar>^ get() {

					array<uchar>^ temp = gcnew array<uchar>(nativeParams->detailTriCount * 4);
					const uchar* temp2 = nativeParams->detailTris;
					for (int i = 0; i < nativeParams->detailTriCount * 4; i++) {
						temp[i] = temp2[i];
					}
					return temp;
				}
			}


			///The number of off-mesh connections. [Limit: >= 0]
			property int OffMeshConCount {
				int get() {
					return nativeParams->offMeshConCount;
				}
				void set(int value) {
					nativeParams->offMeshConCount = value;
				}
			}

			/// Off-mesh connection vertices. [(ax, ay, az, bx, by, bz) * #offMeshConCount] [Unit: wu]
			property array<float>^ OffMeshConVerts {
				void set(array<float>^ vertices) {
					float * newArray = new float[vertices->Length];
					for (int i = 0; i < vertices->Length; i++) {
						newArray[i] = vertices[i];
					}
					delete[] nativeParams->offMeshConVerts;
					nativeParams->offMeshConVerts = newArray;
				}
				array<float>^ get() {

					array<float>^ temp = gcnew array<float>(nativeParams->offMeshConCount * 6);
					const float* temp2 = nativeParams->offMeshConVerts;
					for (int i = 0; i < nativeParams->offMeshConCount * 6; i++) {
						temp[i] = temp2[i];
					}
					return temp;
				}
			}

			/// Off-mesh connection radii. [Size: #offMeshConCount] [Unit: wu]
			property array<float>^ OffMeshConRad {
				void set(array<float>^ vertices) {
					float * newArray = new float[vertices->Length];
					for (int i = 0; i < vertices->Length; i++) {
						newArray[i] = vertices[i];
					}
					delete[] nativeParams->offMeshConRad;
					nativeParams->offMeshConRad = newArray;
				}
				array<float>^ get() {

					array<float>^ temp = gcnew array<float>(nativeParams->offMeshConCount);
					const float* temp2 = nativeParams->offMeshConRad;
					for (int i = 0; i < nativeParams->offMeshConCount; i++) {
						temp[i] = temp2[i];
					}
					return temp;
				}
			}


			/// User defined flags assigned to the off-mesh connections. [Size: #offMeshConCount]
			property array<ushort>^ OffMeshConFlags {
				void set(array<ushort>^ vertices) {
					ushort * newArray = new ushort[vertices->Length];
					for (int i = 0; i < vertices->Length; i++) {
						newArray[i] = vertices[i];
					}
					delete[] nativeParams->offMeshConFlags;
					nativeParams->offMeshConFlags = newArray;
				}
				array<ushort>^ get() {

					array<ushort>^ temp = gcnew array<ushort>(nativeParams->offMeshConCount);
					const ushort* temp2 = nativeParams->offMeshConFlags;
					for (int i = 0; i < nativeParams->offMeshConCount; i++) {
						temp[i] = temp2[i];
					}
					return temp;
				}
			}

			// User defined area ids assigned to the off-mesh connections. [Size: #offMeshConCount]
			property array<uchar>^ OffMeshConAreas {
				void set(array<uchar>^ vertices) {
					uchar * newArray = new uchar[vertices->Length];
					for (int i = 0; i < vertices->Length; i++) {
						newArray[i] = vertices[i];
					}
					delete[] nativeParams->offMeshConAreas;
					nativeParams->offMeshConAreas = newArray;
				}
				array<uchar>^ get() {

					array<uchar>^ temp = gcnew array<uchar>(nativeParams->offMeshConCount);
					const uchar* temp2 = nativeParams->offMeshConAreas;
					for (int i = 0; i < nativeParams->offMeshConCount; i++) {
						temp[i] = temp2[i];
					}
					return temp;
				}
			}

			/// The permitted travel direction of the off-mesh connections. [Size: #offMeshConCount]
			///
			/// 0 = Travel only from endpoint A to endpoint B.<br/>
			/// #DT_OFFMESH_CON_BIDIR = Bidirectional travel.
			property array<uchar>^ OffMeshConDir {
				void set(array<uchar>^ vertices) {
					uchar * newArray = new uchar[vertices->Length];
					for (int i = 0; i < vertices->Length; i++) {
						newArray[i] = vertices[i];
					}
					delete[] nativeParams->offMeshConDir;
					nativeParams->offMeshConDir = newArray;
				}
				array<uchar>^ get() {

					array<uchar>^ temp = gcnew array<uchar>(nativeParams->offMeshConCount);
					const uchar* temp2 = nativeParams->offMeshConDir;
					for (int i = 0; i < nativeParams->offMeshConCount; i++) {
						temp[i] = temp2[i];
					}
					return temp;
				}
			}


			/// The user defined ids of the off-mesh connection. [Size: #offMeshConCount]
			property array<uint>^ OffMeshConUserID {
				void set(array<uint>^ vertices) {
					uint * newArray = new uint[vertices->Length];
					for (int i = 0; i < vertices->Length; i++) {
						newArray[i] = vertices[i];
					}
					delete[] nativeParams->offMeshConUserID;
					nativeParams->offMeshConUserID = newArray;
				}
				array<uint>^ get() {

					array<uint>^ temp = gcnew array<uint>(nativeParams->polyCount * 4);
					const uint* temp2 = nativeParams->offMeshConUserID;
					for (int i = 0; i < nativeParams->polyCount * 4; i++) {
						temp[i] = temp2[i];
					}
					return temp;
				}
			}

			///The user defined id of the tile.
			property uint UserID {
				uint get() {
					return nativeParams->userId;
				}
				void set(uint value) {
					nativeParams->userId = value;
				}
			}

			///The tile's x-grid location within the multi-tile destination mesh. (Along the x-axis.)
			property int TileX {
				int get() {
					return nativeParams->tileX;
				}
				void set(int value) {
					nativeParams->tileX = value;
				}
			}

			///The tile's y-grid location within the multi-tile destination mesh. (Along the x-axis.)
			property int TileY {
				int get() {
					return nativeParams->tileY;
				}
				void set(int value) {
					nativeParams->tileY = value;
				}
			}

			///The tile's layer within the layered destination mesh. [Limit: >= 0] (Along the y-axis.)
			property int TileLayer {
				int get() {
					return nativeParams->tileLayer;
				}
				void set(int value) {
					nativeParams->tileLayer = value;
				}
			}


			///The minimum bounds of the tile. [(x, y, z)] [Unit: wu]
			property array<float>^ Bmin {
				void set(array<float>^ vertices) {
					float newArray[3];
					for (int i = 0; i < 3; i++) {
						nativeParams->bmin[i] = vertices[i];
					}
				}
				array<float>^ get() {
					array<float>^ temp = gcnew array<float>(nativeParams->offMeshConCount);
					for (int i = 0; i < 3; i++) {
						temp[i] = nativeParams->bmin[i];
					}
					return temp;
				}
			}

			///The maximum bounds of the tile. [(x, y, z)] [Unit: wu]
			property array<float>^ Bmax {
				void set(array<float>^ vertices) {
					float newArray[3];
					for (int i = 0; i < 3; i++) {
						nativeParams->bmax[i] = vertices[i];
					}
				}
				array<float>^ get() {
					array<float>^ temp = gcnew array<float>(nativeParams->offMeshConCount);
					for (int i = 0; i < 3; i++) {
						temp[i] = nativeParams->bmax[i];
					}
					return temp;
				}
			}

			///The agent height. [Unit: wu]
			property float WalkableHeight {
				float get() {
					return nativeParams->walkableHeight;
				}
				void set(float value) {
					nativeParams->walkableHeight = value;
				}
			}

			///The agent radius. [Unit: wu]
			property float WalkableRadius {
				float get() {
					return nativeParams->walkableRadius;
				}
				void set(float value) {
					nativeParams->walkableRadius = value;
				}
			}

			///The agent maximum traversable ledge. (Up/Down) [Unit: wu]
			property float WalkableClimb {
				float get() {
					return nativeParams->walkableClimb;
				}
				void set(float value) {
					nativeParams->walkableClimb = value;
				}
			}

			///The xz-plane cell size of the polygon mesh. [Limit: > 0] [Unit: wu]
			property float CS {
				float get() {
					return nativeParams->cs;
				}
				void set(float value) {
					nativeParams->cs = value;
				}
			}

			///The y-axis cell height of the polygon mesh. [Limit: > 0] [Unit: wu]
			property float CH {
				float get() {
					return nativeParams->ch;
				}
				void set(float value) {
					nativeParams->ch = value;
				}
			}

			/// True if a bounding volume tree should be built for the tile.
			/// @note The BVTree is not normally needed for layered navigation meshes.
			property bool BuildBvTree {
				bool get() {
					return nativeParams->buildBvTree;
				}
				void set(bool value) {
					nativeParams->buildBvTree = value;
				}
			}

		internal:
			dtNavMeshCreateParams* nativeParams;
		};
	}
}
#endif // !DetourConfig
