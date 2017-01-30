#pragma once
#ifndef RecastPolyMeshDetail
#define RecastPolyMeshDetail
#include "Recast.h"
#include "RecastBuilder.h"
#include "RecastSharp.h"
#include "ContourSet.h"

namespace Native {
	namespace Recast {
		public ref class PolyMeshDetail
		{
		public:
			static PolyMeshDetail^ AllocatePolyMeshDetail() {
				auto temp = rcAllocPolyMeshDetail();
				if (temp == nullptr) {
					//TODO
					throw gcnew System::OutOfMemoryException();
				}
				return gcnew PolyMeshDetail(temp);
			}

			void Free() {
				rcFreePolyMeshDetail(nativeMeshDetail);
				nativeMeshDetail = nullptr;
			}

			void Build(BuildContext^ context, Configuration^ configuration, CompactHeightField^ chf, PolyMesh^ mesh) {

				auto t = rcBuildPolyMeshDetail(context->nativeContext, *(mesh->nativeMesh), *(chf->nativeCHF), configuration->DetailSampleDist, configuration->DetailSampleMaxError, *nativeMeshDetail);
				if (!t) {
					throw gcnew BuildContourSetException();
				}
			}
		internal:
			PolyMeshDetail(rcPolyMeshDetail* mesh) {
				nativeMeshDetail = mesh;
			}
			rcPolyMeshDetail* nativeMeshDetail;
		};
	}
}
#endif