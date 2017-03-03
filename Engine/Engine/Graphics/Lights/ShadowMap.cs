using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Core;
using Fusion.Core.Mathematics;
using Fusion.Core.Configuration;
using Fusion.Engine.Common;
using Fusion.Drivers.Graphics;
using System.Runtime.InteropServices;


namespace Fusion.Engine.Graphics {

	class ShadowMap : DisposableBase {

		readonly GraphicsDevice device;

		public const int MaxShadowmapSize	= 8192;
		

		/// <summary>
		/// Gets color shadow map buffer.
		/// Actually stores depth value.
		/// </summary>
		public RenderTarget2D ColorBuffer {
			get {
				return csmColor;
			}
		}



		/// <summary>
		/// Gets color shadow map buffer.
		/// Actually stores depth value.
		/// </summary>
		public RenderTarget2D ParticleShadow {
			get {
				return prtShadow;
			}
		}



		/// <summary>
		/// Gets color shadow map buffer.
		/// </summary>
		public DepthStencil2D DepthBuffer {
			get {
				return csmDepth;
			}
		}


		readonly int	shadowmapSize;
		DepthStencil2D	csmDepth;
		RenderTarget2D	csmColor;
		RenderTarget2D	prtShadow;



		/// <summary>
		/// 
		/// </summary>
		/// <param name="singleShadowMapSize"></param>
		/// <param name="splitCount"></param>
		public ShadowMap ( GraphicsDevice device, int size )
		{
			this.device			=	device;
			this.shadowmapSize	=	size;

			if (size<64 || size > MaxShadowmapSize) {
				throw new ArgumentOutOfRangeException("cascadeSize must be within range 64.." + MaxShadowmapSize.ToString());
			}

			if (!MathUtil.IsPowerOfTwo( shadowmapSize )) {
				Log.Warning("CascadedShadowMap : splitSize is not power of 2");
			}

			csmColor	=	new RenderTarget2D( device, ColorFormat.R32F,		shadowmapSize, shadowmapSize );
			csmDepth	=	new DepthStencil2D( device, DepthFormat.D24S8,		shadowmapSize, shadowmapSize );
			prtShadow	=	new RenderTarget2D( device, ColorFormat.Rgba8_sRGB,	shadowmapSize, shadowmapSize );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose ( bool disposing )
		{
			if (disposing) {
				SafeDispose( ref csmColor );
				SafeDispose( ref csmDepth );
				SafeDispose( ref prtShadow );
			}

			base.Dispose( disposing );
		}


		
		/// <summary>
		/// 
		/// </summary>
		public void Clear ()
		{
			device.Clear( csmDepth.Surface, 1, 0 );
			device.Clear( csmColor.Surface, Color4.White );
			device.Clear( prtShadow.Surface, Color4.White );
		}
	}
}
