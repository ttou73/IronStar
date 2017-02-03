#pragma once

#ifndef RECAST
#define RECAST

#include "RCConfig.h"
#include "Recast.h"
#include <string.h>
using uint = unsigned int;
using ushort = unsigned short;
using uchar = unsigned char;
using namespace Fusion::Core::Mathematics;


namespace Native {
	namespace Recast {
		 
		ref struct BuildContext;

		public ref class RecastMesh
		{
		public:
			array<Vector3>^ Vertices;
			array<int>^ Indices;
		};


		//TODO : move to another file. 
		//TODO : virtual
		public ref class BuildContext {
		public:
			BuildContext(bool state) {
				nativeContext = new rcContext(state);
			}

			~BuildContext() {
				this->!BuildContext();
			}

			!BuildContext() {
				if (nativeContext != nullptr) {
					delete nativeContext;
					nativeContext = nullptr;
				}
			}
		internal:
			rcContext* nativeContext;
		};
			
	 
		public ref class RecastException : public System::Exception {

		public:
			RecastException(System::String^ s) : System::Exception(s) {

			}
		};
	}
}
#endif // !RECAST
