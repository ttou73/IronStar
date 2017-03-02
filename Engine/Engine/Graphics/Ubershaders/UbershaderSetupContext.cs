using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Fusion.Core.Mathematics;
using System.Runtime.InteropServices;
using Fusion.Drivers.Graphics;
using Fusion.Engine.Common;

namespace Fusion.Engine.Graphics.Ubershaders {
	public class UbershaderSetupContext {

		public readonly Game Game;
		readonly RenderSystem rs;
		readonly GraphicsDevice device;

		readonly object targetObject;

		PropertyInfo[] samplers;
		PropertyInfo[] resources;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		public UbershaderSetupContext( RenderSystem rs, object target )
		{
			Game = rs.Game;
			this.rs = rs;
			device = Game.GraphicsDevice;

			targetObject	=	target;

			samplers	=	UbershaderGenerator.GetSamplerProperties(targetObject.GetType());
			resources	=	UbershaderGenerator.GetResourceProperties(targetObject.GetType());
		}


		public void ApplyPS ()
		{		
			for (int i=0; i<samplers.Length; i++) {
				device.PixelShaderResources[i] = (ShaderResource)resources[i].GetValue(targetObject);
			}

			for (int i=0; i<samplers.Length; i++) {
				device.PixelShaderSamplers[i] = (SamplerState)samplers[i].GetValue(targetObject);
			}
		}


		public void ApplyVS ()
		{		
			for (int i=0; i<samplers.Length; i++) {
				device.VertexShaderResources[i] = (ShaderResource)resources[i].GetValue(targetObject);
			}

			for (int i=0; i<samplers.Length; i++) {
				device.VertexShaderSamplers[i] = (SamplerState)samplers[i].GetValue(targetObject);
			}
		}


		public void ApplyGS ()
		{		
			for (int i=0; i<samplers.Length; i++) {
				device.GeometryShaderResources[i] = (ShaderResource)resources[i].GetValue(targetObject);
			}

			for (int i=0; i<samplers.Length; i++) {
				device.GeometryShaderSamplers[i] = (SamplerState)samplers[i].GetValue(targetObject);
			}
		}


		public void ApplyCS ()
		{		
			for (int i=0; i<samplers.Length; i++) {
				device.ComputeShaderResources[i] = (ShaderResource)resources[i].GetValue(targetObject);
			}

			for (int i=0; i<samplers.Length; i++) {
				device.ComputeShaderSamplers[i] = (SamplerState)samplers[i].GetValue(targetObject);
			}
		}
	}
}
