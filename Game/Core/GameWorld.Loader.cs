using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Fusion;
using Fusion.Core.Mathematics;
using Fusion.Engine.Common;
using Fusion.Core.Content;
using Fusion.Engine.Server;
using Fusion.Engine.Client;
using Fusion.Core.Extensions;
using IronStar.SFX;
using Fusion.Core.IniParser.Model;
using IronStar.Views;
using Fusion.Engine.Graphics;
using IronStar.Mapping;

namespace IronStar.Core {

	/// <summary>
	/// World represents entire game state.
	/// </summary>
	public partial class GameWorld : IServerInstance, IClientInstance {

		public class Loader : IGamePreloader {

			void IGamePreloader.LoadContent()
			{
				throw new NotImplementedException();
			}
		}

	}
}
