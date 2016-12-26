using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Engine.Common;
using Fusion.Core.Content;

namespace Fusion.Engine.Client {
	public interface IGameLoader {

		/// <summary>
		/// Loads client instance 
		/// </summary>
		/// <returns></returns>
		IClientInstance Load ();
	}
}
