#pragma once
#ifndef RecastCompactHeightField
#define RecastCompactHeightField
#include "HeightField.h"
namespace Native {
	namespace Recast {
		public ref class CompactHeightField
		{
		public:
			static CompactHeightField^ AllocateCompactHeightField() {
				auto temp = rcAllocCompactHeightfield();
				if (temp == nullptr) {
					//TODO
					throw gcnew System::OutOfMemoryException();
				}
				return gcnew CompactHeightField(temp);
			}

			void Free() {
				rcFreeCompactHeightfield(nativeCHF);
				nativeCHF = nullptr;
			}

			void Build(BuildContext^ context, Configuration^ configuration, HeightField^ heightField) {

				auto t = rcBuildCompactHeightfield(context->nativeContext, configuration->WalkableHeight, configuration->WalkableClimb, *(heightField->nativeHeightField), *nativeCHF);
				if (!t) {
					throw gcnew CompactHeightFieldBuildException();
				}
			}

			void ErodeWalkableArea(BuildContext^ context, Configuration^ configuration) {
				bool t = rcErodeWalkableArea(context->nativeContext, configuration->WalkableRadius, *nativeCHF);

				if (!t) {
					throw gcnew CompactHeightFieldErodeException();
				}
			}

			void BuildRegionsMonotone(BuildContext^ context, Configuration^ configuration) {
				bool t = rcBuildRegionsMonotone(context->nativeContext, *nativeCHF, 0, configuration->MinRegionArea, configuration->MergeRegionArea);

				if (!t) {
					throw gcnew CompactHeightFieldPartitionException();
				}
			}

		internal:
			CompactHeightField(rcCompactHeightfield* native) {
				nativeCHF = native;
			}

			rcCompactHeightfield* nativeCHF;
		};

	}
}

#endif