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

#include "DetourNavMesh.h"

namespace Native {
	namespace Detour {
		
		

		public ref class DetourException : System::Exception {
		public:
			DetourException(System::String^ s) : System::Exception(s) {

			}
		};



		//TODO, DT_FAILURE overflow signed int

		/// <summary>
		/// Operation status for detour methods.
		/// </summary>
		public enum class OperationStatus {
			//Main information

			///<summary>Main: Operation failed.</summary>
			Failure = DT_FAILURE, 

			///<summary>Main: Operation succeed.summary>
			Success = DT_SUCCESS, 

			///<summary>Main: Operation still in progress.</summary>
			InProgress = DT_IN_PROGRESS, 

			//Additional
			///<summary>Additional: Input data is not recognized.</summary>
			WrongMagic = DT_WRONG_MAGIC,

			///<summary>Additional: Input data is in wrong version.</summary>
			WrongVersion = DT_WRONG_VERSION,

			///<summary>Additional: Operation ran out of memory.</summary>
			OutOfMemory = DT_OUT_OF_MEMORY,
			
			///<summary>Additional: An input parameter was invalid.</summary>
			InvalidParam = DT_INVALID_PARAM,	 

			///<summary>Additional: Result buffer for the query was too small to store all results.</summary>
			BufferTooSmall = DT_BUFFER_TOO_SMALL, 

			///<summary>Additional: Query ran out of nodes during search.</summary>
			OutOfNodes = DT_OUT_OF_NODES,

			///<summary>Additional: Query did not reach the end location, returning best guess.</summary>
			PartialResult = DT_PARTIAL_RESULT
		};


		/*
		/// <summary>
		/// Detail information for status.
		/// </summary>
		public enum class StatusDetails {

		};*/
		 
		/// <summary>
		/// Vertex flags returned by NavigationMeshQuery.FindStraightPath.
		/// </summary>
		public enum class  StraightPathFlags
		{
			///<summary>The vertex is the start position in the path.</summary>
			Start = DT_STRAIGHTPATH_START,

			///<summary>The vertex is the end position in the path.</summary>
			End = DT_STRAIGHTPATH_END, 

			///<summary>The vertex is the start of an off-mesh connection.</summary>
			OffmeshConnection = DT_STRAIGHTPATH_OFFMESH_CONNECTION
		};


		/// <summary>
		/// Options for NavigationMeshQuery.FindStraightPath.
		/// </summary>
		public enum class StraightPathOptions
		{
			///<summary>Add a vertex at every polygon edge crossing where area changes.</summary>
			AreaCrossings = DT_STRAIGHTPATH_AREA_CROSSINGS,	///< Add a vertex at every polygon edge crossing where area changes.
			///<summary>Add a vertex at every polygon edge crossing.</summary>
			AllCrossings = DT_STRAIGHTPATH_ALL_CROSSINGS
		};

		/// <summary>
		/// Options for NavigationMeshQuery.InitSlicedFindPath and UpdateSlicedFindPath
		/// </summary>
		public enum class FindPathOptions
		{
			///<summary>Use raycasts during pathfind to "shortcut" (raycast still consider costs).</summary>
			AnyAngle = DT_FINDPATH_ANY_ANGLE
		};


		/// <summary>
		/// Options for NavigationMeshQuery.Raycast
		/// </summary>
		public enum class RaycastOptions
		{
			///<summary>Raycast should calculate movement cost along the ray and fill RaycastHit::cost.</summary>
			UseCosts = DT_RAYCAST_USE_COSTS
		}; 

		///<Summary>
		/// Tile flags used for various functions and fields.
		/// For an example, see dtNavMesh::addTile().
		///</Summary>
		public enum class TileFlags
		{
			///<summary> The navigation mesh owns the tile memory and is responsible for freeing it.</summary>
			FreeData = DT_TILE_FREE_DATA,
		};

		static public ref class Detour {
		public:
			static int VertsPerPolygon = DT_VERTS_PER_POLYGON;
			/// <summary>
			/// Returns true of status is success.
			/// </summary>
			static bool StatusSucceed(OperationStatus status)
			{
				return (static_cast<dtStatus>(status) & DT_SUCCESS) != 0;
			}

			/// <summary>
			/// Returns true of status is failure.
			/// </summary>
			static bool StatusFailed(OperationStatus status)
			{
				return (static_cast<dtStatus>(status) & DT_FAILURE) != 0;
			}

			/// <summary>
			/// Returns true of status is in progress.
			/// </summary>
			static bool StatusInProgress(OperationStatus status)
			{
				return (static_cast<dtStatus>(status) & DT_IN_PROGRESS) != 0;
			}

			/// <summary>
			/// Returns true if specific detail is set.
			/// </summary>
			static bool StatusDetail(OperationStatus status, unsigned int detail)
			{
				return (static_cast<dtStatus>(status) & detail) != 0;
			}
		};
	}
}
#endif