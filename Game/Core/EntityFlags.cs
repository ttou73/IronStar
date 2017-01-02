using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronStar.Core {
	[Flags]
	public enum EntityFlags {
		None			=	0x0000,
		InlineModel		=	0x0001,
	}
}
