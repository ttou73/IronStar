#pragma once
#ifndef RecastStructs
#define RecastStructs

namespace Native {
	namespace Recast {

		public ref struct Vector3 {

		public:
			Vector3(float x, float y, float z) {
				X = x;
				Y = y;
				Z = z;
			};
			float X;
			float Y;
			float Z;
		};

	}
}


#endif // !RecastStructs
