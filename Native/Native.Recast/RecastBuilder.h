#pragma once
#ifndef RecastBuilder_H
#define RecastBuilder_H
#include "RecastStructs.h"
#include "RecastSharp.h"
#include "HeightField.h"
#include "CompactHeightField.h"
#include "ContourSet.h"
#include "PolyMesh.h"
#include "PolyMeshDetail.h"
#include "RCConfig.h"

namespace Native {
	namespace Recast {



		static public ref class RecastBuilder {
		public:
			static PolyMesh^  BuildNavigationMesh(RecastMesh^ input, RCConfig^ configuration, BuildContext^ buildContext);
			
			static Vector3 CalculateBmin(RecastMesh^ mesh) {
				auto rv = *(mesh->Vertices[0]);
				auto arr = mesh->Vertices;
				for (int i = 0; i < arr->Length; i++) {
					auto testV = arr[i];
					if (testV->X < rv.X) {
						rv.X = testV->X;
					}

					if (testV->Y < rv.Y) {
						rv.Y = testV->Y;
					}

					if (testV->Z < rv.Z) {
						rv.Z = testV->Z;
					}
				}
				return rv;
			}

			static Vector3 CalculateBmax(RecastMesh^ mesh) {
				auto rv = *(mesh->Vertices[0]);
				auto arr = mesh->Vertices;
				for (int i = 0; i < arr->Length; i++) {
					auto testV = arr[i];
					if (testV->X > rv.X) {
						rv.X = testV->X;
					}

					if (testV->Y > rv.Y) {
						rv.Y = testV->Y;
					}

					if (testV->Z > rv.Z) {
						rv.Z = testV->Z;
					}
				}
				return rv;
			}

			static void CalculateGridSize(RCConfig^ configuration) {
				auto native = configuration->nativeConfig;
				rcCalcGridSize(native->bmin, native->bmax, native->cs, &native->width, &native->height);
			}

			//TODO:: think about location of this function
			static void MarkWalkableTriangles(BuildContext^ context, RCConfig^ configuration, RecastMesh^ mesh, array<uchar>^ triangleAreas) {


				//TODO :: keep array in RecastMesh in native array(not in c++/cli Vector3)
				float* vertices = new float[mesh->Vertices->Length * 3];
				for (int i = 0; i < mesh->Vertices->Length; i++) {
					vertices[i * 3 + 0] = mesh->Vertices[i]->X;
					vertices[i * 3 + 1] = mesh->Vertices[i]->Y;
					vertices[i * 3 + 2] = mesh->Vertices[i]->Z;
				}
				pin_ptr<uchar> ptr = &triangleAreas[0];
				pin_ptr<int> triPtr = &mesh->Indices[0];
				//TODO: check ptr
				rcMarkWalkableTriangles(context->nativeContext, configuration->WalkableSlopeAngle, vertices, mesh->Vertices->Length, (int*)triPtr, mesh->Indices->Length / 3, (uchar*)ptr);
				delete[] vertices;
			}
		};

		
	}
}

#endif // !RecastBuilder
