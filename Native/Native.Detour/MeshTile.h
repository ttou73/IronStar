#pragma once
#ifndef DtMeshTile
#define DtMeshTile

#include "DetourNavMesh.h"
#include "DetourSharp.h"
#include "Poly.h"
namespace Native {
	namespace Detour {

		///<Summary>
		///Defines the location of detail sub-mesh data within a MeshTile.
		///</Summary>
		public ref struct PolyDetail
		{
		public:

			PolyDetail() {
				nativeDetail = new dtPolyDetail();
			}

			PolyDetail% PolyDetail::operator=(const PolyDetail% other)
			{
				copy(other.nativeDetail);
				return *this;
			}

			PolyDetail(const PolyDetail% other) {
				nativeDetail = new dtPolyDetail();

				copy(other.nativeDetail);
			}

			

			~PolyDetail() {
				this->!PolyDetail();
			}

			!PolyDetail() {
				if (nativeDetail != nullptr) {
					delete nativeDetail;
					nativeDetail = nullptr;
				}
			}

			///<Summary>
			///The offset of the vertices in the dtMeshTile::detailVerts array.
			///</Summary>
			property uint VertBase {
				uint get() {
					return nativeDetail->vertBase;
				}
				void set(uint v) {
					nativeDetail->vertBase = v;
				}
			}

			///<Summary>
			///The offset of the triangles in the dtMeshTile::detailTris array.
			///</Summary>
			property uint TriBase {
				uint get() {
					return nativeDetail->triBase;
				}
				void set(uint v) {
					nativeDetail->triBase = v;
				}
			}

			///<Summary>
			///The number of vertices in the sub-mesh.
			///</Summary>
			property int VertCount {
				int get() {
					return nativeDetail->vertCount;
				}
				void set(int v) {
					nativeDetail->vertCount = v;
				}
			}

			///<Summary>
			///The number of triangles in the sub-mesh.
			///</Summary>
			property int TriCount {
				int get() {
					return nativeDetail->triCount;
				}
				void set(int v) {
					nativeDetail->triCount = v;
				}
			}

		internal:
			dtPolyDetail* nativeDetail;

			PolyDetail(const dtPolyDetail* other) {
				nativeDetail = new dtPolyDetail();
				copy(other);
			}

			void copy(const dtPolyDetail* other) {
				nativeDetail->triBase = other->triBase;
				nativeDetail->triCount = other->triCount;
				nativeDetail->vertBase = other->vertBase;
				nativeDetail->vertCount = other->vertCount;
			}
		};

		///<Summary>
		///Defines a link between polygons.
		///</Summary>
		public ref struct Link
		{
		public:
			Link() {
				nativeLink = new dtLink();
			}

			Link% Link::operator=(const Link% other)
			{
				copy(other.nativeLink);
				return *this;
			}

			Link(const Link% other) {
				nativeLink = new dtLink();

				copy(other.nativeLink);
			}


			~Link() {
				this->!Link();
			}

			!Link() {
				if (nativeLink != nullptr) {
					delete nativeLink;
					nativeLink = nullptr;
				}
			}

			///<Summary>
			/// Neighbour reference. (The neighbor that is linked to.)
			///</Summary>
			property PolyReference Reference {
				PolyReference get() {
					return nativeLink->ref;
				}
				void set(PolyReference v) {
					nativeLink->ref = v;
				}
			}


			///<Summary>
			/// Index of the next link.
			///</Summary>
			property uint Next {
				uint get() {
					return nativeLink->next;
				}
				void set(uint v) {
					nativeLink->next = v;
				}
			}

			///<Summary>
			/// Index of the polygon edge that owns this link.
			///</Summary>
			property char Edge {
				char get() {
					return nativeLink->edge;
				}
				void set(char v) {
					nativeLink->edge = v;
				}
			}

			///<Summary>
			/// If a boundary link, defines on which side the link is.
			///</Summary>
			property char Side {
				char get() {
					return nativeLink->side;
				}
				void set(char v) {
					nativeLink->side = v;
				}
			}

			///<Summary>
			/// If a boundary link, defines the minimum sub-edge area.
			///</Summary>
			property char BMin {
				char get() {
					return nativeLink->bmin;
				}
				void set(char v) {
					nativeLink->bmin = v;
				}
			}

			///<Summary>
			/// If a boundary link, defines the maximum sub-edge area.
			///</Summary>
			property char BMax {
				char get() {
					return nativeLink->bmax;
				}
				void set(char v) {
					nativeLink->bmax = v;
				}
			}

		internal:


			Link(const dtLink* other) {
				nativeLink = new dtLink();

				copy(other);
			}

			void copy(const dtLink * other) {
				nativeLink->bmax = other->bmax;
				nativeLink->bmin = other->bmin;
				nativeLink->side = other->side;
				nativeLink->edge = other->edge;
				nativeLink->next = other->next;
				nativeLink->ref = other->ref;
			}
			dtLink* nativeLink;
		};

