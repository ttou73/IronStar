using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusion.Engine.Graphics.Ubershaders {

	[AttributeUsage(AttributeTargets.Class)]
	internal sealed class ShaderSharedStructureAttribute : Attribute {

		public readonly Type[] StructTypes; 

		public ShaderSharedStructureAttribute (params Type[] structTypes)
		{
			StructTypes = structTypes;
		}
	}
}
