using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion;
using Fusion.Core.Utils;

namespace ShooterDemo.Core {
	public class Factory {

		/// <summary>
		///	Computes CRC32 for given prefab name.
		/// </summary>
		/// <param name="prefabName"></param>
		/// <returns></returns>
		public static uint GetPrefabID ( string prefabName )
		{
			return Crc32.ComputeChecksum( Encoding.ASCII.GetBytes(prefabName.ToLowerInvariant()) );
		}

	}
}
