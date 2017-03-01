using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Core.Extensions;

namespace Fusion.Engine.Graphics.Ubershaders {

	/// <summary>
	/// Marks class as requiring the list of shaders.
	/// </summary>
	[AttributeUsage(AttributeTargets.Struct)]
	internal sealed class ShaderStructureAttribute : Attribute {

	}
}
