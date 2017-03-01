using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusion.Engine.Graphics.Ubershaders {

	/// <summary>
	/// See also: http://stackoverflow.com/questions/33477163/get-value-of-constant-by-name
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	class ShaderDefineAttribute : Attribute {
	}
}
