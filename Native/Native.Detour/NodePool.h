#pragma once
#ifndef DtNodePool
#define DtNodePool
#include "DetourSharp.h"
#include "DetourNode.h"

namespace Native {
	namespace Detour {
		public ref struct Node {
		public:
			Node() {
				nativeNode = new dtNode();
			}


			Node% Node::operator=(Node% other)
			{
				nativeNode->cost = other.nativeNode->cost;
				nativeNode->flags = other.nativeNode->flags;
				nativeNode->id = other.nativeNode->id;
				nativeNode->pidx = other.nativeNode->pidx;
				nativeNode->state = other.nativeNode->state;
				for (int i = 0; i < 3; i++) {
					nativeNode->pos[i] = other.nativeNode->pos[i];
				}
				nativeNode->total = other.nativeNode->total;
				return *this;
			}

			Node(const Node% other) {
				nativeNode = new dtNode();
				nativeNode->cost = other.nativeNode->cost;
				nativeNode->flags = other.nativeNode->flags;
				nativeNode->id = other.nativeNode->id;
				nativeNode->pidx = other.nativeNode->pidx;
				nativeNode->state = other.nativeNode->state;
				for (int i = 0; i < 3; i++) {
					nativeNode->pos[i] = other.nativeNode->pos[i];
				}
				nativeNode->total = other.nativeNode->total;
			}

			~Node() {
				this->!Node();
			}
			!Node() {
				if (nativeNode != nullptr) {
					delete nativeNode;
					nativeNode = nullptr;
				}
			}
			///<summary>
			///Position of the node.
			///</summary>
			property Vector3 Pos {
				Vector3 get() {
					Vector3 t;
					t.X = nativeNode->pos[0];
					t.Y = nativeNode->pos[1];
					t.Z = nativeNode->pos[2];
					return t;
				}
				void set(Vector3 v) {
					nativeNode->pos[0] = v.X;
					nativeNode->pos[1] = v.Y;
					nativeNode->pos[2] = v.Z;
				}
			}

			///<summary>
			///Cost from previous node to current node.
			///</summary>
			property float Cost {
				float get() {
					return nativeNode->cost;
				}
				void set(float v) {
					nativeNode->cost = v;
				}
			}

			///<summary>
			///Cost up to the node.
			///</summary>
			property float Total {
				float get() {
					return nativeNode->total;
				}
				void set(float v) {
					nativeNode->total = v;
				}
			}

			///<summary>
			///Index to parent node.
			///</summary>
			property uint PidX {
				uint get() {
					return nativeNode->pidx;
				}
				void set(uint v) {
					nativeNode->pidx = v;
				}
			}

			///<summary>
			///Extra state information. A polyRef can have multiple nodes with different extra info. see DT_MAX_STATES_PER_NODE
			///</summary>
			property uint State {
				uint get() {
					return nativeNode->state;
				}
				void set(uint v) {
					nativeNode->state = v;
				}
			}

			///<summary>
			///Node flags. A combination of dtNodeFlags..
			///</summary>
			property uint Flags {
				uint get() {
					return nativeNode->flags;
				}
				void set(uint v) {
					nativeNode->flags = v;
				}
			}

			///<summary>
			///Polygon ref the node corresponds to.
			///</summary>
			property PolyReference Id {
				PolyReference get() {
					return nativeNode->flags;
				}
				void set(PolyReference v) {
					nativeNode->flags = v;
				}
			}

		internal:
			Node(dtNode* node) {
				nativeNode = node; 
			}

		
			dtNode* nativeNode;
		};


		public ref class NodePool
		{
		public:
			NodePool(int maxNodes, int hashSize) {
				nativePool = new dtNodePool(maxNodes, hashSize);
			}


			~NodePool() {
				this->!NodePool();
			}

			!NodePool() {
				if (nativePool != nullptr && isNativeOwner) {
					delete nativePool;
					nativePool = nullptr;
				}
			}
			void Clear() {
				nativePool->clear();
			}

			// Get a dtNode by ref and extra state information. If there is none then - allocate
			// There can be more than one node for the same polyRef but with different extra state information
			Node^ GetNode(PolyReference id, uchar state) {
				return gcnew Node(nativePool->getNode(id, state));
			}
			Node^ FindNode(dtPolyRef id, unsigned char state) {
				return gcnew Node(nativePool->findNode(id, state));
			}
			array<Node^>^ FindNodes(dtPolyRef id, const int maxNodes) {
				dtNode* _nodes;
				int length = nativePool->findNodes(id, &_nodes, maxNodes);
				auto nodes = gcnew array<Node^>(length);
				for (int i = 0; i < length; i++) {
					nodes[i] = gcnew Node(&_nodes[i]);
				}
				return nodes;
			}

			inline uint getNodeIdx(const Node^ node)
			{
				return nativePool->getNodeIdx(node->nativeNode);
			}

			inline Node^ GetNodeAtIdx(uint idx)
			{
				return gcnew Node(nativePool->getNodeAtIdx(idx));
			}

			inline int getMemUsed()
			{
				return nativePool->getMemUsed();
			}

			inline int GetMaxNodes()  { return nativePool->getMaxNodes(); }

			inline int getHashSize()  { return nativePool->getHashSize(); }
			inline ushort GetFirst(int bucket)  { return nativePool->getFirst(bucket); }
			inline ushort GetNext(int i)  { return nativePool->getNext(i); }
			inline int GetNodeCount() { return nativePool->getNodeCount(); }
		internal:
			NodePool(dtNodePool* native, bool isNativeOwner) {
				this->isNativeOwner = isNativeOwner;
				nativePool = native;
			}
			dtNodePool* nativePool;
		private:
			bool isNativeOwner = true;
		};
	}
}
#endif