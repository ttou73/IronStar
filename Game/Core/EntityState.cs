using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion;
using Fusion.Core;
using Fusion.Core.Utils;
using Fusion.Core.Mathematics;
using System.IO;

namespace ShooterDemo.Core {
	[Flags]
	public enum EntityState : int {

		None				=	0x0000,
		HasTraction			=	0x0001,
	}
}
