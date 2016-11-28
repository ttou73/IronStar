using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Engine.Common;
using Fusion.Core.Extensions;
using System.IO;
using Fusion.Engine.Storage;
using Fusion.Core.IniParser.Model;
using Fusion.Core.IniParser;

namespace Fusion.Core.Content {

	[ContentLoader(typeof(IniData))]
	public class IniLoader : ContentLoader {
		
		public override object Load ( ContentManager content, Stream stream, Type requestedType, string assetPath, IStorage storage )
		{
			var ip = new StreamIniDataParser();
			ip.Parser.Configuration.AllowDuplicateSections  =   false;
			ip.Parser.Configuration.AllowDuplicateKeys      =   false;
			ip.Parser.Configuration.AllowKeysWithoutSection =   true;
			ip.Parser.Configuration.CommentString           =   "#";
			ip.Parser.Configuration.OverrideDuplicateKeys   =   true;
			ip.Parser.Configuration.KeyValueAssigmentChar   =   '=';
			ip.Parser.Configuration.AllowKeysWithoutValues  =   true;

			var iniData = ip.ReadData( new StreamReader( stream ) );

			return iniData;
		}
	}
}