		/// Bounding volume node.
		/// @note This structure is rarely if ever used by the end user.
		/// @see dtMeshTile
		public ref struct BVNode
		{
		public:
			BVNode() {
				nativeBV = new dtBVNode();
			}

			BVNode% BVNode::operator=(const BVNode% other)
			{
				copy(other.nativeBV);
				return *this;
			}

			BVNode(const BVNode% other) {
				nativeBV = new dtBVNode();

				copy(other.nativeBV);
			}

			
			~BVNode() {
				this->!BVNode();
			}

			!BVNode() {
				if (nativeBV != nullptr) {
					delete nativeBV;
					nativeBV = nullptr;
				}
			}

			///<Summary>
			///Minimum bounds of the node's AABB. [(x, y, z)]
			///</Summary>
			property UShort3 BMin {
				void set(UShort3 vertices) {
					nativeBV->bmin[0] = vertices.X;
					nativeBV->bmin[1] = vertices.Y;
					nativeBV->bmin[2] = vertices.Z;
				}
				UShort3 get() {
					return UShort3(nativeBV->bmin[0], nativeBV->bmin[1], nativeBV->bmin[2]);
				}
			}

			///<Summary>
			///Maximum bounds of the node's AABB. [(x, y, z)]
			///</Summary>
			property UShort3 BMax {
				void set(UShort3 vertices) {
					nativeBV->bmax[0] = vertices.X;
					nativeBV->bmax[1] = vertices.Y;
					nativeBV->bmax[2] = vertices.Z;
				}
				UShort3 get() {
					return UShort3(nativeBV->bmax[0], nativeBV->bmax[1], nativeBV->bmax[2]);
				}
			}

			///<Summary>
			///The node's index. (Negative for escape sequence.)
			///</Summary>
			property int Index {
				int get() {
					return nativeBV->i;
				}
				void set(int v) {
					nativeBV->i = v;
				}
			}

		internal:


			BVNode(const dtBVNode* other) {
				nativeBV = new dtBVNode();

				copy(other);
			}

			dtBVNode* nativeBV;

			void copy(const dtBVNode* other) {
				for (int i = 0; i < 3; i++) {
					nativeBV->bmax[i] = other->bmax[i];
					nativeBV->bmin[i] = other->bmin[i];
				}
				nativeBV->i = other->i;
			}
		};

