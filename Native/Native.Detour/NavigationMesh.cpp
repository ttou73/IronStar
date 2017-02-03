#include "NavigationMesh.h"
using namespace Native::Detour;

OperationStatus NavigationMesh::Initialize(const NavigationMeshParams ^ params)
{
	return static_cast<OperationStatus>(nativeNavMesh->init(params->nativeNavMeshParams));
}

OperationStatus NavigationMesh::Initialize(array<uchar>^ data, const int dataSize, const int flags)
{
	pin_ptr<uchar> ptr = &data[0];
	return static_cast<OperationStatus>(nativeNavMesh->init(static_cast<uchar*>(ptr), dataSize, flags));
}

const NavigationMeshParams ^ NavigationMesh::GetParameters()
{
	auto temp = nativeNavMesh->getParams();
	return gcnew NavigationMeshParams(temp);
}

void NavigationMesh::CalcTileLoc(Vector3 pos, int % tx, int % ty)
{
	float* temp = new float[3];
	temp[0] = pos.X; temp[1] = pos.Y; temp[2] = pos.Z;

	int _tx;
	int _ty;
	nativeNavMesh->calcTileLoc(temp, &_tx, &_ty);
	tx = _tx;
	ty = _ty;
	delete[] temp;
}

TileReference NavigationMesh::GetTileRefAt(int x, int y, int layer)
{
	return nativeNavMesh->getTileRefAt(x, y, layer);
}

TileReference NavigationMesh::GetTileRef(const MeshTile ^ tile)
{
	return nativeNavMesh->getTileRef(tile->nativeMeshTile);
}

int NavigationMesh::GetMaxTiles()
{
	return nativeNavMesh->getMaxTiles();
}

const MeshTile^ NavigationMesh::GetTile(int i)
{
	//TODO : check here.
	const dtNavMesh* t = nativeNavMesh;
	return gcnew MeshTile(t->getTile(i));
}

PolyReference NavigationMesh::GetPolyRefBase(const MeshTile ^ tile)
{
	return nativeNavMesh->getPolyRefBase(tile->nativeMeshTile);
}

OperationStatus NavigationMesh::GetOffMeshConnectionPolyEndPoints(PolyReference prevRef, PolyReference polyRef, [Out] float % startPos, [Out] float % endPos)
{
	float _startPos;
	float _endPos;
	auto rv = static_cast<OperationStatus>(nativeNavMesh->getOffMeshConnectionPolyEndPoints(prevRef, polyRef, &_startPos, &_endPos));
	startPos = _startPos;
	endPos = _endPos;
	return rv;
}

OperationStatus NavigationMesh::SetPolyFlags(dtPolyRef ref, ushort flags)
{
	return static_cast<OperationStatus>(nativeNavMesh->setPolyFlags(ref, flags));
}

OperationStatus NavigationMesh::GetPolyFlags(dtPolyRef ref, [Out] ushort % resultFlags)
{
	ushort _resultFlags;
	auto rv = static_cast<OperationStatus>(nativeNavMesh->getPolyFlags(ref, &_resultFlags));
	resultFlags = _resultFlags;
	return rv;
}

OperationStatus NavigationMesh::SetPolyArea(dtPolyRef ref, uchar area)
{
	return static_cast<OperationStatus>(nativeNavMesh->setPolyArea(ref, area));
}

OperationStatus NavigationMesh::GetPolyArea(dtPolyRef ref, [Out] uchar% resultArea)
{
	uchar _resultArea;
	auto rv = static_cast<OperationStatus>(nativeNavMesh->getPolyArea(ref, &_resultArea));
	resultArea = _resultArea;
	return rv;
}



OperationStatus NavigationMesh::AddTile(array<uchar>^ data, int dataSize, int flags, TileReference lastRef, [Out] TileReference% result)
{
	uint outValue;

	pin_ptr<uchar> ptr = &data[0];

	auto rStatus = static_cast<OperationStatus>(nativeNavMesh->addTile(static_cast<uchar*>(ptr), dataSize, flags, lastRef, &outValue));

	result = outValue;
	return rStatus;
}


OperationStatus NavigationMesh::RemoveTile(TileReference ref, IntPtr % data, [Out] int% dataSize)
{
	uchar* _data;
	int _dataSize;
	auto rv = static_cast<OperationStatus>(nativeNavMesh->removeTile(ref, &_data, &_dataSize));
	data = IntPtr(_data);
	return rv;
}


