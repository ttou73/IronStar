#pragma once
#ifndef RecastPolyMesh
#define RecastPolyMesh
#include "Recast.h"
#include "RecastBuilder.h"
#include "RecastSharp.h"
#include "ContourSet.h"

namespace Native {
	namespace Recast {
		public ref class PolyMesh
		{
		public:
			static PolyMesh^ AllocatePolyMesh() {
				auto temp = rcAllocPolyMesh();
				if (temp == nullptr) {
					//TODO
					throw gcnew System::OutOfMemoryException();
				}
				return gcnew PolyMesh(temp);
			}

			void Free() {
				rcFreePolyMesh(nativeMesh);
				nativeMesh = nullptr;
			}

			void Build(BuildContext^ context, RCConfig^ configuration, CompactHeightField^ chf, ContourSet^ set) {

				auto t = rcBuildPolyMesh(context->nativeContext, *(set->nativeSet), configuration->MaxVertsPerPoly, *nativeMesh);
				if (!t) {
					throw gcnew BuildPolyMeshException();
				}
			}

			///The mesh vertices. [Form: (x, y, z) * #nverts]
			property array<ushort>^ Verts {
				array<ushort>^ get() {
				    array<ushort>^ temp = gcnew array<ushort>(nativeMesh->nverts * 3);
					const ushort* temp2 = nativeMesh->verts;
					for (int i = 0; i < nativeMesh->nverts * 3; i++) {
						temp[i] = temp2[i];
					}
					return temp;
				}
			}

			///< Polygon and neighbor data. [Length: #npolys * 2 * #nvp]
			property array<ushort>^ Polys {
				array<ushort>^ get() {
					array<ushort>^ temp = gcnew array<ushort>(nativeMesh->npolys * 2 * nativeMesh->nvp);
					const ushort* temp2 = nativeMesh->polys;
					for (int i = 0; i < nativeMesh->npolys * 2 * nativeMesh->nvp; i++) {
						temp[i] = temp2[i];
					}
					return temp;
				}
			}

			property int VerticesCount {
				int get() {
					return nativeMesh->nverts;
				}
			}

			property int PolysCount {
				int get() {
					return nativeMesh->npolys;
				}
			}

		internal:
			PolyMesh(rcPolyMesh* mesh) {
				nativeMesh = mesh;
			}
			rcPolyMesh* nativeMesh;
		};
	}
}
#endif