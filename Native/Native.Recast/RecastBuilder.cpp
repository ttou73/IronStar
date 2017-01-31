#include "RecastBuilder.h"
using namespace Native::Recast;

PolyMesh^ RecastBuilder::BuildNavigationMesh(RecastMesh ^ input, RCConfig^ configuration, BuildContext^ buildContext)
{
	int triangleCount = input->Indices->Length / 3;
	int verticesCount = input->Vertices->Length;

	configuration->BMin = CalculateBmin(input);
	configuration->BMax = CalculateBmax(input);

	CalculateGridSize(configuration);

	HeightField^ heightField = HeightField::AllocateHeightField();
	
	heightField->Create(buildContext, configuration);

	array<uchar>^ triangleAreas = gcnew array<uchar>(triangleCount);
	
	for (int i = 0; i < triangleCount; i++) {
		triangleAreas[i] = 0;
	}

	MarkWalkableTriangles(buildContext, configuration, input, triangleAreas);
	heightField->RasterizeTriangles(buildContext, configuration, input, triangleAreas);

	heightField->FilterLowHangingWalkableObstacles(buildContext, configuration);
	heightField->FilterLedgeSpans(buildContext, configuration);
	heightField->FilterWalkableLowHeightSpans(buildContext, configuration);	

	CompactHeightField^ chf = CompactHeightField::AllocateCompactHeightField();
	chf->Build(buildContext, configuration, heightField);
	heightField->Free();

	chf->ErodeWalkableArea(buildContext, configuration);

	//TODO:: MARK volume areas

	//TODO:: add another type of triangulation

	//Monotonne partition
	chf->BuildRegionsMonotone(buildContext, configuration);

	ContourSet^ contourSet = ContourSet::AllocateContourSet();
	contourSet->Build(buildContext, configuration, chf);


	PolyMesh^ polyMesh = PolyMesh::AllocatePolyMesh();
	polyMesh->Build(buildContext, configuration, chf, contourSet);


	PolyMeshDetail^ polyMeshDetail = PolyMeshDetail::AllocatePolyMeshDetail();
	polyMeshDetail->Build(buildContext, configuration, chf, polyMesh);

	chf->Free();
	contourSet->Free();

	//Comment next line for working with Detour
	polyMeshDetail->Free();

	//TODO:: we should return polyMeshDetail too
	
	return polyMesh;
	// TODO: insert return statement here
}
