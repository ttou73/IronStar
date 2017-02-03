#pragma once
#ifndef RecastContourSet
#define RecastContourSet
#include "Recast.h"
#include "RecastSharp.h"
#include "CompactHeightField.h"

namespace Native {
	namespace Recast {
		public ref class ContourSet
		{
		public:
			ContourSet() {
				auto temp = rcAllocContourSet();
				if (temp == nullptr) {
					//TODO
					throw gcnew System::OutOfMemoryException();
				}
				nativeSet = temp;
			}


			~ContourSet() {
				this->!ContourSet();
			}

			!ContourSet() {
				if (nativeSet != nullptr) {
					rcFreeContourSet(nativeSet);
					nativeSet = nullptr;
				}
			}

			void Build(BuildContext^ context, RCConfig^ configuration, CompactHeightField^ chf) {

				auto t = rcBuildContours(context->nativeContext, *(chf->nativeCHF), configuration->MaxSimplificationError, configuration->MaxEdgeLen, *nativeSet);
				if (!t) {
					throw gcnew RecastException("Can't build ContourSet");
				}
			}
		internal:
			ContourSet(rcContourSet* set) {
				nativeSet = set;
			}
			rcContourSet* nativeSet;
		};
	}
}
#endif