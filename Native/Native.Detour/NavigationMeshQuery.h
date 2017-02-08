#pragma once
#ifndef NavMeshQuery
#define NavMeshQuery
#include "DetourSharp.h"
#include "NavigationMesh.h"
#include "DetourNavMeshQuery.h"
#include "QueryFilter.h"
#include "NodePool.h"

namespace Native {
	namespace Detour {

		public ref struct RaycastHit
		{
		public:

			RaycastHit() {
				nativeRaycastHit = new dtRaycastHit();
			}

			RaycastHit% RaycastHit::operator=(RaycastHit% other)
			{
				nativeRaycastHit->hitEdgeIndex = other.nativeRaycastHit->hitEdgeIndex;
				nativeRaycastHit->maxPath = other.nativeRaycastHit->maxPath;
				nativeRaycastHit->pathCost = other.nativeRaycastHit->pathCost;
				nativeRaycastHit->pathCount = other.nativeRaycastHit->pathCount;
				nativeRaycastHit->t = other.nativeRaycastHit->t;
				for (int i = 0; i < 3; i++) {
					nativeRaycastHit->hitNormal[i] = other.nativeRaycastHit->hitNormal[i];
				}
				for (int i = 0; i < nativeRaycastHit->pathCount; i++) {
					nativeRaycastHit->path[i] = other.nativeRaycastHit->path[i];
				}
				return *this;
			}

			RaycastHit(const RaycastHit% other) {
				nativeRaycastHit = new dtRaycastHit();

				nativeRaycastHit->hitEdgeIndex = other.nativeRaycastHit->hitEdgeIndex;
				nativeRaycastHit->maxPath = other.nativeRaycastHit->maxPath;
				nativeRaycastHit->pathCost = other.nativeRaycastHit->pathCost;
				nativeRaycastHit->pathCount = other.nativeRaycastHit->pathCount;
				nativeRaycastHit->t = other.nativeRaycastHit->t;
				for (int i = 0; i < 3; i++) {
					nativeRaycastHit->hitNormal[i] = other.nativeRaycastHit->hitNormal[i];
				}
				for (int i = 0; i < nativeRaycastHit->pathCount; i++) {
					nativeRaycastHit->path[i] = other.nativeRaycastHit->path[i];
				}
			}

			/// The hit parameter. (FLT_MAX if no wall hit.)
			property float T {
				float get() {
					return nativeRaycastHit->t;
				}
				void set(float v) {
					nativeRaycastHit->t = v;
				}
			}

			/// hitNormal	The normal of the nearest wall hit. [(x, y, z)]
			property Vector3 HitNormal {
				Vector3 get() {
					Vector3 t;
					t.X = nativeRaycastHit->hitNormal[0];
					t.Y = nativeRaycastHit->hitNormal[1];
					t.Z = nativeRaycastHit->hitNormal[2];
					return t;
				}
				void set(Vector3 v) {
					nativeRaycastHit->hitNormal[0] = v.X;
					nativeRaycastHit->hitNormal[1] = v.Y;
					nativeRaycastHit->hitNormal[2] = v.Z;
				}
			}

			/// The index of the edge on the final polygon where the wall was hit.
			property int HitEdgeIndex {
				int get() {
					return nativeRaycastHit->hitEdgeIndex;
				}
				void set(int v) {
					nativeRaycastHit->hitEdgeIndex = v;
				}
			}

			/// Pointer to an array of reference ids of the visited polygons. [opt]
			property array<PolyReference>^ Path{
				array<PolyReference>^ get() {
					auto t = gcnew array<PolyReference>(nativeRaycastHit->pathCount);
					for (int i = 0; i < t->Length; i++) {
						t[i] = nativeRaycastHit->path[i];
					}
					return t;
				}

				void set(array<PolyReference>^ v) {
					for (int i = 0; i < v->Length; i++) {
						nativeRaycastHit->path[i] = v[i];
					}
				}
			}

			/// The maximum number of polygons the @p path array can hold.
			property int MaxPath {
				int get() {
					return nativeRaycastHit->maxPath;
				}
				void set(int v) {
					nativeRaycastHit->maxPath = v;
				}
			}

			
			///  The cost of the path until hit.
			property float PathCost {
				float get() {
					return nativeRaycastHit->pathCost;
				} 
			}

		internal:
			dtRaycastHit* nativeRaycastHit;
		};

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

