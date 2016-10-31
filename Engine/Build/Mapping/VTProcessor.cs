﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Fusion.Core.Mathematics;
using Fusion.Core.Shell;
using Fusion.Engine.Imaging;
using Fusion.Build.Processors;
using Fusion.Engine.Graphics;
using Fusion.Engine.Storage;
using Fusion.Core.IniParser;
using Fusion.Core.IniParser.Model;

namespace Fusion.Build.Mapping {

	[AssetProcessor("MegaTexture", "Performs megatexture assembly")]
	public class VTProcessor : AssetProcessor {



		/// <summary>
		/// 
		/// </summary>
		public VTProcessor ()
		{
		}




		/// <summary>
		/// 
		/// </summary>
		/// <param name="buildContext"></param>
		public override void Process ( AssetSource assetFile, BuildContext context )
		{
			var localFileDir	=	Path.GetDirectoryName( assetFile.KeyPath );
			var mapStorage		=	context.GetAssetStorage(assetFile);
			var dependencies	=	new List<string>();//( assetFile.GetAllDependencies() );

			//
			//	Parse megatexture file :
			//
			var textures	=	ParseMegatexFile( assetFile );
			var pageTable	=	CreateVTTextureTable( textures, context );


			Log.Message("-------- virtual texture: {0} --------", assetFile.KeyPath );

			if (assetFile.TargetFileExists) {
				
				Log.Message("Removed:");
				foreach ( var rd in assetFile.GetRemovedDependencies()) {
					Log.Message("...removed: {0}", rd );
				}

				Log.Message("Changed:");
				foreach ( var cd in assetFile.GetChangedDependencies()) {
					Log.Message("...changed: {0}", cd );
				}
			}



			Log.Message("{0} textures", pageTable.SourceTextures.Count);

			//	packing textures to atlas :
			Log.Message("Packing textures to atlas...");
			PackTextureAtlas( pageTable.SourceTextures );

			//	generating pages :
			Log.Message("Generating pages...");
			GenerateMostDetailedPages( pageTable.SourceTextures, context, pageTable, mapStorage );

			//	generating mipmaps :
			Log.Message("Generating mipmaps...");
			for (int mip=0; mip<VTConfig.MipCount-1; mip++) {
				Log.Message("Generating mip level {0}/{1}...", mip, VTConfig.MipCount);
				GenerateMipLevels( context, pageTable, mip, mapStorage );
			}

			//	generating fallback image :
			/*Log.Message( "Generating fallback image..." );
			GenerateFallbackImage( context, pageTable, VTConfig.MipCount-1, mapStorage );*/

															 
			//
			//	Write asset and report files :
			//
			using ( var sw = new BinaryWriter(assetFile.OpenTargetStream(dependencies)) ) {

				sw.Write( pageTable.SourceTextures.Count );

				foreach ( var tex in pageTable.SourceTextures ) {
					sw.Write( tex.KeyPath );
					sw.Write( tex.TexelOffsetX );
					sw.Write( tex.TexelOffsetY );
					sw.Write( tex.Width );
					sw.Write( tex.Height );
				}
			}

			Log.Message("----------------" );
		}



		/// <summary>
		/// Reads raw megatex file
		/// </summary>
		/// <param name="assetFile"></param>
		/// <returns></returns>
		IEnumerable<string> ParseMegatexFile ( AssetSource assetFile )
		{
			return	File.ReadAllLines(assetFile.FullSourcePath)
					.Select( f1 => f1.Trim() )
					.Where( f2 => !f2.StartsWith("#") && !string.IsNullOrWhiteSpace(f2) )
					.Select( f3 => f3 )
					.ToArray();
		}



		/// <summary>
		/// Reads INI-file
		/// </summary>
		/// <param name="assetFile"></param>
		IniData ParseMegatexIniFile ( AssetSource assetFile )
		{
			

			var ip = new StreamIniDataParser();
			ip.Parser.Configuration.AllowDuplicateSections	=	false;
			ip.Parser.Configuration.AllowDuplicateKeys		=	true;
			ip.Parser.Configuration.CommentString			=	"#";
			ip.Parser.Configuration.OverrideDuplicateKeys	=	true;
			ip.Parser.Configuration.KeyValueAssigmentChar	=	'=';
			ip.Parser.Configuration.AllowKeysWithoutValues	=	false;

			using ( var reader = new StreamReader( assetFile.OpenSourceStream() ) ) {
				return	ip.ReadData( reader );
			}
		}

																					 

