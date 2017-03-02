using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusion.Engine.Graphics.Ubershaders {

	internal sealed class ShaderSamplerAttribute : Attribute {

		public readonly bool IsComparison; 

		public ShaderSamplerAttribute ( bool comparison = false)
		{
			IsComparison = comparison;
		}
	}
}
