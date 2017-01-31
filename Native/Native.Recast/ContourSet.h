#pragma once
#ifndef RecastContourSet
#define RecastContourSet
#include "Recast.h"
#include "RecastBuilder.h"
#include "RecastSharp.h"

namespace Native {
	namespace Recast {
		public ref class ContourSet
		{
		public:
			static ContourSet^ AllocateContourSet() {
				auto temp = rcAllocContourSet();
				if (temp == nullptr) {
					//TODO
					throw gcnew System::OutOfMemoryException();
				}
				return gcnew ContourSet(temp);
			}

			void Free() {
				rcFreeContourSet(nativeSet);
				nativeSet = nullptr;
			}

			void Build(BuildContext^ context, RCConfig^ configuration, CompactHeightField^ chf) {

				auto t = rcBuildContours(context->nativeContext, *(chf->nativeCHF), configuration->MaxSimplificationError, configuration->MaxEdgeLen, *nativeSet);
				if (!t) {
					throw gcnew BuildContourSetException();
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