		/// <summary>
		/// Creates VT tex table from INI-data and base directory
		/// </summary>
		/// <param name="iniData"></param>
		/// <param name="baseDirectory"></param>
		/// <returns></returns>
		VTTextureTable CreateVTTextureTable ( IEnumerable<string> textures, BuildContext context )
		{
			var texTable	=	new VTTextureTable();

			foreach ( var texturePath in textures ) {
				var tex = new VTTexture( texturePath, context );

				texTable.AddTexture( tex );
			}

			return texTable;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="textures"></param>
		void PackTextureAtlas ( IEnumerable<VTTexture> textures )
		{
			var allocator = new Allocator2D( VTConfig.VirtualPageCount );

			foreach ( var tex in textures ) {

				var size = Math.Max(tex.Width/128, tex.Height/128);

				var addr = allocator.Alloc( size, tex );

				tex.TexelOffsetX	=	addr.X * 128;
				tex.TexelOffsetY	=	addr.Y * 128;
			}
		}



		/// <summary>
		/// Split textures on tiles.
		/// </summary>
		/// <param name="textures"></param>
		void GenerateMostDetailedPages ( ICollection<VTTexture> textures, BuildContext context, VTTextureTable pageTable, IStorage mapStorage )
		{
			int totalCount = textures.Count;
			int counter = 0;

			foreach ( var texture in textures ) {
				Log.Message("...{0}/{1} - {2}", counter, totalCount, texture.KeyPath );
				texture.SplitIntoPages( context, pageTable, mapStorage );
				counter++;
			}
		}



		/// <summary>
		/// Generates mip levels for all tiles.
		/// </summary>
		/// <param name="buildContext"></param>
		/// <param name="pageTable"></param>
		/// <param name="sourceMipLevel"></param>
		/// <param name="mapStorage"></param>
		void GenerateMipLevels ( BuildContext buildContext, VTTextureTable pageTable, int sourceMipLevel, IStorage mapStorage )
		{
			if (sourceMipLevel>=VTConfig.MipCount) {
				throw new ArgumentOutOfRangeException("mipLevel");
			}

			int count	= VTConfig.VirtualPageCount >> sourceMipLevel;
			int sizeB	= VTConfig.PageSizeBordered;
			var cache	= new TileSamplerCache( mapStorage, pageTable ); 

			for ( int pageX = 0; pageX < count; pageX+=2 ) {
				for ( int pageY = 0; pageY < count; pageY+=2 ) {

					var address00 = new VTAddress( pageX + 0, pageY + 0, sourceMipLevel );
					var address01 = new VTAddress( pageX + 0, pageY + 1, sourceMipLevel );
					var address10 = new VTAddress( pageX + 1, pageY + 0, sourceMipLevel );
					var address11 = new VTAddress( pageX + 1, pageY + 1, sourceMipLevel );
					
					//	there are no images touching target mip-level.
					//	NOTE: we can skip images that are touched by border.
					if ( !pageTable.IsAnyExists( address00, address01, address10, address11 ) ) {
						continue;
					}

					var address =	new VTAddress( pageX/2, pageY/2, sourceMipLevel+1 );

					var tile	=	new VTTile(address);

					var offsetX	=	(pageX) * VTConfig.PageSize;
					var offsetY	=	(pageY) * VTConfig.PageSize;
					var border	=	VTConfig.PageBorderWidth;

					var colorValue	=	Color.Zero;
					var normalValue	=	Color.Zero;
					var specularValue	=	Color.Zero;

					for ( int x=0; x<sizeB; x++) {
						for ( int y=0; y<sizeB; y++) {

							int srcX	=	offsetX + x*2 - border * 2;
							int srcY	=	offsetY + y*2 - border * 2;

							SampleMegatextureQ4( cache, srcX, srcY, sourceMipLevel, ref colorValue, ref normalValue, ref specularValue );
							
							tile.SetValues( x, y, ref colorValue, ref normalValue, ref specularValue );

						}
					}

					pageTable.Add( address );
					pageTable.SaveTile( address, mapStorage, tile );
				}
			}

		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="texelX"></param>
		/// <param name="texelY"></param>
		/// <param name="mipLevel"></param>
		/// <returns></returns>
		void SampleMegatextureQ4 ( TileSamplerCache cache, int texelX, int texelY, int mipLevel, ref Color a, ref Color b, ref Color c )
		{
			int textureSize	=	VTConfig.TextureSize >> mipLevel;
			
			texelX = MathUtil.Clamp( 0, texelX, textureSize );
			texelY = MathUtil.Clamp( 0, texelY, textureSize );

			int pageX	= texelX / VTConfig.PageSize;
			int pageY	= texelY / VTConfig.PageSize;
			int x		= texelX % VTConfig.PageSize;
			int y		= texelY % VTConfig.PageSize;
			int pbw		= VTConfig.PageBorderWidth;

			var address = new VTAddress( pageX, pageY, mipLevel );

			cache.LoadImage( address ).SampleQ4( x+pbw, y+pbw, ref a, ref b, ref c );
		}



		/// <summary>
		/// Generate one big texture where all textures are presented.
		/// </summary>
		void GenerateFallbackImage ( BuildContext buildContext, VTTextureTable pageTable, int sourceMipLevel, IStorage storage )
		{
			int		pageSize		=	VTConfig.PageSize;
			int		numPages		=	VTConfig.VirtualPageCount >> sourceMipLevel;
			int		fallbackSize	=	VTConfig.TextureSize >> sourceMipLevel;

			Image fallbackImage =	new Image( fallbackSize, fallbackSize, Color.Black );

			for ( int pageX=0; pageX<numPages; pageX++) {
				for ( int pageY=0; pageY<numPages; pageY++) {

					var addr	=	new VTAddress( pageX, pageY, sourceMipLevel );
					var image	=	pageTable.LoadPage( addr, storage );

					for ( int x=0; x<pageSize; x++) {
						for ( int y=0; y<pageSize; y++) {

							int u = pageX * pageSize + x;
							int v = pageY * pageSize + y;

							fallbackImage.Write( u, v, image.Sample( x, y ) );
						}
					}
				}
			}

			Image.SaveTga( fallbackImage, storage.OpenWrite("fallback.tga") );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="image00"></param>
		/// <param name="image01"></param>
		/// <param name="image10"></param>
		/// <param name="image11"></param>
		/// <returns></returns>
		Image MipImages ( Image image00, Image image01, Image image10, Image image11 )
		{
			const int pageSize = VTConfig.PageSize;

			if (image00.Width!=image00.Height || image00.Width != pageSize ) {
				throw new ArgumentException("Bad image size");
			}
			if (image01.Width!=image01.Height || image01.Width != pageSize ) {
				throw new ArgumentException("Bad image size");
			}
			if (image10.Width!=image10.Height || image10.Width != pageSize ) {
				throw new ArgumentException("Bad image size");
			}
			if (image11.Width!=image11.Height || image11.Width != pageSize ) {
				throw new ArgumentException("Bad image size");
			}

			var image = new Image( pageSize, pageSize, Color.Black );

			for ( int i=0; i<pageSize/2; i++) {
				for ( int j=0; j<pageSize/2; j++) {
					image.Write( i,j, image00.SampleMip( i, j ) );
				}
			}

			for ( int i=pageSize/2; i<pageSize; i++) {
				for ( int j=pageSize/2; j<pageSize; j++) {
					image.Write( i,j, image11.SampleMip( i, j ) );
				}
			}

			for ( int i=0; i<pageSize/2; i++) {
				for ( int j=pageSize/2; j<pageSize; j++) {
					image.Write( i,j, image01.SampleMip( i, j ) );
				}
			}

			for ( int i=pageSize/2; i<pageSize; i++) {
				for ( int j=0; j<pageSize/2; j++) {
					image.Write( i,j, image10.SampleMip( i, j ) );
				}
			}

			return image;
		} 


	}
}