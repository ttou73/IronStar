using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Fusion;
using Fusion.Engine.Imaging;
using Fusion.Core.Mathematics;
using Fusion.Core.Extensions;
using Fusion.Engine.Graphics;
using Fusion.Engine.Storage;

namespace Fusion.Build.Mapping {

	class TileSamplerCache {

		LRUCache<VTAddress, VTTile> cache;

		readonly IStorage storage;
			
		public TileSamplerCache ( IStorage mapStorage )
		{
			this.storage	=	mapStorage;
			this.cache		=	new LRUCache<VTAddress,VTTile>(128);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		public VTTile LoadImage ( VTAddress address )
		{
			VTTile tile;

			if (!cache.TryGetValue(address, out tile)) {
				
				var path	=	address.GetFileNameWithoutExtension(".tile");

				if (storage.FileExists(path)) {
					tile	=	new VTTile(address);
					tile.Read( storage.OpenFile(path, FileMode.Open, FileAccess.Read) );
				} else {
					tile	=	new VTTile(address);
					tile.Clear( Color.Black );
				}

				cache.Add( address, tile );

			}
			
			return tile;
		}
	}
}