		///<Summary>
		/// Defines an navigation mesh off-mesh connection within a dtMeshTile object. 
		/// An off-mesh connection is a user defined traversable connection made up to two vertices.
		///</Summary>
		public ref struct OffMeshConnection
		{
		public:
			OffMeshConnection() {
				nativeOffMesh = new dtOffMeshConnection();
			}

			OffMeshConnection% OffMeshConnection::operator=(const OffMeshConnection% other)
			{
				copy(other.nativeOffMesh);
				return *this;
			}

			OffMeshConnection(const OffMeshConnection% other) {
				nativeOffMesh = new dtOffMeshConnection();

				copy(other.nativeOffMesh);
			}


			~OffMeshConnection() {
				this->!OffMeshConnection();
			}

			!OffMeshConnection() {
				if (nativeOffMesh != nullptr) {
					delete nativeOffMesh;
					nativeOffMesh = nullptr;
				}
			}

			///<Summary>
			///The endpoints of the connection.
			///</Summary>
			property Vector3 StartPoint {
				Vector3 get() {
					return Vector3(nativeOffMesh->pos[0], nativeOffMesh->pos[1], nativeOffMesh->pos[2]);
				}
				void set(Vector3 v) {
					nativeOffMesh->pos[0] = v.X;
					nativeOffMesh->pos[1] = v.Y;
					nativeOffMesh->pos[2] = v.Z;
				}
			}
			///<Summary>
			///The endpoints of the connection.
			///</Summary>
			property Vector3 EndPoint {
				Vector3 get() {
					return Vector3(nativeOffMesh->pos[3], nativeOffMesh->pos[4], nativeOffMesh->pos[5]);
				}
				void set(Vector3 v) {
					nativeOffMesh->pos[3] = v.X;
					nativeOffMesh->pos[4] = v.Y;
					nativeOffMesh->pos[5] = v.Z;
				}
			}


			///<Summary>
			/// The radius of the endpoints. [Limit: >= 0]
			///</Summary>
			property float Radius {
				float get() {
					return nativeOffMesh->rad;
				}
				void set(float v) {
					nativeOffMesh->rad = v;
				}
			}


			///<Summary>
			/// The polygon reference of the connection within the tile.
			///</Summary>
			property ushort Poly {
				ushort get() {
					return nativeOffMesh->poly;
				}
				void set(ushort v) {
					nativeOffMesh->poly = v;
				}
			}


			///<Summary>
			/// Link flags. 
			/// @note These are not the connection's user defined flags. Those are assigned via the 
			/// connection's dtPoly definition. These are link flags used for internal purposes.
			///</Summary>
			property uchar Flags {
				uchar get() {
					return nativeOffMesh->flags;
				}
				void set(uchar v) {
					nativeOffMesh->flags = v;
				}
			}

			///<Summary>
			/// End point side.
			///</Summary>
			property uchar Side {
				uchar get() {
					return nativeOffMesh->side;
				}
				void set(uchar v) {
					nativeOffMesh->side = v;
				}
			}

			///<Summary>
			/// End point side.
			///</Summary>
			property uint UserID {
				uint get() {
					return nativeOffMesh->userId;
				}
				void set(uint v) {
					nativeOffMesh->userId = v;
				}
			}

		internal:

			dtOffMeshConnection* nativeOffMesh;


			OffMeshConnection(const dtOffMeshConnection* other) {
				nativeOffMesh = new dtOffMeshConnection();

				copy(other);
			}


			void copy(const dtOffMeshConnection* other) {
				for (int i = 0; i < 6; i++) {
					nativeOffMesh[i] = other[i];
				}

				nativeOffMesh->side = other->side;
				nativeOffMesh->userId = other->userId;
				nativeOffMesh->flags = other->flags;
				nativeOffMesh->poly = other->poly;
				nativeOffMesh->rad = other->rad;
			}
		};

