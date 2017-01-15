using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Engine.Common;
using Fusion.Core.Content;

namespace Fusion.Engine.Client {
	public interface IContentPrecacher {

		/// <summary>
		/// Loads content in separate thread
		/// </summary>
		void LoadContent ();
	}
}
