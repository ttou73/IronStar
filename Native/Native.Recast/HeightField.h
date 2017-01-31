#pragma once
#ifndef RecastHeightField
#define RecastHeightField
#include "Recast.h"
#include "RecastSharp.h"

namespace Native {
	namespace Recast {
		public ref class HeightField
		{
		public:
			static HeightField^ AllocateHeightField() {
				auto temp = rcAllocHeightfield();
				if (temp == nullptr) {
					//TODO
					throw gcnew System::OutOfMemoryException();
				}
				return gcnew HeightField(temp);
			}

			void Free() {
				rcFreeHeightField(nativeHeightField);
				nativeHeightField = nullptr;
			}

			void Create(BuildContext^ context, Configuration^ configuration) {
				auto t = rcCreateHeightfield(context->nativeContext, *nativeHeightField, configuration->Width, configuration->Height, configuration->nativeConfig->bmin, configuration->nativeConfig->bmax, configuration->CS, configuration->CH);
				if (!t) {
					throw gcnew HeightFieldCreateException();
				}
			}

			void RasterizeTriangles(BuildContext^ context, Configuration^ configuration, RecastMesh^ mesh, array<uchar>^ triangleAreas) {
				//TODO :: keep array in RecastMesh in native array(not in c++/cli Vector3)
				float* vertices = new float[mesh->Vertices->Length * 3];
				for (int i = 0; i < mesh->Vertices->Length; i++) {
					vertices[i * 3 + 0] = mesh->Vertices[i]->X;
					vertices[i * 3 + 1] = mesh->Vertices[i]->Y;
					vertices[i * 3 + 2] = mesh->Vertices[i]->Z;
				}
				pin_ptr<uchar> ptr = &triangleAreas[0];
				//TODO: check ptr
				auto t = rcRasterizeTriangles(context->nativeContext, vertices, (uchar*)ptr, mesh->Indices->Length / 3, *nativeHeightField, configuration->WalkableClimb);
				if (!t) {
					throw gcnew HeightFieldCreateException();
				}
				delete[] vertices;
			}

			void FilterLowHangingWalkableObstacles(BuildContext^ buildContext, Configuration^ configuration) {
				rcFilterLowHangingWalkableObstacles(buildContext->nativeContext, configuration->WalkableClimb, *nativeHeightField);
			}

			void FilterLedgeSpans(BuildContext^ buildContext, Configuration^ configuration) {
				rcFilterLedgeSpans(buildContext->nativeContext,configuration->WalkableHeight, configuration->WalkableClimb, *nativeHeightField);
			}	

			void FilterWalkableLowHeightSpans(BuildContext^ buildContext, Configuration^ configuration) {
				rcFilterWalkableLowHeightSpans(buildContext->nativeContext, configuration->WalkableHeight, *nativeHeightField);
			}

			//TODO. Think about finalizer and destructor.
		internal:
			HeightField(rcHeightfield* native) {
				nativeHeightField = native;
			}
			rcHeightfield* nativeHeightField;
		};

	}
}


#endif // !RecastHeightField
