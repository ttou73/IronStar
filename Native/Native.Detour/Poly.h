#pragma once
#ifndef DtPoly
#define DtPoly
#include "DetourSharp.h"
#include "DetourNavMesh.h"

namespace Native {
	namespace Detour {
		public ref struct Poly {
		public:

			Poly() {
				nativePoly = new dtPoly();
			}

			Poly% Poly::operator=(Poly% other)
			{
				nativePoly->areaAndtype = other.nativePoly->areaAndtype;
				nativePoly->firstLink = other.nativePoly->firstLink;
				nativePoly->flags = other.nativePoly->flags;
				nativePoly->vertCount = other.nativePoly->vertCount;
				for (int i = 0; i < 6; i++) {
					nativePoly->neis[i] = other.nativePoly->neis[i];
					nativePoly->verts[i] = other.nativePoly->verts[i];
				}
				return *this;
			}

			Poly(const Poly% other) {
				nativePoly = new dtPoly();

				nativePoly->areaAndtype = other.nativePoly->areaAndtype;
				nativePoly->firstLink = other.nativePoly->firstLink;
				nativePoly->flags = other.nativePoly->flags;
				nativePoly->vertCount = other.nativePoly->vertCount;
				for (int i = 0; i < 6; i++) {
					nativePoly->neis[i] = other.nativePoly->neis[i];
					nativePoly->verts[i] = other.nativePoly->verts[i];
				}
			}

			/// The indices of the polygon's vertices.
			/// The actual vertices are located in dtMeshTile::verts.
			property array<ushort>^ Vertices {
				array<ushort>^ get() {
					auto t = gcnew array<ushort>(Detour::VertsPerPolygon);
					auto t2 = nativePoly->verts;
					for (int i = 0; i < t->Length; i++) {
						t[i] = t2[i];
					}
					return t;
				}
			}

			/// Packed data representing neighbor polygons references and flags for each edge.
			property array<ushort>^ Neighbors {
				array<ushort>^ get() {
					auto t = gcnew array<ushort>(Detour::VertsPerPolygon);
					auto t2 = nativePoly->neis;
					for (int i = 0; i < t->Length; i++) {
						t[i] = t2[i];
					}
					return t;
				}
			}

			/// The user defined polygon flags.
			property ushort Flags
			{
				ushort get() {
					return nativePoly->flags;
				}
				void set(ushort value) {
					nativePoly->flags = value;
				}
			}

			/// Index to first link in linked list. (Or #DT_NULL_LINK if there is no link.)
			property uint FirstLink
			{
				uint get() {
					return nativePoly->firstLink;
				}
				void set(uint value) {
					nativePoly->firstLink = value;
				}
			}

			/// The number of vertices in the polygon.
			property uchar VertCount
			{
				uchar get() {
					return nativePoly->vertCount;
				}
				void set(uchar value) {
					nativePoly->vertCount = value;
				}
			}

			/// Sets or gets the user defined area id. [Limit: < #DT_MAX_AREAS]
			property uchar Area
			{
				void set(uchar t)
				{
					nativePoly->setArea(t);
				}
				uchar get() {
					return nativePoly->getArea();
				}
			}

			/// Sets or gets the polygon type. (See: #dtPolyTypes.)
			property uchar Type
			{
				void set(uchar t)
				{
				nativePoly->setType(t);
				}
				uchar get()
				{
					return nativePoly->getType();
				}
			}
			

		internal:
			dtPoly* nativePoly;

			
		};
	}
}
#endif