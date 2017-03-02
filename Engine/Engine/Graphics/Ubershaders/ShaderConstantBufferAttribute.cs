using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusion.Engine.Graphics.Ubershaders {

	internal sealed class ShaderConstantBufferAttribute : Attribute {

		public readonly Type ConstantType; 
		public readonly int ArraySize;

		public ShaderConstantBufferAttribute ( Type constantType, int arraySize = 0 )
		{
			ArraySize		=	arraySize;
			ConstantType	=	constantType;
		}
	}
}
