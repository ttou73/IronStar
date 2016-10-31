using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Engine.Common;


namespace ShooterDemo.Core {
	public class EntityEventArgs : EventArgs {

		public readonly Entity Entity;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		public EntityEventArgs ( Entity e )
		{
			Entity = e;
		}
	}
}
