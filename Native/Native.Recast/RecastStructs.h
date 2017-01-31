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

			Vector3(const Vector3% other) {
				X = other.X;
				Y = other.Y;
				Z = other.Z;
			}
			
			float X;
			float Y;
			float Z;
		};

	}
}


#endif // !RecastStructs
