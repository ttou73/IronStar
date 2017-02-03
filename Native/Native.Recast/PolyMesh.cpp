#include "PolyMeshDetail.h"

Native::Recast::PolyMesh::~PolyMesh() {
	this->!PolyMesh();
}

Native::Recast::PolyMesh::!PolyMesh() {
	if (nativeMesh != nullptr) {
		if (HasDetails) {
			details->~PolyMeshDetail();
		}
		rcFreePolyMesh(nativeMesh);
		nativeMesh = nullptr;
	}
}