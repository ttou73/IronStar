using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Engine.Client;
using Fusion.Engine.Server;

namespace Fusion.Engine.Common {
	public interface IGameFactory {
		IGameLoader		CreateLoader ( Game game, string serverInfo );
		IClientInstance CreateClient ( Game game, string serverInfo );
		IServerInstance CreateServer ( Game game, string map );
		IUserInterface	CreateUI ( Game game );
	}
}