			/// Finds the polygons along the navigation graph that touch the specified circle.
			///  @param[in]		startRef		The reference id of the polygon where the search starts.
			///  @param[in]		centerPos		The center of the search circle. [(x, y, z)]
			///  @param[in]		radius			The radius of the search circle.
			///  @param[in]		filter			The polygon filter to apply to the query.
			///  @param[out]	resultRef		The reference ids of the polygons touched by the circle. [opt]
			///  @param[out]	resultParent	The reference ids of the parent polygons for each result. 
			///  								Zero if a result polygon has no parent. [opt]
			///  @param[out]	resultCost		The search cost from @p centerPos to the polygon. [opt]
			///  @param[out]	resultCount		The number of polygons found. [opt]
			///  @param[in]		maxResult		The maximum number of polygons the result arrays can hold.
			/// @returns The status flags for the query.
			OperationStatus FindPolysAroundCircle(PolyReference startRef, Vector3 centerPos, const float radius, QueryFilter^ filter,
				[Out] array<PolyReference>^% resultRef, [Out] array<PolyReference>^% resultParent, [Out]array<float>^% resultCost, const int maxResult);
				

			/// Finds the polygons along the navigation graph that touch the specified convex polygon.
			///  @param[in]		startRef		The reference id of the polygon where the search starts.
			///  @param[in]		verts			The vertices describing the convex polygon. (CCW) 
			///  								[(x, y, z) * @p nverts]
			///  @param[in]		nverts			The number of vertices in the polygon.
			///  @param[in]		filter			The polygon filter to apply to the query.
			///  @param[out]	resultRef		The reference ids of the polygons touched by the search polygon. [opt]
			///  @param[out]	resultParent	The reference ids of the parent polygons for each result. Zero if a 
			///  								result polygon has no parent. [opt]
			///  @param[out]	resultCost		The search cost from the centroid point to the polygon. [opt]
			///  @param[out]	resultCount		The number of polygons found.
			///  @param[in]		maxResult		The maximum number of polygons the result arrays can hold.
			/// @returns The status flags for the query.
			OperationStatus FindPolysAroundShape(PolyReference startRef, array<Vector3>^ vertices, QueryFilter^ filter, [Out] array<PolyReference>^% resultRef,
				                          [Out] array<PolyReference>^% resultParent, [Out]array<float>^% resultCost, const int maxResult);
				
				

			/// Finds polygons that overlap the search box.
			///  @param[in]		center		The center of the search box. [(x, y, z)]
			///  @param[in]		extents		The search distance along each axis. [(x, y, z)]
			///  @param[in]		filter		The polygon filter to apply to the query.
			///  @param[out]	polys		The reference ids of the polygons that overlap the query box.
			///  @param[out]	polyCount	The number of polygons in the search result.
			///  @param[in]		maxPolys	The maximum number of polygons the search result can hold.
			/// @returns The status flags for the query.
			OperationStatus QueryPolygons(Vector3 center, Vector3 extents, QueryFilter^ filter, [Out] array<PolyReference>^% polys, const int maxPolys);
				
				

