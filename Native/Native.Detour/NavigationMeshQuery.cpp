#include "stdafx.h"
#include "NavigationMeshQuery.h"
using namespace Native::Detour;



OperationStatus NavigationMeshQuery::FindPath(PolyReference startRef, PolyReference endRef, Vector3 startPos, Vector3 endPos, 
											  QueryFilter ^ filter, array<PolyReference>^% path, const int maxPath)
{
	float* _sp = new float[3];
	float* _ep = new float[3];
	_sp[0] = startPos.X; _sp[1] = startPos.Y; _sp[2] = startPos.X;
	_ep[0] = endPos.X; _ep[1] = endPos.Y; _ep[2] = endPos.X;

	PolyReference* _path = new PolyReference[maxPath];
	int pathCount;
	
	auto rv = static_cast<OperationStatus>(nativeQuery->findPath(startRef, endRef, _sp, _ep, filter->nativeFilter, _path, &pathCount, maxPath));
	if (!Detour::StatusFailed(rv)) {
		path = gcnew array<PolyReference>(pathCount);
		for (int i = 0; i < pathCount; i++) {
			path[i] = _path[i];
		}
	}
	delete _sp;
	delete _ep;
	delete _path;
	return rv;
}

OperationStatus NavigationMeshQuery::FindStraightPath(Vector3 startPos, Vector3 endPos, array<PolyReference>^ path, int pathSize,
													 array<Vector3>^% straightPath, array<StraightPathFlags>^% straightPathFlags, 
													 array<PolyReference>^% straightPathRefs, int maxStraightPath, StraightPathOptions options)
{
	float* _sp = new float[3];
	float* _ep = new float[3];
	_sp[0] = startPos.X; _sp[1] = startPos.Y; _sp[2] = startPos.X;
	_ep[0] = endPos.X; _ep[1] = endPos.Y; _ep[2] = endPos.X;
	pin_ptr<PolyReference> _path = &path[0];

	float* _straightPath = new float[maxStraightPath * 3];
	uchar* _straightPathFlags = new uchar[maxStraightPath];
	PolyReference* _straightPathRefs = new PolyReference[maxStraightPath];
	int pathCount;

	auto rv = static_cast<OperationStatus>(nativeQuery->findStraightPath(_sp, _ep, (PolyReference*)_path, pathSize, _straightPath, _straightPathFlags, _straightPathRefs, &pathCount, maxStraightPath, static_cast<int>(options)));
	
	if (!Detour::StatusFailed(rv)) {

		straightPath = gcnew array<Vector3>(pathCount);
		straightPathFlags = gcnew array<StraightPathFlags>(pathCount);
		straightPathRefs = gcnew array<PolyReference>(pathCount);
		for (int i = 0; i < pathCount; i++) {
			straightPath[i].X = _straightPath[i * 3 + 0];
			straightPath[i].Y = _straightPath[i * 3 + 1];
			straightPath[i].Z = _straightPath[i * 3 + 2];

			straightPathFlags[i] = static_cast<StraightPathFlags>(_straightPathFlags[i]);
			straightPathRefs[i] = _straightPathRefs[i];
		}
	}

	delete _sp;
	delete _ep;
	delete _straightPath;
	delete _straightPathFlags;
	delete _straightPathRefs;
	return rv;
}

OperationStatus NavigationMeshQuery::InitializeSlicedFindPath(PolyReference startRef, PolyReference endRef, Vector3 startPos,
															  Vector3 endPos, QueryFilter^ filter, FindPathOptions options)
{
	float* _sp = new float[3];
	float* _ep = new float[3];
	_sp[0] = startPos.X; _sp[1] = startPos.Y; _sp[2] = startPos.X;
	_ep[0] = endPos.X; _ep[1] = endPos.Y; _ep[2] = endPos.X;

	auto rv = static_cast<OperationStatus>(nativeQuery->initSlicedFindPath(startRef, endRef, _sp, _ep, filter->nativeFilter, static_cast<uint>(options)));

	delete _sp;
	delete _ep;
	return rv;
}

OperationStatus Native::Detour::NavigationMeshQuery::UpdateSlicedFindPath(const int maxIter, int % doneIters)
{
	int _doneIters;
	auto rv = static_cast<OperationStatus>(nativeQuery->updateSlicedFindPath(maxIter, &_doneIters));
	doneIters = _doneIters;
	return rv;
}

