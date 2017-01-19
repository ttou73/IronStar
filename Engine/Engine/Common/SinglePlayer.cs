using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusion.Engine.Common {
	public class SinglePlayer : GameComponent {

		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="game"></param>
		public SinglePlayer( Game game ) : base( game )
		{
		}



		/// <summary>
		/// 
		/// </summary>
		public override void Initialize()
		{
		}



		/// <summary>
		/// Starts the single player game.
		/// </summary>
		/// <param name="map"></param>
		public void Start ( string map )
		{
		}



		/// <summary>
		/// Stops the single player game
		/// </summary>
		public void Kill ()
		{
		}


		/// <summary>
		/// Sets and gets pause state of the single game.
		/// </summary>
		public bool Pause {
			get {
				return pause;
			}
			set {
				pause = value;
			}
		}

		bool pause = false;



		/// <summary>
		/// Updates state of the single player game
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update ( GameTime gameTime )
		{
		}



	}
}
