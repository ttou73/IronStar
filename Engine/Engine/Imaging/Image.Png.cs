using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;
using Fusion.Core.Mathematics;


namespace Fusion.Engine.Imaging {
	partial class Image {

		/*-----------------------------------------------------------------------------------------
		 * 
		 *	Image loading :
		 * 
		-----------------------------------------------------------------------------------------*/

		public static Image LoadPng ( Stream stream )
		{
			PngBitmapDecoder decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
			BitmapSource bitmapSource = decoder.Frames[0];

			var image	=	new Image( bitmapSource.PixelWidth, bitmapSource.PixelHeight, Color.Black );

			var pixels	=	new byte[ image.Width * image.Height * 4 ];

			bitmapSource.CopyPixels( Int32Rect.Empty, pixels, image.Width*4, 0 );

			return image;
		}

	}
}