		public ref struct MeshHeader
		{

		public:
			MeshHeader() {
				nativeHeader = new dtMeshHeader();
			}

			~MeshHeader() {
				this->!MeshHeader();
			}

			!MeshHeader() {
				if (nativeHeader != nullptr) {
					delete nativeHeader;
					nativeHeader = nullptr;
				}
			}

			MeshHeader% MeshHeader::operator=(const MeshHeader% other)
			{
				copy(other.nativeHeader);
				return *this;
			}

			MeshHeader(const MeshHeader% other) {
				nativeHeader = new dtMeshHeader();

				copy(other.nativeHeader);
			}

			///<Summary>
			///Tile magic number. (Used to identify the data format.)
			///</Summary>
			property int Magic {
				int get() {
					return nativeHeader->magic;
				}
				void set(int v) {
					nativeHeader->magic = v;
				}
			}

			///<Summary>
			///Tile data format version number.
			///</Summary>
			property int Version {
				int get() {
					return nativeHeader->version;
				}
				void set(int v) {
					nativeHeader->version = v;
				}
			}

			///<Summary>
			///The x-position of the tile within the dtNavMesh tile grid. (x, y, layer)
			///</Summary>
			property int X {
				int get() {
					return nativeHeader->x;
				}
				void set(int v) {
					nativeHeader->x = v;
				}
			}

			///<Summary>
			///The y-position of the tile within the dtNavMesh tile grid. (x, y, layer)
			///</Summary>
			property int Y {
				int get() {
					return nativeHeader->y;
				}
				void set(int v) {
					nativeHeader->y = v;
				}
			}

			///<Summary>
			///The layer of the tile within the dtNavMesh tile grid. (x, y, layer)
			///</Summary>
			property int Layer {
				int get() {
					return nativeHeader->layer;
				}
				void set(int v) {
					nativeHeader->layer = v;
				}
			}

			///<Summary>
			///The user defined id of the tile.
			///</Summary>
			property uint UserId {
				uint get() {
					return nativeHeader->userId;
				}
				void set(uint v) {
					nativeHeader->userId = v;
				}
			}

			///<Summary>
			///The number of sub-meshes in the detail mesh.
			///</Summary>
			property int DetailMeshCount {
				int get() {
					return nativeHeader->detailMeshCount;
				}
				void set(int v) {
					nativeHeader->detailMeshCount = v;
				}
			}

			///<Summary>
			///The number of polygons in the tile.
			///</Summary>
			property int PolyCount {
				int get() {
					return nativeHeader->polyCount;
				}
				void set(int v) {
					nativeHeader->polyCount = v;
				}
			}

			///<Summary>
			///The number of vertices in the tile.
			///</Summary>
			property int VertCount {
				int get() {
					return nativeHeader->vertCount;
				}
				void set(int v) {
					nativeHeader->vertCount = v;
				}
			}

			///<Summary>
			///The number of allocated links.
			///</Summary>
			property int MaxLinkCount {
				int get() {
					return nativeHeader->maxLinkCount;
				}
				void set(int v) {
					nativeHeader->maxLinkCount = v;
				}
			}

			///<Summary>
			/// The number of unique vertices in the detail mesh. (In addition to the polygon vertices.)
			///</Summary>
			property int DetailVertCount {
				int get() {
					return nativeHeader->detailVertCount;
				}
				void set(int v) {
					nativeHeader->detailVertCount = v;
				}
			}

			///<Summary>
			/// The number of triangles in the detail mesh.
			///</Summary>
			property int DetailTriCount {
				int get() {
					return nativeHeader->detailTriCount;
				}
				void set(int v) {
					nativeHeader->detailTriCount = v;
				}
			}

			///<Summary>
			/// The number of bounding volume nodes. (Zero if bounding volumes are disabled.
			///</Summary>
			property int BvNodeCount {
				int get() {
					return nativeHeader->bvNodeCount;
				}
				void set(int v) {
					nativeHeader->bvNodeCount = v;
				}
			}

			///<Summary>
			/// The number of off-mesh connections.
			///</Summary>
			property int OffMeshConCount {
				int get() {
					return nativeHeader->offMeshConCount;
				}
				void set(int v) {
					nativeHeader->offMeshConCount = v;
				}
			}

			///<Summary>
			/// The index of the first polygon which is an off-mesh connection.
			///</Summary>
			property int OffMeshBase {
				int get() {
					return nativeHeader->offMeshBase;
				}
				void set(int v) {
					nativeHeader->offMeshBase = v;
				}
			}

			///<Summary>
			///The height of the agents using the tile.
			///</Summary>
			property float WalkableHeight {
				float get() {
					return nativeHeader->walkableHeight;
				}
				void set(float v) {
					nativeHeader->walkableHeight = v;
				}
			}

			///<Summary>
			///The radius of the agents using the tile.
			///</Summary>
			property float WalkableRadius {
				float get() {
					return nativeHeader->walkableRadius;
				}
				void set(float v) {
					nativeHeader->walkableRadius = v;
				}
			}

			///<Summary>
			///The maximum climb height of the agents using the tile.
			///</Summary>
			property float WalkableClimb {
				float get() {
					return nativeHeader->walkableClimb;
				}
				void set(float v) {
					nativeHeader->walkableClimb = v;
				}
			}

			///<Summary>
			///The bounding volume quantization factor. 
			///</Summary>
			property float BVQuantFactor {
				float get() {
					return nativeHeader->bvQuantFactor;
				}
				void set(float v) {
					nativeHeader->bvQuantFactor = v;
				}
			}

			///<Summary>
			///The minimum bounds of the tile's AABB. [(x, y, z)]
			///</Summary>
			property Vector3 BMin {
				void set(Vector3 vertices) {
					nativeHeader->bmin[0] = vertices.X;
					nativeHeader->bmin[1] = vertices.Y;
					nativeHeader->bmin[2] = vertices.Z;
				}
				Vector3 get() {
					return Vector3(nativeHeader->bmin[0], nativeHeader->bmin[1], nativeHeader->bmin[2]);
				}
			}

			///<Summary>
			///The maximum bounds of the tile's AABB. [(x, y, z)]
			///</Summary>
			property Vector3 BMax {
				void set(Vector3 vertices) {
					nativeHeader->bmax[0] = vertices.X;
					nativeHeader->bmax[1] = vertices.Y;
					nativeHeader->bmax[2] = vertices.Z;
				}
				Vector3 get() {
					return Vector3(nativeHeader->bmax[0], nativeHeader->bmax[1], nativeHeader->bmax[2]);
				}
			}

		internal:
			dtMeshHeader* nativeHeader;

			MeshHeader(const dtMeshHeader* other) {
				nativeHeader = new dtMeshHeader();
				copy(other);
			}

			void copy(const dtMeshHeader* other) {
				nativeHeader->bvNodeCount = other->bvNodeCount;
				nativeHeader->bvQuantFactor = other->bvQuantFactor;
				nativeHeader->detailMeshCount = other->detailMeshCount;
				nativeHeader->detailTriCount = other->detailTriCount;
				nativeHeader->detailVertCount = other->detailVertCount;
				nativeHeader->layer = other->layer;
				nativeHeader->magic = other->magic;
				nativeHeader->maxLinkCount = other->maxLinkCount;
				nativeHeader->offMeshBase = other->offMeshBase;
				nativeHeader->offMeshConCount = other->offMeshConCount;
				nativeHeader->polyCount = other->polyCount;
				nativeHeader->userId = other->userId;
				nativeHeader->version = other->version;
				nativeHeader->vertCount = other->vertCount;
				nativeHeader->walkableClimb = other->walkableClimb;
				nativeHeader->walkableHeight = other->walkableHeight;
				nativeHeader->walkableRadius = other->walkableRadius;
				nativeHeader->x = other->x;
				nativeHeader->y = other->y;

				for (int i = 0; i < 3; i++) {
					nativeHeader->bmin[i] = other->bmin[i];
					nativeHeader->bmax[i] = other->bmax[i];
				}
			}
		};



