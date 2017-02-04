#pragma once
#ifndef DtQueryFilter
#define DtQueryFilter

#include "DetourSharp.h"
#include "DetourNavMeshQuery.h"
#include "MeshTile.h"
#include "Poly.h"

namespace Native {
	namespace Detour {
		public ref class QueryFilter {
		public:

			
			QueryFilter() {
				nativeFilter = new dtQueryFilter();
			}

			~QueryFilter() {
				this->!QueryFilter();
			}

			!QueryFilter() {
				if (nativeFilter != nullptr) {
					delete nativeFilter;
					nativeFilter = nullptr;
				}
			}


			///Cost per area type. (Used by default implementation.)
			float GetAreaCost(int index) {
				return nativeFilter->getAreaCost(index);
			}

			///Cost per area type. (Used by default implementation.)
			void SetAreaCost(int index, float value) {
				nativeFilter->setAreaCost(index, value);
			}


			///Flags for polygons that can be visited. (Used by default implementation.)
			property uint IncludeFlags{
				uint get() {
					return nativeFilter->getIncludeFlags();
				}
				void set(uint value) {
					nativeFilter->setIncludeFlags(value);
				}
			}

			///Flags for polygons that should not be visted. (Used by default implementation.)
			property ushort ExcludeFlags {
				ushort get() {
					return nativeFilter->getExcludeFlags();
				}
				void set(ushort value) {
					nativeFilter->setExcludeFlags(value);
				}
			}


			/// Returns cost to move from the beginning to the end of a line segment
			/// that is fully contained within a polygon.
			///  @param[in]		pa			The start position on the edge of the previous and current polygon. [(x, y, z)]
			///  @param[in]		pb			The end position on the edge of the current and next polygon. [(x, y, z)]
			///  @param[in]		prevRef		The reference id of the previous polygon. [opt]
			///  @param[in]		prevTile	The tile containing the previous polygon. [opt]
			///  @param[in]		prevPoly	The previous polygon. [opt]
			///  @param[in]		curRef		The reference id of the current polygon.
			///  @param[in]		curTile		The tile containing the current polygon.
			///  @param[in]		curPoly		The current polygon.
			///  @param[in]		nextRef		The refernece id of the next polygon. [opt]
			///  @param[in]		nextTile	The tile containing the next polygon. [opt]
			///  @param[in]		nextPoly	The next polygon. [opt]

			float GetCost(Vector3 posA, Vector3 posB,
				const PolyReference prevRef, const MeshTile^ prevTile, const Poly^ prevPoly,
				const PolyReference curRef, const MeshTile^ curTile, const  Poly^ curPoly,
				const PolyReference nextRef, const MeshTile^ nextTile, const  Poly^ nextPoly)
			{
				float* _posA = new float[3];
				float* _posB = new float[3];
				_posA[0] = posA.X; _posA[1] = posA.Y; _posA[2] = posA.Z;
				_posB[0] = posB.X; _posB[1] = posB.Y; _posB[2] = posB.Z;
				
				float rv = nativeFilter->getCost(_posA, _posB,
					prevRef, prevTile->nativeMeshTile, prevPoly->nativePoly,
					curRef, curTile->nativeMeshTile, curPoly->nativePoly,
					nextRef, nextTile->nativeMeshTile, nextPoly->nativePoly);

				delete _posA;
				delete _posB;

				return rv;
			}


			/// Returns true if the polygon can be visited.  (I.e. Is traversable.)
			///  @param[in]		ref		The reference id of the polygon test.
			///  @param[in]		tile	The tile containing the polygon.
			///  @param[in]		poly  The polygon to test.
			bool PassFilter(const PolyReference ref, const MeshTile^ tile, const Poly^ poly) {
				return nativeFilter->passFilter(ref, tile->nativeMeshTile, poly->nativePoly);
			}


		internal:
			dtQueryFilter* nativeFilter;
		};
	}
}
#endif // !DtQueryFilter
