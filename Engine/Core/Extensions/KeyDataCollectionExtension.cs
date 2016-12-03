using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Core.IniParser.Model;

namespace Fusion.Core.Extensions {
	public static class KeyDataCollectionExtension {

		/// <summary>
		/// Gets value from KeyDataCollection.
		/// If value is missing returns proveded default value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="keyDataCollection"></param>
		/// <param name="keyName"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static T Get<T>( this KeyDataCollection keyDataCollection, string keyName, T defaultValue )
		{
			var stringValue = keyDataCollection[keyName];

			if (!string.IsNullOrWhiteSpace(stringValue)) {
				return StringConverter.FromString<T>( stringValue );
			} else {
				return defaultValue;
			}
		}
		
	}
}
