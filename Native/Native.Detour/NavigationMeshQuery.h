#pragma once
#ifndef NavMeshQuery
#define NavMeshQuery
#include "DetourSharp.h"
#include "NavigationMesh.h"
#include "DetourNavMeshQuery.h"
#include "QueryFilter.h"

namespace Native {
	namespace Detour {
		public ref class NavigationMeshQuery {
		public:
			NavigationMeshQuery() {
				nativeQuery = new dtNavMeshQuery();
			}

			~NavigationMeshQuery() {
				this->!NavigationMeshQuery();
			}
			!NavigationMeshQuery() {
				if (nativeQuery != nullptr) {
					delete nativeQuery;
					nativeQuery = nullptr;
				}
			}

			void Initialize(NavigationMesh^ navMesh, int maxNodes) {
				associatedNavMesh = navMesh;
				auto t = static_cast<OperationStatus>(nativeQuery->init(navMesh->nativeNavMesh, maxNodes));
				if (t != OperationStatus::Success) {
					throw gcnew DetourException("Can't init NavigationMeshQuery");
				}

			}


			/// Finds a path from the start polygon to the end polygon.
			///  @param[in]		startRef	The refrence id of the start polygon.
			///  @param[in]		endRef		The reference id of the end polygon.
			///  @param[in]		startPos	A position within the start polygon. [(x, y, z)]
			///  @param[in]		endPos		A position within the end polygon. [(x, y, z)]
			///  @param[in]		filter		The polygon filter to apply to the query.
			///  @param[out]	path		An ordered list of polygon references representing the path. (Start to end.) 
			///  							[(polyRef) * @p pathCount]
			///  @param[out]	pathCount	The number of polygons returned in the @p path array.
			///  @param[in]		maxPath		The maximum number of polygons the @p path array can hold. [Limit: >= 1]
			OperationStatus FindPath(PolyReference startRef, PolyReference endRef,
				Vector3 startPos, Vector3 endPos, QueryFilter^ filter,[Out] array<PolyReference>^% path, const int maxPath);
				

			NavigationMesh^ GetNavigationMesh() {
				return associatedNavMesh;
			}

		internal:
			NavigationMesh^ associatedNavMesh;
			dtNavMeshQuery* nativeQuery;
		};
	}
}
#endif // !