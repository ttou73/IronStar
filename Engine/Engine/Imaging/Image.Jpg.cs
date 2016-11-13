using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Fusion.Core.Mathematics;
using Color = Fusion.Core.Mathematics.Color;


namespace Fusion.Engine.Imaging {
	partial class Image {

		/*-----------------------------------------------------------------------------------------
		 * 
		 *	Image loading :
		 * 
		-----------------------------------------------------------------------------------------*/

		public static Size2 TakeJpgSize( Stream stream )
		{
			var decoder = new JpegBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat|BitmapCreateOptions.DelayCreation, BitmapCacheOption.OnDemand);
			var bitmapSource = decoder.Frames[0];
			return new Size2( bitmapSource.PixelWidth, bitmapSource.PixelHeight );
		}



		public static Image LoadJpg ( Stream stream )
		{
			var decoder = new JpegBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
			var bitmapSource = decoder.Frames[0];

			
			var bpp			=	bitmapSource.Format.BitsPerPixel;
			var format		=	bitmapSource.Format;
			var pixelCount  =   bitmapSource.PixelWidth * bitmapSource.PixelHeight;
			var byteCount	=	MathUtil.IntDivRoundUp( pixelCount * bpp, 8 );
			var stride		=	MathUtil.IntDivRoundUp( bitmapSource.PixelWidth * bpp, 8 );
			var pixels		=	new byte[ byteCount ];

			bitmapSource.CopyPixels( Int32Rect.Empty, pixels, stride, 0 );


			var image   =   new Image( bitmapSource.PixelWidth, bitmapSource.PixelHeight, Color.Black );


			if (format==PixelFormats.Bgr24) {
				for ( int i = 0; i<pixelCount; i++ ) {
					var offset	=	i * 3;
					var color	=	new Color( pixels[offset+2], pixels[offset+1], pixels[offset+0] );
					image.RawImageData[ i ]		=	color;
				}
			} else if ( format==PixelFormats.Bgra32 ) {
				for ( int i = 0; i<pixelCount; i++ ) {
					var offset  =   i * 4;
					var color   =   new Color( pixels[offset+2], pixels[offset+1], pixels[offset+0], pixels[offset+3] );
					image.RawImageData[i]     =   color;
				}
			} else if ( format==PixelFormats.Gray8 ) {
				for ( int i = 0; i<pixelCount; i++ ) {
					var offset  =   i * 1;
					var color   =   new Color( pixels[offset], pixels[offset], pixels[offset], (byte)255 );
					image.RawImageData[i]     =   color;
				}
			} else {
				throw new NotSupportedException( string.Format("PNG format {0} is not supported", format) );
			}

			return image;
		}

	}
}
