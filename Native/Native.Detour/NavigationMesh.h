#pragma once
#ifndef DtNavigationMesh
#define DtNavigationMesh
#include "DetourNavMesh.h"	
#include "DetourSharp.h"
#include "MeshTile.h"

namespace Native {
	namespace Detour {

		public ref class NavigationMeshParams {
		public:
			~NavigationMeshParams() {
				this->!NavigationMeshParams();
			}

			!NavigationMeshParams() {
				if (nativeNavMeshParams != nullptr) {
					delete nativeNavMeshParams;
					nativeNavMeshParams = nullptr;
				}
			}

			///The world space origin of the navigation mesh's tile space. [(x, y, z)]
			property Vector3 Origin {
				void set(Vector3 origin) {
					nativeNavMeshParams->orig[0] = origin.X;
					nativeNavMeshParams->orig[1] = origin.Y;
					nativeNavMeshParams->orig[2] = origin.Z;
				}

				Vector3 get() {
					return Vector3(nativeNavMeshParams->orig[0], nativeNavMeshParams->orig[1], nativeNavMeshParams->orig[2]);
				}
			}				

			///The width of each tile. (Along the x-axis.)
			property float TileWidth {
				void set(float tileWidth) {
					nativeNavMeshParams->tileWidth = tileWidth;
				}

				float get() {
					return nativeNavMeshParams->tileWidth;
				}
			}

			///The height of each tile. (Along the x-axis.)
			property float TileHeight {
				void set(float tileHeight) {
					nativeNavMeshParams->tileHeight = tileHeight;
				}

				float get() {
					return nativeNavMeshParams->tileHeight;
				}
			}

			///The maximum number of tiles the navigation mesh can contain.
			property int MaxTiles {
				void set(int maxTiles) {
					nativeNavMeshParams->maxTiles = maxTiles;
				}

				int get() {
					return nativeNavMeshParams->maxTiles;
				}
			}

			///The maximum number of polygons each tile can contain.
			property int MaxPolys {
				void set(int maxPolys) {
					nativeNavMeshParams->maxPolys = maxPolys;
				}

				int get() {
					return nativeNavMeshParams->maxPolys;
				}
			}
		internal:
			NavigationMeshParams(const dtNavMeshParams* temp) {
				temp = nativeNavMeshParams;
			}

			dtNavMeshParams* nativeNavMeshParams;
		};