		/// Defines a navigation mesh tile.
		/// @ingroup detour	
		public ref struct MeshTile
		{
		public:
			//This class don't have destructors because navmesh owns all MeshTiles and will free them.
			
			///<Summary>
			/// Counter describing modifications to the tile.
			///</Summary>
			property uint Salt {
				uint get() {
					return nativeMeshTile->salt;
				}
			}

			///<Summary>
			/// Index to the next free link.
			///</Summary>
			property uint LinksFreeList {
				uint get() {
					return nativeMeshTile->linksFreeList;
				}
			}


			///<Summary>
			/// The tile header.
			///</Summary>
			property MeshHeader^ Header {
				MeshHeader^ get() {
					return gcnew MeshHeader(nativeMeshTile->header);
				}
			}

			///<Summary>
			/// The tile vertices. 
			///</Summary>
			property array<Vector3>^ Verts {
				array<Vector3>^ get() {
					auto t = gcnew array<Vector3>(nativeMeshTile->header->vertCount);
					for (int i = 0; i < t->Length; i++) {
						t[i] = Vector3(nativeMeshTile->verts[i * 3 + 0], nativeMeshTile->verts[i * 3 + 1], nativeMeshTile->verts[i * 3 + 2]);
					}
					return t;
				}
			}

			///<Summary>
			/// The tile polygons 
			///</Summary>
			property array<Poly^>^ Polys {
				array<Poly^>^ get() {
					auto t = gcnew array<Poly^>(nativeMeshTile->header->polyCount);
					for (int i = 0; i < t->Length; i++) {
						t[i] = gcnew Poly(&nativeMeshTile->polys[i]);
					}
					return t;
				}
			}

			///<Summary>
			/// The tile's detail sub-meshes. 
			///</Summary>
			property array<Link^>^ Links {
				array<Link^>^ get() {
					auto t = gcnew array<Link^>(nativeMeshTile->header->maxLinkCount);
					for (int i = 0; i < t->Length; i++) {
						t[i] = gcnew Link(&nativeMeshTile->links[i]);
					}
					return t;
				}
			}

			///<Summary>
			/// The tile's detail sub-meshes. 
			///</Summary>
			property array<PolyDetail^>^ DetailMeshes {
				array<PolyDetail^>^ get() {
					auto t = gcnew array<PolyDetail^>(nativeMeshTile->header->detailMeshCount);
					for (int i = 0; i < t->Length; i++) {
						t[i] = gcnew PolyDetail(&nativeMeshTile->detailMeshes[i]);
					}
					return t;
				}
			}

			///<Summary>
			///The detail mesh's unique vertices. 
			///</Summary>
			property array<Vector3>^ DetailVerts {
				array<Vector3>^ get() {
					auto t = gcnew array<Vector3>(nativeMeshTile->header->detailVertCount);
					for (int i = 0; i < t->Length; i++) {
						t[i] = Vector3(nativeMeshTile->detailVerts[i * 3 + 0], nativeMeshTile->detailVerts[i * 3 + 1], nativeMeshTile->detailVerts[i * 3 + 2]);
					}
					return t;
				}
			}

			///<Summary>
			/// The detail mesh's triangles.
			///</Summary>
			property array<Byte3>^ DetailTris {
				array<Byte3>^ get() {
					auto t = gcnew array<Byte3>(nativeMeshTile->header->detailTriCount);
					for (int i = 0; i < t->Length; i++) {
						t[i] = Byte3(nativeMeshTile->detailTris[i * 3 + 0], nativeMeshTile->detailTris[i * 3 + 1], nativeMeshTile->detailTris[i * 3 + 2]);
					}
					return t;
				}
			}


			///<Summary>
			/// The tile bounding volume nodes. [Size: dtMeshHeader::bvNodeCount]
			/// (Will be null if bounding volumes are disabled.)
			///</Summary>
			property array<BVNode^>^ BVTree {
				array<BVNode^>^ get() {
					auto t = gcnew array<BVNode^>(nativeMeshTile->header->bvNodeCount);
					for (int i = 0; i < t->Length; i++) {
						t[i] = gcnew BVNode(&nativeMeshTile->bvTree[i]);
					}
					return t;
				}
			}

			///<Summary>
			///The tile off-mesh connections
			///</Summary>
			property array<OffMeshConnection^>^ OffMeshConnections {
				array<OffMeshConnection^>^ get() {
					auto t = gcnew array<OffMeshConnection^>(nativeMeshTile->header->offMeshConCount);
					for (int i = 0; i < t->Length; i++) {
						t[i] = gcnew OffMeshConnection(&nativeMeshTile->offMeshCons[i]);
					}
					return t;
				}
			}
			
			///<Summary>
			///The tile data
			///</Summary>
			property array<uchar>^ Data {
				array<uchar>^ get() {
					auto t = gcnew array<uchar>(nativeMeshTile->dataSize);
					for (int i = 0; i < t->Length; i++) {
						t[i] = nativeMeshTile->data[i];
					}
					return t;
				}
			}


			///<Summary>
			/// Size of the tile data
			///</Summary>
			property int DataSize {
				int get() {
					return nativeMeshTile->dataSize;
				}
			}

			///<Summary>
			///Tile flags
			///</Summary>
			property TileFlags Flags {
				TileFlags get() {
					return static_cast<TileFlags>(nativeMeshTile->flags);
				}
			}

			///<Summary>
			///The next free tile, or the next tile in the spatial grid.
			///</Summary>
			property MeshTile^ Next {
				MeshTile^ get() {
					return gcnew MeshTile(nativeMeshTile->next);
				}
			}
		internal:
			const dtMeshTile* nativeMeshTile;

			MeshTile(dtMeshTile* other) {
				nativeMeshTile = other;
			}

			MeshTile(const dtMeshTile* other) {
				nativeMeshTile = other;
			}
		};
	}
}
#endif // !MeshTile