			/// Finds the non-overlapping navigation polygons in the local neighbourhood around the center position.
			///  @param[in]		startRef		The reference id of the polygon where the search starts.
			///  @param[in]		centerPos		The center of the query circle. [(x, y, z)]
			///  @param[in]		radius			The radius of the query circle.
			///  @param[in]		filter			The polygon filter to apply to the query.
			///  @param[out]	resultRef		The reference ids of the polygons touched by the circle.
			///  @param[out]	resultParent	The reference ids of the parent polygons for each result. 
			///  								Zero if a result polygon has no parent. [opt]
			///  @param[out]	resultCount		The number of polygons found.
			///  @param[in]		maxResult		The maximum number of polygons the result arrays can hold.
			/// @returns The status flags for the query.
			OperationStatus FindLocalNeighbourhood(PolyReference startRef, Vector3 centerPos, const float radius, QueryFilter^ filter,
			                           	          [Out] array<PolyReference>^% resultRef, [Out] array<PolyReference>^% resultParent, const int maxResult);
				
				
			/// Moves from the start to the end position constrained to the navigation mesh.
			///  @param[in]		startRef		The reference id of the start polygon.
			///  @param[in]		startPos		A position of the mover within the start polygon. [(x, y, x)]
			///  @param[in]		endPos			The desired end position of the mover. [(x, y, z)]
			///  @param[in]		filter			The polygon filter to apply to the query.
			///  @param[out]	resultPos		The result position of the mover. [(x, y, z)]
			///  @param[out]	visited			The reference ids of the polygons visited during the move.
			///  @param[out]	visitedCount	The number of polygons visited during the move.
			///  @param[in]		maxVisitedSize	The maximum number of polygons the @p visited array can hold.
			/// @returns The status flags for the query.
			OperationStatus MoveAlongSurface(PolyReference startRef, Vector3 startPos, Vector3 endPos, QueryFilter^ filter,
				[Out] Vector3% resultPos, [Out] array<PolyReference>^% visited, const int maxVisitedSize);

			/// Casts a 'walkability' ray along the surface of the navigation mesh from 
			/// the start position toward the end position.
			/// @note A wrapper around raycast(..., RaycastHit*). Retained for backward compatibility.
			///  @param[in]		startRef	The reference id of the start polygon.
			///  @param[in]		startPos	A position within the start polygon representing 
			///  							the start of the ray. [(x, y, z)]
			///  @param[in]		endPos		The position to cast the ray toward. [(x, y, z)]
			///  @param[out]	t			The hit parameter. (FLT_MAX if no wall hit.)
			///  @param[out]	hitNormal	The normal of the nearest wall hit. [(x, y, z)]
			///  @param[in]		filter		The polygon filter to apply to the query.
			///  @param[out]	path		The reference ids of the visited polygons. [opt]
			///  @param[out]	pathCount	The number of visited polygons. [opt]
			///  @param[in]		maxPath		The maximum number of polygons the @p path array can hold.
			/// @returns The status flags for the query.
			OperationStatus Raycast(PolyReference startRef, Vector3 startPos, Vector3 endPos, QueryFilter^ filter,
									[Out] float% t, [Out] Vector3% hitNormal, [Out] array<PolyReference>^% path, const int maxPath);
				

			/// Casts a 'walkability' ray along the surface of the navigation mesh from 
			/// the start position toward the end position.
			///  @param[in]		startRef	The reference id of the start polygon.
			///  @param[in]		startPos	A position within the start polygon representing 
			///  							the start of the ray. [(x, y, z)]
			///  @param[in]		endPos		The position to cast the ray toward. [(x, y, z)]
			///  @param[in]		filter		The polygon filter to apply to the query.
			///  @param[in]		flags		govern how the raycast behaves. See dtRaycastOptions
			///  @param[out]	hit			Pointer to a raycast hit structure which will be filled by the results.
			///  @param[in]		prevRef		parent of start ref. Used during for cost calculation [opt]
			/// @returns The status flags for the query.
			OperationStatus Raycast(PolyReference startRef, Vector3 startPos, Vector3 endPos, QueryFilter^ filter,
							        RaycastOptions options, RaycastHit^ hit, PolyReference prevRef);
				

