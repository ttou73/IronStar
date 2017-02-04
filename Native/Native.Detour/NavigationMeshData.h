#pragma once
#ifndef NavigationMeshData
#include "DetourSharp.h"
#include "DetourConfiguration.h"

namespace Native {
	namespace Detour {
		public ref class NavigationMeshData {

		public:
			NavigationMeshData(DetourConfiguration^ config) {
				uchar* arr;
				int test;
				bool t = dtCreateNavMeshData(config->nativeParams, &arr, &test);
				if (!t) {
					throw gcnew DetourException("Can't create navigation mesh data");
				}
				size = test;
				data = gcnew array<uchar>(size);
				for (int i = 0; i < size; i++) {
					data[i] = arr[i];
				}

				nativePointer = arr;
			}


			~NavigationMeshData() {
				this->!NavigationMeshData();
			}

			!NavigationMeshData() {
				if (nativePointer != nullptr) {
					dtFree(nativePointer);
					nativePointer = nullptr;						
				}
			}

			property array<uchar>^ Data {
				array<uchar>^ get() {
					return data;
				}
			}


			property int Size {
				int get() {
					return size;
				}
			}

		internal:
			array<uchar>^ data;
			int size;
			uchar* nativePointer;
		};
	}
}
#endif // !NavigationMeshData
