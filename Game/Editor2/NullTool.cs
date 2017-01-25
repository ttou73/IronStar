using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Engine.Graphics;
using Fusion.Core.Mathematics;
using Fusion.Engine.Common;
using Fusion;
using IronStar.Mapping;

namespace IronStar.Editor2 {
	public class NullTool : Manipulator {


		/// <summary>
		/// 
		/// </summary>
		public NullTool ( MapEditor editor ) : base(editor)
		{
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update ( GameTime gameTime, int x, int y )
		{
		}


		public override bool IsManipulating {
			get {
				return false;
			}
		}


		public override bool StartManipulation ( int x, int y )
		{
			return false;
		}


		public override void UpdateManipulation ( int x, int y )
		{
		}


		public override void StopManipulation ( int x, int y )
		{
		}

	}
}
