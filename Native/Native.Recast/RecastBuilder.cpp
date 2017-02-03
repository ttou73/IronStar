#include "RecastBuilder.h"
using namespace Native::Recast;

PolyMesh^ RecastBuilder::BuildNavigationMesh(RecastMesh ^ input, RCConfig^ configuration, BuildContext^ buildContext, bool generatePolyMeshDetail)
{
	int triangleCount = input->Indices->Length / 3;
	int verticesCount = input->Vertices->Length;

	configuration->BMin = CalculateBmin(input);
	configuration->BMax = CalculateBmax(input);

	CalculateGridSize(configuration);

	HeightField^ heightField = gcnew HeightField();
	
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
		
	CompactHeightField^ chf = gcnew CompactHeightField();
	chf->Build(buildContext, configuration, heightField);
	
	heightField->~HeightField();

	chf->ErodeWalkableArea(buildContext, configuration);

	//TODO:: MARK volume areas

	//Monotonne partition

	switch (configuration->PartitionType)
	{
	case PartitionType::Watershed:
		chf->BuildDistanceField(buildContext, configuration);
		chf->BuildRegions(buildContext, configuration);
			break;
	case PartitionType::Monotone:
		chf->BuildRegionsMonotone(buildContext, configuration);
		break;
	case PartitionType::Layer:
		chf->BuildLayerRegions(buildContext, configuration);
		break;
	default:
		throw gcnew System::Exception();
		break;
	}


	ContourSet^ contourSet = gcnew ContourSet();
	contourSet->Build(buildContext, configuration, chf);


	PolyMesh^ polyMesh = gcnew PolyMesh();
	polyMesh->Build(buildContext, configuration, chf, contourSet);


	if (generatePolyMeshDetail) {
		PolyMeshDetail^ polyMeshDetail = gcnew PolyMeshDetail();
		polyMeshDetail->Build(buildContext, configuration, chf, polyMesh);
		polyMesh->Details = polyMeshDetail;
	}
	chf->~CompactHeightField();
	contourSet->~ContourSet();
	
	return polyMesh;
}
