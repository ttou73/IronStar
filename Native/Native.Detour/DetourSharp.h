#pragma once
#ifndef DetourSharp
#define DetourSharp
#include "DetourStatus.h"



using PolyReference = unsigned int;
using TileReference = unsigned int;
using uint = unsigned int;
using ushort = unsigned short;
using uchar = unsigned char;

using namespace Fusion::Core::Mathematics;
using namespace System;
using namespace System::Runtime::InteropServices;

namespace Native {
	namespace Detour {
		public ref class DetourException : System::Exception {
		public:
			DetourException(System::String^ s) : System::Exception(s) {

			}
		};


		//TODO, DT_FAILURE overflow signed int
		public enum class OperationStatus {
			Failure = DT_FAILURE,		// Operation failed.
			Success = DT_SUCCESS, 	// Operation succeed.
			InProgress = DT_IN_PROGRESS  // Operation still in progress.
		};




		// Detail information for status.
		public enum class StatusDetails {
			WrongMagic = DT_WRONG_MAGIC,		// Input data is not recognized.
			WrongVersion = DT_WRONG_VERSION,	// Input data is in wrong version.
			OutOfMemory = DT_OUT_OF_MEMORY,	// Operation ran out of memory.
			InvalidParam = DT_INVALID_PARAM,	// An input parameter was invalid.
			BufferTooSmall = DT_BUFFER_TOO_SMALL,	// Result buffer for the query was too small to store all results.
			OutOfNodes = DT_OUT_OF_NODES,		// Query ran out of nodes during search.
			PartialResult = DT_PARTIAL_RESULT	// Query did not reach the end location, returning best guess.
		};
	}
}
#endif