OperationStatus Native::Detour::NavigationMeshQuery::FinalizeSlicedFindPath(array<PolyReference>^% path, const int maxPath)
{
	PolyReference* _path = new PolyReference[maxPath];
	int pathCount;

	auto rv = static_cast<OperationStatus>(nativeQuery->finalizeSlicedFindPath(_path, &pathCount, maxPath));
	if (!Detour::StatusFailed(rv)) {
		path = gcnew array<PolyReference>(pathCount);
		for (int i = 0; i < pathCount; i++) {
			path[i] = _path[i];
		}
	}
	delete _path;
	return rv;
}

OperationStatus Native::Detour::NavigationMeshQuery::FinalizeSlicedFindPathPartial(array<PolyReference>^ existing, int existingSize, array<PolyReference>^% path, const int maxPath)
{
	PolyReference* _path = new PolyReference[maxPath];
	int pathCount;

	pin_ptr<PolyReference> _existing = &existing[0];
	auto rv = static_cast<OperationStatus>(nativeQuery->finalizeSlicedFindPathPartial(_existing, existingSize, _path, &pathCount, maxPath));
	if (!Detour::StatusFailed(rv)) {
		path = gcnew array<PolyReference>(pathCount);
		for (int i = 0; i < pathCount; i++) {
			path[i] = _path[i];
		}
	}
	delete _path;
	return rv;
}

OperationStatus NavigationMeshQuery::FindPolysAroundCircle(PolyReference startRef, Vector3 centerPos, const float radius,
	QueryFilter ^ filter, array<PolyReference>^% resultRef, array<PolyReference>^% resultParent,
	array<float>^% resultCost, const int maxResult)
{

	PolyReference* _resultRef = new PolyReference[maxResult];
	PolyReference* _resultParent = new PolyReference[maxResult];
	float* _resultCost = new float[maxResult];
	int resultCount;

	float* _cp = new float[3];
	_cp[0] = centerPos.X; _cp[1] = centerPos.Y; _cp[2] = centerPos.X;
	auto rv = static_cast<OperationStatus>(nativeQuery->findPolysAroundCircle(startRef, _cp, radius, filter->nativeFilter,
										   _resultRef, _resultParent, _resultCost, &resultCount, maxResult));

	if (!Detour::StatusFailed(rv)) {
		for (int i = 0; i < resultCount; i++) {
			resultRef[i] = _resultRef[i];
			resultParent[i] = _resultParent[i];
			resultCost[i] = _resultCost[i];
		}
	}

	delete _resultRef;
	delete _resultParent;
	delete _resultCost;
	delete _cp;
	return rv;
}

OperationStatus Native::Detour::NavigationMeshQuery::FindPolysAroundShape(PolyReference startRef, array<Vector3>^ vertices, QueryFilter ^ filter, array<PolyReference>^% resultRef, array<PolyReference>^% resultParent, array<float>^% resultCost, const int maxResult)
{
	PolyReference* _resultRef = new PolyReference[maxResult];
	PolyReference* _resultParent = new PolyReference[maxResult];
	float* _resultCost = new float[maxResult];
	int resultCount;

	float* _vertices = new float[vertices->Length * 3];
	for (int i = 0; i < vertices->Length; i++) {
		_vertices[i * 3 + 0] = vertices[i].X;
		_vertices[i * 3 + 1] = vertices[i].Y;
		_vertices[i * 3 + 2] = vertices[i].Z;
	}

	auto rv = static_cast<OperationStatus>(nativeQuery->findPolysAroundShape(startRef, _vertices, vertices->Length, filter->nativeFilter,
		                                   _resultRef, _resultParent, _resultCost, &resultCount, maxResult));

	if (!Detour::StatusFailed(rv)) {
		for (int i = 0; i < resultCount; i++) {
			resultRef[i] = _resultRef[i];
			resultParent[i] = _resultParent[i];
			resultCost[i] = _resultCost[i];
		}
	}

	delete _resultRef;
	delete _resultParent;
	delete _resultCost;
	delete _vertices;
	return rv;
}

OperationStatus Native::Detour::NavigationMeshQuery::QueryPolygons(Vector3 center, Vector3 extents, QueryFilter^ filter, 
	                                                              array<PolyReference>^% polys, const int maxPolys)
{
	PolyReference* _polys = new PolyReference[maxPolys];
	int polyCount;

	float* _cp = new float[3];
	_cp[0] = center.X; _cp[1] = center.Y; _cp[2] = center.X;

	float* _extents = new float[3];
	_extents[0] = extents.X; _extents[1] = extents.Y; _extents[2] = extents.X;

	auto rv = static_cast<OperationStatus>(nativeQuery->queryPolygons(_cp, _extents, filter->nativeFilter, _polys, &polyCount, maxPolys));

	if (!Detour::StatusFailed(rv)) {
		for (int i = 0; i < polyCount; i++) {
			polys[i] = _polys[i];
		}
	}

	delete _polys;
	delete _extents;
	delete _cp;
	return rv;
}

