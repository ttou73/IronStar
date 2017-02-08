#pragma once
#ifndef DtMeshTile
#define DtMeshTile

#include "DetourNavMesh.h"
#include "DetourSharp.h"
namespace Native {
	namespace Detour {
		/// Defines a navigation mesh tile.
		/// @ingroup detour	
		public ref struct MeshTile
		{
		public:
			//This class don't have destructors because navmesh owns all tiles and will free them.
			
			//TODO: get native field.

			property MeshTile^ Next {
				MeshTile^ get() {
					return gcnew MeshTile(nativeMeshTile->next);
				}
			}
		internal:
			const dtMeshTile* nativeMeshTile;

			MeshTile(dtMeshTile* other) {
				nativeMeshTile = other;
			}

			MeshTile(const dtMeshTile* other) {
				nativeMeshTile = other;
			}
		};
	}
}
#endif // !MeshTile
