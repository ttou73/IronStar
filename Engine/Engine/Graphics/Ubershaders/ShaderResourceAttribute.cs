using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusion.Engine.Graphics.Ubershaders {

	enum ShaderResourceType {
		Texture2D,
		Texture2DArray,
		Texture3D,
		TextureCube,
		TextureCubeArray,
		StructuredBuffer,
	}

	internal sealed class ShaderResourceAttribute : Attribute {

		public readonly ShaderResourceType ResourceType;
		public readonly Type StructureType;

		public ShaderResourceAttribute( ShaderResourceType resourceType, Type structureType = null )
		{
			ResourceType	= resourceType;
			StructureType	= structureType;

			if (resourceType==ShaderResourceType.StructuredBuffer && structureType==null) {
				throw new ArgumentException("StructuredBuffer requires specified type");
			}
		}
	}
}
