using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusion.Engine.Common {
	public class GameEditor : GameComponent {


		IEditorInstance	editor;


		/// <summary>
		/// Gets instance of the running editor or null.
		/// </summary>
		public IEditorInstance Instance {
			get {
				return editor;
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="game"></param>
		public GameEditor( Game game ) : base( game )
		{
		}



		/// <summary>
		/// 
		/// </summary>
		public override void Initialize()
		{
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose( bool disposing )
		{
			if (disposing) {
				SafeDispose( ref editor );
			}
			base.Dispose( disposing );
		}



		public void Start ( string map )
		{
			if (editor!=null) {
				Log.Warning("Editor is already started");
				return;
			}
			editor = Game.GameFactory.CreateEditor( Game, map );
			editor?.Initialize();
		}



		public void Stop ()
		{
			if (editor==null) {
				Log.Warning("Editor is not started");
			}
			SafeDispose( ref editor );
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update ( GameTime gameTime )
		{
			editor?.Update( gameTime );
		}
	}
}
