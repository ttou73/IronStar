using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusion.Engine.Common {
	public interface IEditorInstance : IDisposable {
		void Initialize ();
		void Update ( GameTime gameTime );
	}
}
