using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Fusion.Core.Mathematics;
using System.Runtime.InteropServices;
using Fusion.Drivers.Graphics;

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

			ReflectDefinitions( sb, type );
			ReflectStructures( sb, type );
			ReflectConstants( sb, type );
			ReflectResources( sb, type );
			ReflectSamplers( sb, type );

			return sb.ToString();
		}



		public static PropertyInfo[] GetSamplerProperties ( Type type )
		{
			var list = type
				.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.Where( p1 => p1.GetCustomAttribute<ShaderSamplerAttribute>() != null )
				.Where( p2 => p2.PropertyType.IsSubclassOf(typeof(SamplerState)) || p2.PropertyType == typeof(SamplerState) )
				.ToArray();

			return list;
		}



		public static PropertyInfo[] GetConstantBufferProperties ( Type type )
		{
			var list = type
				.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.Where( p1 => p1.GetCustomAttribute<ShaderConstantBufferAttribute>() != null )
				.Where( p2 => p2.PropertyType.IsSubclassOf(typeof(ConstantBuffer)) || p2.PropertyType == typeof(ConstantBuffer) )
				.ToArray();

			return list;
		}



		public static PropertyInfo[] GetResourceProperties ( Type type )
		{
			var list = type
				.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.Where( p1 => p1.GetCustomAttribute<ShaderResourceAttribute>() != null )
				.Where( p2 => p2.PropertyType.IsSubclassOf(typeof(ShaderResource)) || p2.PropertyType == typeof(ShaderResource) )
				.ToArray();

			return list;
		}


		/*-----------------------------------------------------------------------------------------
		 * 
		 *	Text generating :
		 * 
		-----------------------------------------------------------------------------------------*/

		static void ReflectResources ( StringBuilder sb, Type type )
		{
			int register = 0;

			foreach ( var prop in GetResourceProperties(type) ) {

				var srvAttr = prop.GetCustomAttribute<ShaderResourceAttribute>();

				var prefix = "";
				if (srvAttr.StructureType!=null) {
					prefix = "<" + srvAttr.StructureType.Name + ">";
				}

				sb.AppendFormat("{0}{1} {2} : register(t{3});\r\n", srvAttr.ResourceType, prefix, prop.Name, register++);
			}

		}



		static void ReflectConstants ( StringBuilder sb, Type type )
		{
			int register = 0;

			//	cbuffer CBLightingParams : register(b0) { 
			//		LightingParams Params : packoffset( c0 ); 
			//	};

			foreach ( var prop in GetConstantBufferProperties(type) ) {

				var cbAttr = prop.GetCustomAttribute<ShaderConstantBufferAttribute>();

				var array = (cbAttr.ArraySize==0) ? "" : "[" + cbAttr.ArraySize.ToString() + "]";

				sb.AppendFormat("cbuffer __buffer{0} : register(b{0}) {{\r\n", register++);
				sb.AppendFormat("\t{0}{1} {2} : packoffset(c0);\r\n", cbAttr.ConstantType.Name, array, prop.Name);
				sb.AppendFormat("}};\r\n", register++);
			}

		}



		static void ReflectSamplers ( StringBuilder sb, Type type )
		{
			int register = 0;

			foreach ( var prop in GetSamplerProperties(type) ) {

				var smAttr = prop.GetCustomAttribute<ShaderSamplerAttribute>();

				if (prop.PropertyType!=typeof(SamplerState)) {
					throw new ArgumentException(string.Format("Property {0} must be SamplerState", prop.Name));
				}

				var sampler = smAttr.IsComparison ? "SamplerComparisonState" : "SamplerState";

				sb.AppendFormat("{0} {1} : register(s{2});\r\n", sampler, prop.Name, register++);
			}

		}



		static void ReflectStructures ( StringBuilder sb, Type type )
		{
			var sharedStruct = type.GetCustomAttribute<ShaderSharedStructureAttribute>();

			if (sharedStruct!=null) {
				foreach ( var structType in sharedStruct.StructTypes ) {
					ReflectStructure( sb, structType );
				}
			}

			foreach ( var member in type.GetMembers(BindingFlags.NonPublic|BindingFlags.Public) ) {

				if (member.GetCustomAttributes().Any( attr => attr is ShaderStructureAttribute )) {

					if (member.MemberType==MemberTypes.NestedType) {
						var nestedType	=  (Type)member;
						ReflectStructure( sb, nestedType );
					}
				}
			}
		}



		static int SizeOf( Type type )
		{
			if (type.IsEnum) {
				return Marshal.SizeOf(Enum.GetUnderlyingType(type));
			} else {
				return Marshal.SizeOf(type);
			}
		}



		static void ReflectStructure ( StringBuilder sb, Type nestedType )
		{			
            //	https://msdn.microsoft.com/en-us/library/windows/desktop/bb509632(v=vs.85).aspx
			//CheckAlligmentRules(nestedType);

			sb.AppendFormat("// {0}\r\n", nestedType);
			sb.AppendFormat("// Marshal.SizeOf = {0}\r\n", Marshal.SizeOf(nestedType));
			sb.AppendFormat("struct {0} {{\r\n", nestedType.Name);

			foreach ( var field in nestedType.GetFields() ) {

				var offset	=	Marshal.OffsetOf( nestedType, field.Name );
				var size	=	SizeOf( field.FieldType );

                sb.AppendFormat("\t{0,-10} {1,-30} // offset: {2,4}\r\n", GetStructFieldHLSLType(field.FieldType), field.Name + ";", offset );
			}

			sb.AppendFormat("}};\r\n");
			sb.AppendFormat("\r\n");
		}



		static void ReflectDefinitions ( StringBuilder sb, Type type )
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
			if (type.IsEnum) return GetStructFieldHLSLType(Enum.GetUnderlyingType(type));

			throw new Exception(string.Format("Bad HLSL type {0}", type));
		}



        private static void CheckAlligmentRules(Type type)
        {
            var fields = type.GetFields().ToList();
            fields.Sort((a, b) => Marshal.OffsetOf(type, a.Name).ToInt32().CompareTo(Marshal.OffsetOf(type, b.Name).ToInt32()));
            int accOffset = 0;

            foreach (var fi in fields) {
                int size = Marshal.SizeOf(fi.FieldType);
                int curOffset = Marshal.OffsetOf(type, fi.Name).ToInt32();

                if (accOffset / 16 == curOffset / 16 && accOffset % 16 != 0 && accOffset % 16 + size > 16)
                {
                    throw new ArgumentException($"Field {fi.Name} in struct {type.Name} has wrong offset {curOffset}. Offset must be {accOffset / 16 * 17}");
                }
                accOffset = curOffset + size;
            }
        }
	}
}
