using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusion.Core.Content {
	public interface IPrecachable {
		void Precache ( ContentManager content );
	}
}