OperationStatus Native::Detour::NavigationMeshQuery::FindLocalNeighbourhood(PolyReference startRef, Vector3 centerPos, const float radius, QueryFilter ^ filter, array<PolyReference>^% resultRef, array<PolyReference>^% resultParent, const int maxResult)
{
	PolyReference* _resultRef = new PolyReference[maxResult];
	PolyReference* _resultParent = new PolyReference[maxResult];
	float* _resultCost = new float[maxResult];
	int resultCount;

	float* _cp = new float[3];
	_cp[0] = centerPos.X; _cp[1] = centerPos.Y; _cp[2] = centerPos.X;

	auto rv = static_cast<OperationStatus>(nativeQuery->findLocalNeighbourhood(startRef, _cp, radius, filter->nativeFilter,
		_resultRef, _resultParent, &resultCount, maxResult));

	if (!Detour::StatusFailed(rv)) {
		for (int i = 0; i < resultCount; i++) {
			resultRef[i] = _resultRef[i];
			resultParent[i] = _resultParent[i]; 
		}
	}

	delete _resultRef;
	delete _resultParent;

	delete _cp;
	return rv;
}

OperationStatus Native::Detour::NavigationMeshQuery::MoveAlongSurface(PolyReference startRef, Vector3 startPos, Vector3 endPos, QueryFilter ^ filter, Vector3% resultPos, array<PolyReference>^% visited, const int maxVisitedSize)
{
	float* _sp = new float[3];
	float* _ep = new float[3];
	_sp[0] = startPos.X; _sp[1] = startPos.Y; _sp[2] = startPos.X;
	_ep[0] = endPos.X; _ep[1] = endPos.Y; _ep[2] = endPos.X;

	float* _rp = new float[3];
	PolyReference* _visited = new PolyReference[maxVisitedSize];
	int visitedCount;

	auto rv = static_cast<OperationStatus>(nativeQuery->moveAlongSurface(startRef, _sp, _ep, filter->nativeFilter, _rp, _visited, &visitedCount, maxVisitedSize));

	if (!Detour::StatusFailed(rv)) {
		for (int i = 0; i < visitedCount; i++) {
			visited[i] = _visited[i];
		}

		resultPos.X = _rp[0];
		resultPos.Y = _rp[1];
		resultPos.Z = _rp[2];
	}

	delete visited;
	delete _rp;
	delete _sp;
	delete _ep;
	return rv;
}

OperationStatus Native::Detour::NavigationMeshQuery::Raycast(PolyReference startRef, Vector3 startPos, Vector3 endPos, QueryFilter ^ filter, float % t, Vector3 % hitNormal, array<PolyReference>^% path, const int maxPath)
{
	float* _sp = new float[3];
	float* _ep = new float[3];
	_sp[0] = startPos.X; _sp[1] = startPos.Y; _sp[2] = startPos.X;
	_ep[0] = endPos.X; _ep[1] = endPos.Y; _ep[2] = endPos.X;

	float _t;
	float* _hit = new float[3];
	PolyReference* _path = new PolyReference[maxPath];
	int pathCount;

	auto rv = static_cast<OperationStatus>(nativeQuery->raycast(startRef, _sp, _ep, filter->nativeFilter, &_t, _hit, _path, &pathCount, maxPath));

	if (!Detour::StatusFailed(rv)) {
		for (int i = 0; i < pathCount; i++) {
			path[i] = _path[i];
		}

		hitNormal.X = _hit[0];
		hitNormal.Y = _hit[1];
		hitNormal.Z = _hit[2];
	}

	delete _path;
	delete _hit;
	delete _sp;
	delete _ep;
	return rv;
}

OperationStatus Native::Detour::NavigationMeshQuery::Raycast(PolyReference startRef, Vector3 startPos, Vector3 endPos, QueryFilter ^ filter, RaycastOptions options, RaycastHit ^ hit, PolyReference prevRef)
{
	float* _sp = new float[3];
	float* _ep = new float[3];
	_sp[0] = startPos.X; _sp[1] = startPos.Y; _sp[2] = startPos.X;
	_ep[0] = endPos.X; _ep[1] = endPos.Y; _ep[2] = endPos.X;

	auto rv = static_cast<OperationStatus>(nativeQuery->raycast(startRef, _sp, _ep, filter->nativeFilter, static_cast<uint>(options), hit->nativeRaycastHit, prevRef));

	delete _sp;
	delete _ep;
	return rv;
}

