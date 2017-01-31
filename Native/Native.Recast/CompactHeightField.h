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

			void Build(BuildContext^ context, RCConfig^ configuration, HeightField^ heightField) {

				auto t = rcBuildCompactHeightfield(context->nativeContext, configuration->WalkableHeight, configuration->WalkableClimb, *(heightField->nativeHeightField), *nativeCHF);
				if (!t) {
					throw gcnew CompactHeightFieldBuildException();
				}
			}

			void ErodeWalkableArea(BuildContext^ context, RCConfig^ configuration) {
				bool t = rcErodeWalkableArea(context->nativeContext, configuration->WalkableRadius, *nativeCHF);

				if (!t) {
					throw gcnew CompactHeightFieldErodeException();
				}
			}

			void BuildDistanceField(BuildContext^ context, RCConfig^ configuration) {
				bool t = rcBuildDistanceField(context->nativeContext, *nativeCHF);

				if (!t) {
					throw gcnew CompactHeightFieldPartitionException();
				}
			}

			void BuildRegions(BuildContext^ context, RCConfig^ configuration) {
				bool t = rcBuildRegions(context->nativeContext, *nativeCHF, 0, configuration->MinRegionArea, configuration->MergeRegionArea);

				if (!t) {
					throw gcnew CompactHeightFieldPartitionException();
				}
			}

			void BuildRegionsMonotone(BuildContext^ context, RCConfig^ configuration) {
				bool t = rcBuildRegionsMonotone(context->nativeContext, *nativeCHF, 0, configuration->MinRegionArea, configuration->MergeRegionArea);

				if (!t) {
					throw gcnew CompactHeightFieldPartitionException();
				}
			}

			void BuildLayerRegions(BuildContext^ context, RCConfig^ configuration) {
				bool t = rcBuildLayerRegions(context->nativeContext, *nativeCHF, 0, configuration->MinRegionArea);

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