		public ref class NavigationMesh {

			//TODO : don't return OperationStatus (OS), throw exception if OS is failed
		public:
			NavigationMesh() {
				nativeNavMesh = dtAllocNavMesh();
			}

			~NavigationMesh() {
				this->!NavigationMesh();
			}

			!NavigationMesh() {
				if (nativeNavMesh == nullptr) {
					dtFreeNavMesh(nativeNavMesh);
					nativeNavMesh = nullptr;
				}
			}

			/// Initializes the navigation mesh for tiled use.
			/// @param[in]	params		Initialization parameters.
			/// @return The status flags for the operation.
			OperationStatus Initialize(const NavigationMeshParams^ params);


			/// Initializes the navigation mesh for single tile use.
			///  @param[in]	data		Data of the new tile. (See: #dtCreateNavMeshData)
			///  @param[in]	dataSize	The data size of the new tile.
			///  @param[in]	flags		The tile flags. (See: #dtTileFlags)
			///  @return The status flags for the operation.
			///  @see dtCreateNavMeshData
			OperationStatus Initialize(array<uchar>^ data, const int dataSize, const int flags);

			/// The navigation mesh initialization params.
			const NavigationMeshParams^ GetParameters();

			/// Adds a tile to the navigation mesh.
			///  @param[in]		data		Data for the new tile mesh. (See: #dtCreateNavMeshData)
			///  @param[in]		dataSize	Data size of the new tile mesh.
			///  @param[in]		flags		Tile flags. (See: #dtTileFlags)
			///  @param[in]		lastRef		The desired reference for the tile. (When reloading a tile.) [opt] [Default: 0]
			///  @param[out]	result		The tile reference. (If the tile was succesfully added.) [opt]
			/// @return The status flags for the operation.
			OperationStatus AddTile(array<uchar>^ data, int dataSize, int flags, TileReference lastRef, [Out] TileReference% result);

			/// Removes the specified tile from the navigation mesh.
			///  @param[in]		ref			The reference of the tile to remove.
			///  @param[out]	data		Data associated with deleted tile.
			///  @param[out]	dataSize	Size of the data associated with deleted tile.
			///  @return The status flags for the operation.
			OperationStatus RemoveTile(TileReference ref, IntPtr% data, [Out] int% dataSize);

			/// Calculates the tile grid location for the specified world position.
			///  @param[in]	x  The world position for the query. [(x, y, z)]
			///  @param[in]	y  The world position for the query. [(x, y, z)]
			///  @param[in]	z  The world position for the query. [(x, y, z)]
			///  @param[out]	tx		The tile's x-location. (x, y)
			///  @param[out]	ty		The tile's y-location. (x, y)
			void CalcTileLoc(Vector3 pos, [Out] int% tx, [Out] int% ty);


			/// Gets the tile reference for the tile at specified grid location.
			/// @param[in]	x		The tile's x-location. (x, y, layer)
			/// @param[in]	y		The tile's y-location. (x, y, layer)
			/// @param[in]	layer	The tile's layer. (x, y, layer)
			/// @return The tile reference of the tile, or 0 if there is none.
			TileReference GetTileRefAt(int x, int y, int layer);

			/// Gets the tile reference for the specified tile.
			/// @param[in]	tile	The tile.
			/// @return The tile reference of the tile.
			TileReference GetTileRef(const MeshTile^ tile);

			/// The maximum number of tiles supported by the navigation mesh.
			/// @return The maximum number of tiles supported by the navigation mesh.
			int GetMaxTiles();

			/// Gets the tile at the specified index.
			/// @param[in]	i		The tile index. [Limit: 0 >= index < #getMaxTiles()]
			/// @return The tile at the specified index.
			const MeshTile^ GetTile(int i);

			/// Gets the polygon reference for the tile's base polygon.
			/// @param[in]	tile		The tile.
			/// @return The polygon reference for the base polygon in the specified tile.
			PolyReference GetPolyRefBase(const MeshTile^ tile);

			/// Gets the endpoints for an off-mesh connection, ordered by "direction of travel".
			///  @param[in]		prevRef		The reference of the polygon before the connection.
			///  @param[in]		polyRef		The reference of the off-mesh connection polygon.
			///  @param[out]	startPos	The start position of the off-mesh connection. [(x, y, z)]
			///  @param[out]	endPos		The end position of the off-mesh connection. [(x, y, z)]
			/// @return The status flags for the operation.
			OperationStatus GetOffMeshConnectionPolyEndPoints(PolyReference prevRef, PolyReference polyRef, [Out]float% startPos, [Out]float% endPos);


			/// Sets the user defined flags for the specified polygon.
			///  @param[in]	ref		The polygon reference.
			///  @param[in]	flags	The new flags for the polygon.
			/// @return The status flags for the operation.
			OperationStatus SetPolyFlags(dtPolyRef ref, ushort flags);

			/// Gets the user defined flags for the specified polygon.
			///  @param[in]		ref				The polygon reference.
			///  @param[out]	resultFlags		The polygon flags.
			/// @return The status flags for the operation.
			OperationStatus GetPolyFlags(dtPolyRef ref, [Out] ushort% resultFlags);

			/// Sets the user defined area for the specified polygon.
			///  @param[in]	ref		The polygon reference.
			///  @param[in]	area	The new area id for the polygon. [Limit: < #DT_MAX_AREAS]
			/// @return The status flags for the operation.
			OperationStatus SetPolyArea(dtPolyRef ref, uchar area);

			/// Gets the user defined area for the specified polygon.
			///  @param[in]		ref			The polygon reference.
			///  @param[out]	resultArea	The area id for the polygon.
			/// @return The status flags for the operation.
			OperationStatus GetPolyArea(dtPolyRef ref, [Out] uchar% resultArea);



			//TODO : DECODE & ENCODE part.

		internal:
			dtNavMesh* nativeNavMesh;
		};
	}
}
#endif // !NavigationMesh
