#pragma once
#ifndef RecastPolyMeshDetail
#define RecastPolyMeshDetail
#include "Recast.h"
#include "RecastSharp.h"
#include "ContourSet.h"
#include "PolyMesh.h"

namespace Native {
	namespace Recast {


		public ref class PolyMeshDetail
		{
		public:
			PolyMeshDetail() {
				auto temp = rcAllocPolyMeshDetail();
				if (temp == nullptr) {
					//TODO
					throw gcnew System::OutOfMemoryException("Can't create PolyMeshDetail. Not enough memory");
				}
				nativeMeshDetail = temp;
			}

			~PolyMeshDetail() {
				this->!PolyMeshDetail();
			}

			!PolyMeshDetail() {
				if (nativeMeshDetail != nullptr) {
					rcFreePolyMeshDetail(nativeMeshDetail);
					nativeMeshDetail = nullptr;
				}
			}

			void Build(BuildContext^ context, RCConfig^ configuration, CompactHeightField^ chf, PolyMesh^ mesh) {

				auto t = rcBuildPolyMeshDetail(context->nativeContext, *(mesh->nativeMesh), *(chf->nativeCHF), configuration->DetailSampleDist, configuration->DetailSampleMaxError, *nativeMeshDetail);
				if (!t) {
					throw gcnew RecastException("Can't build PolyMeshDetail");
				}
			}
		internal: 
			rcPolyMeshDetail* nativeMeshDetail;
		};
	}
}
#endif