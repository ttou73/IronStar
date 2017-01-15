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

		public class Precacher : IContentPrecacher {

			readonly ContentManager content;
			readonly string serverInfo;

			/// <summary>
			/// 
			/// </summary>
			/// <param name="content"></param>
			/// <param name="serverInfo"></param>
			public Precacher ( ContentManager content, string serverInfo )
			{
				this.content	=	content;
				this.serverInfo	=	serverInfo;
			}


			/// <summary>
			/// 
			/// </summary>
			void IContentPrecacher.LoadContent()
			{
				content	.EnumerateAssets("fx")
						.Select( name => content.Precache<FXFactory>(@"fx\"+name) )
						.ToArray();


				content	.EnumerateAssets("models")
						.Select( name => content.Precache<ModelDescriptor>(@"models\"+name) )
						.ToArray();

				content	.EnumerateAssets("entities")
						.Select( name => content.Load<EntityFactory>(@"entities\"+name) )
						.ToArray();

				var map = content.Precache<Map>(@"maps\" + serverInfo );
			}
		}

	}
}
