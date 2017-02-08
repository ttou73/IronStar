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
				

			/// Finds the straight path from the start to the end position within the polygon corridor.
			///  @param[in]		startPos			Path start position. [(x, y, z)]
			///  @param[in]		endPos				Path end position. [(x, y, z)]
			///  @param[in]		path				An array of polygon references that represent the path corridor.
			///  @param[in]		pathSize			The number of polygons in the @p path array.
			///  @param[out]	straightPath		Points describing the straight path. [(x, y, z) * @p straightPathCount].
			///  @param[out]	straightPathFlags	Flags describing each point. (See: #dtStraightPathFlags) [opt]
			///  @param[out]	straightPathRefs	The reference id of the polygon that is being entered at each point. [opt]
			///  @param[out]	straightPathCount	The number of points in the straight path.
			///  @param[in]		maxStraightPath		The maximum number of points the straight path arrays can hold.  [Limit: > 0]
			///  @param[in]		options				Query options. (see: #dtStraightPathOptions)
			/// @returns The status flags for the query.
			OperationStatus FindStraightPath(Vector3 startPos, Vector3 endPos, array<PolyReference>^ path, int pathSize,
				[Out] array<Vector3>^% straightPath, [Out] array<StraightPathFlags>^% straightPathFlags,
				[Out] array<PolyReference>^% straightPathRefs, int maxStraightPath, StraightPathOptions options);


			///@}
			/// @name Sliced Pathfinding Functions
			/// Common use case:
			///	-# Call initSlicedFindPath() to initialize the sliced path query.
			///	-# Call updateSlicedFindPath() until it returns complete.
			///	-# Call finalizeSlicedFindPath() to get the path.
			///@{ 

			/// Intializes a sliced path query.
			///  @param[in]		startRef	The refrence id of the start polygon.
			///  @param[in]		endRef		The reference id of the end polygon.
			///  @param[in]		startPos	A position within the start polygon. [(x, y, z)]
			///  @param[in]		endPos		A position within the end polygon. [(x, y, z)]
			///  @param[in]		filter		The polygon filter to apply to the query.
			///  @param[in]		options		query options (see: #dtFindPathOptions)
			/// @returns The status flags for the query.
			OperationStatus InitializeSlicedFindPath(PolyReference startRef, PolyReference endRef,
				Vector3 startPos, Vector3 endPos, QueryFilter^ filter, FindPathOptions options);
				

			/// Updates an in-progress sliced path query.
			///  @param[in]		maxIter		The maximum number of iterations to perform.
			///  @param[out]	doneIters	The actual number of iterations completed. [opt]
			/// @returns The status flags for the query.
			OperationStatus UpdateSlicedFindPath(const int maxIter,[Out] int% doneIters);

			/// Finalizes and returns the results of a sliced path query.
			///  @param[out]	path		An ordered list of polygon references representing the path. (Start to end.) 
			///  							[(polyRef) * @p pathCount]
			///  @param[out]	pathCount	The number of polygons returned in the @p path array.
			///  @param[in]		maxPath		The max number of polygons the path array can hold. [Limit: >= 1]
			/// @returns The status flags for the query.
			OperationStatus FinalizeSlicedFindPath([Out] array<PolyReference>^% path, const int maxPath);

			/// Finalizes and returns the results of an incomplete sliced path query, returning the path to the furthest
			/// polygon on the existing path that was visited during the search.
			///  @param[in]		existing		An array of polygon references for the existing path.
			///  @param[in]		existingSize	The number of polygon in the @p existing array.
			///  @param[out]	path			An ordered list of polygon references representing the path. (Start to end.) 
			///  								[(polyRef) * @p pathCount]
			///  @param[out]	pathCount		The number of polygons returned in the @p path array.
			///  @param[in]		maxPath			The max number of polygons the @p path array can hold. [Limit: >= 1]
			/// @returns The status flags for the query.
			OperationStatus FinalizeSlicedFindPathPartial(array<PolyReference>^ existing, int existingSize,
				[Out] array<PolyReference>^% path, const int maxPath);

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