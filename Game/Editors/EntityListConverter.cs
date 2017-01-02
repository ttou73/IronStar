using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Fusion.Core.Content;
using Fusion.Build;

namespace IronStar.Editors {

	public class EntityListConverter : TypeConverter {
		
		public override bool GetStandardValuesSupported( ITypeDescriptorContext context )
		{
			return true; // display drop
		}
		
		public override bool GetStandardValuesExclusive( ITypeDescriptorContext context )
		{
			return true; // drop-down vs combo
		}
		
		public override StandardValuesCollection GetStandardValues( ITypeDescriptorContext context )
		{
			var list = Directory
						.GetFiles( Path.Combine(Builder.FullInputDirectory, "entities"), "*.xml")
						.Select( name => Path.GetFileNameWithoutExtension(name) )
						.ToArray();
			
			return new StandardValuesCollection( list );
		}
	}
}
