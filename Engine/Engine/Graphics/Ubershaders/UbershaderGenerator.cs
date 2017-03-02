using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Fusion.Core.Mathematics;
using System.Runtime.InteropServices;

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

			ReflectConstants( sb, type );
			ReflectStructures( sb, type );

			return sb.ToString();
		}



		static void ReflectStructures ( StringBuilder sb, Type type )
		{
			foreach ( var member in type.GetMembers(BindingFlags.NonPublic|BindingFlags.Public) ) {

				if (member.GetCustomAttributes().Any( attr => attr is ShaderStructureAttribute )) {

					if (member.MemberType==MemberTypes.NestedType) {
						var nestedType	=  (Type)member;

						sb.AppendFormat("// {0}\r\n", nestedType);
						sb.AppendFormat("// Marshal.SizeOf = {0}\r\n", Marshal.SizeOf(nestedType));
						sb.AppendFormat("struct {0} {{\r\n", nestedType.Name);

						foreach ( var field in nestedType.GetFields() ) {

							var offset	=	Marshal.OffsetOf( nestedType, field.Name );
							var size	=	Marshal.SizeOf( field.FieldType );

							//	TODO : check alignment rules
							//	https://msdn.microsoft.com/en-us/library/windows/desktop/bb509632(v=vs.85).aspx

							sb.AppendFormat("\t{0,-10} {1,-30} // offset: {2,4}\r\n", GetStructFieldHLSLType(field.FieldType), field.Name + ";", offset );
						}

						sb.AppendFormat("}}\r\n");
						sb.AppendFormat("\r\n");
					}
				}
			}
		}



		static void ReflectConstants ( StringBuilder sb, Type type )
		{
			foreach ( var field in type.GetFields(BindingFlags.Instance | 
                       BindingFlags.Static |
                       BindingFlags.NonPublic |
                       BindingFlags.Public) ) {

				if (field.GetCustomAttribute<ShaderDefineAttribute>()==null) {
					continue;
				}

				if (field.IsLiteral) {

					object value;

					if (field.FieldType==typeof(int)) value = field.GetValue(null);
					else if (field.FieldType==typeof(float)) value = field.GetValue(null);
					else if (field.FieldType==typeof(uint)) value = field.GetValue(null);
					else throw new Exception(string.Format("Bad type for HLSL definition : {0}", field.FieldType));

					sb.AppendFormat("#define {0,-16} {1}\r\n", field.Name, value.ToString());
				}
			}
			sb.AppendFormat("\r\n");
		}



		static string GetStructFieldHLSLType ( Type type )
		{
			if (type==typeof( int )) return "int";
			if (type==typeof( uint )) return "uint";
			if (type==typeof( float )) return "float";
			if (type==typeof( Vector2 )) return "float2";
			if (type==typeof( Vector3 )) return "float3";
			if (type==typeof( Vector4 )) return "float4";
			if (type==typeof( Int2 )) return "int2";
			if (type==typeof( Int3 )) return "int3";
			if (type==typeof( Int4 )) return "int4";
			if (type==typeof( Color3 )) return "float3";
			if (type==typeof( Color4 )) return "float4";
			if (type==typeof( Matrix )) return "float4x4";

			throw new Exception(string.Format("Bad HLSL type {0}", type));
		}
	}
}
