#pragma once
#ifndef NavMeshQuery
#define NavMeshQuery
#include "DetourSharp.h"
#include "NavigationMesh.h"
#include "DetourNavMeshQuery.h"

namespace Native {
	namespace Detour {
		public ref class NavigationMeshQuery {
			NavigationMeshQuery() {
				nativeQuery = new dtNavMeshQuery();
			}

			~NavigationMeshQuery() {
				this->!NavigationMeshQuery();
			}
			!NavigationMeshQuery() {
				if (nativeQuery != nullptr) {
					delete nativeQuery;
					nativeQuery = nullptr;
				}
			}

			void Initialize(NavigationMesh^ navMesh, int maxNodes) {
				associatedNavMesh = navMesh;
				auto t = static_cast<OperationStatus>(nativeQuery->init(navMesh->nativeNavMesh, maxNodes));
				if (t != OperationStatus::Success) {
					throw gcnew DetourException("Can't init NavigationMeshQuery");
				}

			}

			NavigationMesh^ GetNavigationMesh() {
				return associatedNavMesh;
			}

		internal:
			NavigationMesh^ associatedNavMesh;
			dtNavMeshQuery* nativeQuery;
		};
	}
}
#endif // !