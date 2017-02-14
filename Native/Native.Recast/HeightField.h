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

			HeightField() {
				auto temp = rcAllocHeightfield();
				if (temp == nullptr) {
					throw gcnew System::OutOfMemoryException("Can't create HeightField. Not enough memory");
				}
				nativeHeightField = temp;
			}

			~HeightField() {
				this->!HeightField();
			}

			!HeightField() {
				if (nativeHeightField != nullptr) {
					rcFreeHeightField(nativeHeightField);
					nativeHeightField = nullptr;
				}
			}

			void Create(BuildContext^ context, RCConfig^ configuration) {
				auto t = rcCreateHeightfield(context->nativeContext, *nativeHeightField, configuration->Width, configuration->Height, configuration->nativeConfig->bmin, configuration->nativeConfig->bmax, configuration->CellSize, configuration->CellHeight);
				if (!t) {
					throw gcnew RecastException("Can't build HeightField");
				}
			}

			void RasterizeTriangles(BuildContext^ context, RCConfig^ configuration, RecastMesh^ mesh, array<uchar>^ triangleAreas) {
				//TODO :: keep array in RecastMesh in native array(not in c++/cli Vector3)
				float* vertices = new float[mesh->Vertices->Length * 3];
				for (int i = 0; i < mesh->Vertices->Length; i++) {
					vertices[i * 3 + 0] = mesh->Vertices[i].X;
					vertices[i * 3 + 1] = mesh->Vertices[i].Y;
					vertices[i * 3 + 2] = mesh->Vertices[i].Z;
				}
				pin_ptr<uchar> ptr = &triangleAreas[0];
				pin_ptr<int> tris = &mesh->Indices[0];
				//TODO: check ptr
				auto t = rcRasterizeTriangles(context->nativeContext, vertices, mesh->Vertices->Length, (int*)tris, (uchar*)ptr, mesh->Indices->Length / 3, *nativeHeightField, configuration->WalkableClimb); 
				if (!t) {
					throw gcnew RecastException("Can't rasterize triangles");
				}
				delete[] vertices;
			}

			void FilterLowHangingWalkableObstacles(BuildContext^ buildContext, RCConfig^ configuration) {
				rcFilterLowHangingWalkableObstacles(buildContext->nativeContext, configuration->WalkableClimb, *nativeHeightField);
			}

			void FilterLedgeSpans(BuildContext^ buildContext, RCConfig^ configuration) {
				rcFilterLedgeSpans(buildContext->nativeContext,configuration->WalkableHeight, configuration->WalkableClimb, *nativeHeightField);
			}	

			void FilterWalkableLowHeightSpans(BuildContext^ buildContext, RCConfig^ configuration) {
				rcFilterWalkableLowHeightSpans(buildContext->nativeContext, configuration->WalkableHeight, *nativeHeightField);
			}

		internal: 
			rcHeightfield* nativeHeightField;
		};

	}
}


#endif // !RecastHeightField
