#include "stdafx.h"
#include "NavigationMeshQuery.h"
using namespace Native::Detour;



OperationStatus Native::Detour::NavigationMeshQuery::FindPath(PolyReference startRef, PolyReference endRef, Vector3 startPos, Vector3 endPos, QueryFilter ^ filter, array<PolyReference>^% path, const int maxPath)
{
	float* _sp = new float[3];
	float* _ep = new float[3];
	_sp[0] = startPos.X; _sp[1] = startPos.Y; _sp[2] = startPos.X;
	_ep[0] = endPos.X; _ep[1] = endPos.Y; _ep[2] = endPos.X;

	PolyReference* _path = new PolyReference[maxPath];
	int pathCount;
	
	auto rv = static_cast<OperationStatus>(nativeQuery->findPath(startRef, endRef, _sp, _ep, filter->nativeFilter, _path, &pathCount, maxPath));
	path = gcnew array<PolyReference>(pathCount);
	for (int i = 0; i < pathCount; i++) {
		path[i] = _path[i];
	}
	delete _sp;
	delete _ep;
	delete _path;
	return rv;
}