			/// Finds the distance from the specified position to the nearest polygon wall.
			///  @param[in]		startRef		The reference id of the polygon containing @p centerPos.
			///  @param[in]		centerPos		The center of the search circle. [(x, y, z)]
			///  @param[in]		maxRadius		The radius of the search circle.
			///  @param[in]		filter			The polygon filter to apply to the query.
			///  @param[out]	hitDist			The distance to the nearest wall from @p centerPos.
			///  @param[out]	hitPos			The nearest position on the wall that was hit. [(x, y, z)]
			///  @param[out]	hitNormal		The normalized ray formed from the wall point to the 
			///  								source point. [(x, y, z)]
			/// @returns The status flags for the query.
			OperationStatus FindDistanceToWall(PolyReference startRef, Vector3 centerPos, const float maxRadius,
				const QueryFilter^ filter, [Out]float% hitDist,[Out] Vector3% hitPos, [Out] Vector3% hitNormal);
				

			/// Returns the segments for the specified polygon, optionally including portals.
			///  @param[in]		ref				The reference id of the polygon.
			///  @param[in]		filter			The polygon filter to apply to the query.
			///  @param[out]	segmentVerts	The segments. [(ax, ay, az, bx, by, bz) * segmentCount]
			///  @param[out]	segmentRefs		The reference ids of each segment's neighbor polygon. 
			///  								Or zero if the segment is a wall. [opt] [(parentRef) * @p segmentCount] 
			///  @param[out]	segmentCount	The number of segments returned.
			///  @param[in]		maxSegments		The maximum number of segments the result arrays can hold.
			/// @returns The status flags for the query.
			OperationStatus GetPolyWallSegments(PolyReference ref, QueryFilter^ filter, array<Segment>^% segments,
				                                array<PolyReference>^% segmentRefs, const int maxSegments);
				

			//TODO : add random function

			/// Finds the closest point on the specified polygon.
			///  @param[in]		ref			The reference id of the polygon.
			///  @param[in]		pos			The position to check. [(x, y, z)]
			///  @param[out]	closest		The closest point on the polygon. [(x, y, z)]
			///  @param[out]	posOverPoly	True of the position is over the polygon.
			/// @returns The status flags for the query.
			OperationStatus ClosestPointOnPoly(PolyReference ref, Vector3 pos, [Out] Vector3% closest, [Out] bool% posOverPoly);

			/// Returns a point on the boundary closest to the source point if the source point is outside the 
			/// polygon's xz-bounds.
			///  @param[in]		ref			The reference id to the polygon.
			///  @param[in]		pos			The position to check. [(x, y, z)]
			///  @param[out]	closest		The closest point. [(x, y, z)]
			/// @returns The status flags for the query.
			OperationStatus ClosestPointOnPolyBoundary(PolyReference ref, Vector3 pos, [Out] Vector3% closest);

			/// Gets the height of the polygon at the provided position using the height detail. (Most accurate.)
			///  @param[in]		ref			The reference id of the polygon.
			///  @param[in]		pos			A position within the xz-bounds of the polygon. [(x, y, z)]
			///  @param[out]	height		The height at the surface of the polygon.
			/// @returns The status flags for the query.
			OperationStatus GetPolyHeight(PolyReference ref, Vector3 pos, [Out] float% height) ;



			/// Returns true if the polygon reference is valid and passes the filter restrictions.
			///  @param[in]		ref			The polygon reference to check.
			///  @param[in]		filter		The filter to apply.
			bool IsValidPolyRef(PolyReference ref, QueryFilter^ filter);

			/// Returns true if the polygon reference is in the closed list. 
			///  @param[in]		ref		The reference id of the polygon to check.
			/// @returns True if the polygon is in closed list.
			bool IsInClosedList(PolyReference ref);
			 

			/// Gets the node pool.
			/// @returns The node pool.
			NodePool^ GetNodePool()  { return gcnew NodePool(nativeQuery->getNodePool(), false); }

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