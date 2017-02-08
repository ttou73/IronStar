#pragma once
#include "Recast.h"
#include "RecastSharp.h"
#include "ContourSet.h"
#include "CompactHeightField.h"

#ifndef RecastPolyMesh
#define RecastPolyMesh


namespace Native {
	namespace Recast {

		ref class PolyMeshDetail;

		public ref class PolyMesh
		{
		public:
			PolyMesh() {
				auto temp = rcAllocPolyMesh();
				if (temp == nullptr) {
					//TODO
					throw gcnew System::OutOfMemoryException();
				}
				nativeMesh = temp;
			}

			~PolyMesh();

			!PolyMesh();

			
			void Build(BuildContext^ context, RCConfig^ configuration, CompactHeightField^ chf, ContourSet^ set) {

				auto t = rcBuildPolyMesh(context->nativeContext, *(set->nativeSet), configuration->MaxVertsPerPoly, *nativeMesh);
				if (!t) {
					throw gcnew RecastException("Can't build PolyMesh");
				}
			}

			/// <summary>
			/// Copy PolyMesh
			/// </summary>
			/// <param name="context"></param>
			/// <param name="src">Source PolyMesh</param>
			/// <param name="dst">A PolyMesh which will contain copy of source</param>
			static void Copy(BuildContext^ context, PolyMesh^ src, PolyMesh^ dst) {
				bool t = rcCopyPolyMesh(context->nativeContext, *src->nativeMesh, *dst->nativeMesh);
				if (!t) {
					throw gcnew RecastException("Can't copy polymesh");
				}
			}

			/// <summary>
			/// Copy PolyMesh
			/// </summary>
			/// <param name="context"></param>
			/// <param name="dst">A PolyMesh which will contain copy</param>
			void Copy(BuildContext^ context, PolyMesh^ dst) {
				bool t = rcCopyPolyMesh(context->nativeContext, *this->nativeMesh, *dst->nativeMesh);
				if (!t) {
					throw gcnew RecastException("Can't copy polymesh");
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

			///< The area id assigned to each polygon. [Length: #npolys]
			property array<uchar>^ Areas {
				array<uchar>^ get() {
					array<uchar>^ temp = gcnew array<uchar>(nativeMesh->npolys);
					const uchar* temp2 = nativeMesh->areas;
					for (int i = 0; i < nativeMesh->npolys; i++) {
						temp[i] = temp2[i];
					}
					return temp;
				}
			}

			property Vector3 BMin {
				Vector3 get() {
					return Vector3(nativeMesh->bmin[0], nativeMesh->bmin[1], nativeMesh->bmin[2]);
				}
			}

			property Vector3 Bmax {
				Vector3 get() {
					return Vector3(nativeMesh->bmax[0], nativeMesh->bmax[1], nativeMesh->bmax[2]);
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

			property bool HasDetails {
				bool get() {
					return details != nullptr;
				}
			}


			property PolyMeshDetail^ Details {
				PolyMeshDetail^ get() {
					return details;
				}
		internal:
			void set(PolyMeshDetail^ details) {
					this->details = details;
				}
			}

		internal: 
			rcPolyMesh* nativeMesh;

		private:
			PolyMeshDetail^ details = nullptr;
		};
	}
}
#endif