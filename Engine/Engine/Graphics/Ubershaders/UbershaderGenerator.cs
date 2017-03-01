using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Fusion.Engine.Graphics.Ubershaders {
	public class UbershaderGenerator {

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		static public string GenerateVirtualHeader ( Type type )
		{
			var sb = new StringBuilder();


			ReflectStructures( sb, type );


			return sb.ToString();
		}



		static void ReflectStructures ( StringBuilder sb, Type type )
		{
			foreach ( var member in type.GetMembers(BindingFlags.NonPublic|BindingFlags.Public) ) {

				if (member.GetCustomAttributes().Any( attr => attr is ShaderStructureAttribute )) {

					if (member.MemberType==MemberTypes.NestedType) {
						var nestedType	=  (Type)member;

						Log.Verbose("...{0}", nestedType);

						foreach ( var field in nestedType.GetFields() ) {
							Log.Verbose("      {0} {1}", field.FieldType, field.Name );
						}
					}
				}
			}
		}
	}
}