OperationStatus Native::Detour::NavigationMeshQuery::FindDistanceToWall(PolyReference startRef, Vector3 centerPos, const float maxRadius, const QueryFilter ^ filter,
	                                                                    float % hitDist, Vector3 % hitPos, Vector3 % hitNormal)
{

	float* _cp = new float[3];
	_cp[0] = centerPos.X; _cp[1] = centerPos.Y; _cp[2] = centerPos.X;

	float _hitDist;
	float* _hitPos = new float[3];
	float* _hitNormal = new float[3];

	auto rv = static_cast<OperationStatus>(nativeQuery->findDistanceToWall(startRef, _cp, maxRadius, filter->nativeFilter, &_hitDist, _hitPos, _hitNormal));

	if (!Detour::StatusFailed(rv)) {
		hitDist = _hitDist;

		hitPos.X = _hitPos[0];
		hitPos.Y = _hitPos[1];
		hitPos.Z = _hitPos[2];

		hitNormal.X = _hitNormal[0];
		hitNormal.Y = _hitNormal[1];
		hitNormal.Z = _hitNormal[2];
	}

	delete _hitPos;
	delete _hitNormal;
	delete _cp;
	return rv;
}

OperationStatus NavigationMeshQuery::GetPolyWallSegments(PolyReference ref, QueryFilter ^ filter, array<Segment>^% segments, 
	                                                     array<PolyReference>^% segmentRefs, const int maxSegments)
{
	float* _segments = new float[maxSegments * 6];

	PolyReference* _segmentRefs = new PolyReference[maxSegments];
	int segmentCount;

	auto rv = static_cast<OperationStatus>(nativeQuery->getPolyWallSegments(ref, filter->nativeFilter, _segments, _segmentRefs, &segmentCount, maxSegments));

	if (!Detour::StatusFailed(rv)) {
		
		segments = gcnew array<Segment>(segmentCount);
		segmentRefs = gcnew array<PolyReference>(segmentCount);
		for (int i = 0; i < segmentCount; i++) {
			Vector3 a(_segments[i * 6 + 0], _segments[i * 6 + 1], _segments[i * 6 + 2]);
			Vector3 b(_segments[i * 6 + 3], _segments[i * 6 + 4], _segments[i * 6 + 5]);
			segments[i] = Segment(a, b);
			segmentRefs[i] = _segmentRefs[i];
		}
	}

	delete _segments;
	delete _segmentRefs;
	return rv;
}

OperationStatus Native::Detour::NavigationMeshQuery::ClosestPointOnPoly(PolyReference ref, Vector3 pos, Vector3 % closest, bool % posOverPoly)
{
	float* _pos = new float[3];
	_pos[0] = pos.X; _pos[1] = pos.Y; _pos[2] = pos.X;

	bool _posOverPoly;
	float* _closest = new float[3];

	auto rv = static_cast<OperationStatus>(nativeQuery->closestPointOnPoly(ref, _pos, _closest, &_posOverPoly));

	if (!Detour::StatusFailed(rv)) {
		closest.X = _closest[0];
		closest.Y = _closest[1];
		closest.Z = _closest[2];
		posOverPoly = _posOverPoly;
	}

	delete _pos;
	delete _closest;
	return rv;
}

OperationStatus Native::Detour::NavigationMeshQuery::ClosestPointOnPolyBoundary(PolyReference ref, Vector3 pos, Vector3 % closest)
{
	float* _pos = new float[3];
	_pos[0] = pos.X; _pos[1] = pos.Y; _pos[2] = pos.X;

	float* _closest = new float[3];

	auto rv = static_cast<OperationStatus>(nativeQuery->closestPointOnPolyBoundary(ref, _pos, _closest));

	if (!Detour::StatusFailed(rv)) {
		closest.X = _closest[0];
		closest.Y = _closest[1];
		closest.Z = _closest[2];
	}

	delete _pos;
	delete _closest;
	return rv;
}

OperationStatus Native::Detour::NavigationMeshQuery::GetPolyHeight(PolyReference ref, Vector3 pos, float % height)
{
	float* _pos = new float[3];
	_pos[0] = pos.X; _pos[1] = pos.Y; _pos[2] = pos.X;

	float _height;

	auto rv = static_cast<OperationStatus>(nativeQuery->getPolyHeight(ref, _pos, &_height));

	height = _height;

	delete _pos;
	return rv;
}

bool Native::Detour::NavigationMeshQuery::IsValidPolyRef(PolyReference ref, QueryFilter ^ filter)
{
	return nativeQuery->isValidPolyRef(ref, filter->nativeFilter);
}

bool Native::Detour::NavigationMeshQuery::IsInClosedList(PolyReference ref)
{
	return nativeQuery->isInClosedList(ref);
}


