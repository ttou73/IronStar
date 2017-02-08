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
