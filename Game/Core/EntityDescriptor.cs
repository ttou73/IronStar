using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Engine.Common;
using Fusion.Core.Mathematics;
using Fusion.Core.IniParser.Model;

namespace IronStar.Core {
	public abstract class EntityDescriptor : Dictionary<string,string> {

		/// <summary>
		/// 
		/// </summary>
		/// <param name="world"></param>
		/// <param name="entity"></param>
		public EntityDescriptor ()
		{
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="iniSectionData"></param>
		public EntityDescriptor( SectionData iniSectionData )
		{
			foreach (var keyValue in iniSectionData.Keys ) {
				base.Add( keyValue.KeyName, keyValue.Value );
			}
		}

	}
}
