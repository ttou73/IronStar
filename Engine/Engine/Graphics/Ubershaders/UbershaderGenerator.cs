﻿using System;
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
			ReflectResources( sb, type );

			return sb.ToString();
		}



		static void ReflectResources ( StringBuilder sb, Type type )
		{
			int register = 0;

			foreach ( var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public) ) {

				var srvAttr = prop.GetCustomAttribute<ShaderResourceAttribute>();

				if (srvAttr==null) {
					continue;
				}

				var prefix = "";
				if (srvAttr.StructureType!=null) {
					prefix = "<" + srvAttr.StructureType.Name + ">";
				}

				sb.AppendFormat("{0}{1} {2} : register({3});\r\n", srvAttr.ResourceType, prefix, prop.Name, register++);
			}

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

                            //	https://msdn.microsoft.com/en-us/library/windows/desktop/bb509632(v=vs.85).aspx
                            CheckAlligmentRules(field.FieldType);

                            sb.AppendFormat("\t{0,-10} {1,-30} // offset: {2,4}\r\n", GetStructFieldHLSLType(field.FieldType), field.Name + ";", offset );
						}

						sb.AppendFormat("}};\r\n");
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


        private static void CheckAlligmentRules(Type type)
        {

            var t = type.GetRuntimeFields().Where(f => !f.IsStatic).ToList();
            t.Sort((a, b) => Marshal.OffsetOf(type, a.Name).ToInt32().CompareTo(Marshal.OffsetOf(type, b.Name).ToInt32()));
            int accOffset = 0;
            foreach (var fi in t)
            {
                int size = Marshal.SizeOf(fi);
                int curOffset = Marshal.OffsetOf(type, fi.Name).ToInt32();
                
                //it means that user uses a FieldOffset attribute.
                if (curOffset != accOffset)
                {
                    if (accOffset / 16 == curOffset / 16 && accOffset % 16 != 0 && accOffset % 16 + size >= 16)
                    {
                        throw new ArgumentException($"Field {fi.Name} in struct {type.Name} has wrong offset {curOffset}. Offset must be {accOffset / 16 * 17}");
                    }
                }  else // we should
                {
                    if (accOffset % 16 + size >= 16)
                    {
                        throw new ArgumentException($"Field {fi.Name} in struct {type.Name} has wrong offset {curOffset}. Offset must be {accOffset / 16 * 17}");
                    }
                }

                accOffset = curOffset + size;
            }
        }
	}
}