using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Engine.Common;
using Fusion.Core.Mathematics;
using Fusion.Engine.Server;
using System.IO;

namespace IronStar.Core {
	class ServerWorld {

		/// <summary>
		/// 
		/// </summary>
		public ServerWorld ( GameServer server, string map )
		{
			throw new NotImplementedException();
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update ( GameTime gameTime )
		{
			throw new NotImplementedException();
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="className"></param>
		/// <param name="parameters"></param>
		public Entity SpawnEntity ( string className, Dictionary<string,string> parameters )
		{
			throw new NotImplementedException();
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sfxName"></param>
		/// <param name="parentID"></param>
		/// <param name="position"></param>
		/// <param name="velocity"></param>
		/// <param name="rotation"></param>
		public void SpawnSFX ( string sfxName, uint parentID, Vector3 position, Vector3 velocity, Quaternion rotation )
		{
			throw new NotImplementedException();
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		public void WriteSnapshot ( BinaryWriter writer )
		{
			throw new NotImplementedException();
		}
	}
}
