﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Fusion;
using Fusion.Core.Mathematics;
using Fusion.Core.Extensions;
using Fusion.Engine.Graphics;
using Fusion.Engine.Imaging;
using Fusion.Engine.Storage;
using Fusion.Core.IniParser.Model;
using Fusion.Core.Content;

namespace Fusion.Build.Mapping {


	internal class VTTexture {

		readonly BuildContext context;

		public readonly string	KeyPath;

		public readonly string  BaseColor	;
		public readonly string  NormalMap	;
		public readonly string  Metallic	;
		public readonly string  Roughness	;
		public readonly string	Emission	;

		public int TexelOffsetX;
		public int TexelOffsetY;

		public readonly int Width;
		public readonly int Height;

		public int AddressX { get { return TexelOffsetX / VTConfig.PageSize; } }
		public int AddressY { get { return TexelOffsetY / VTConfig.PageSize; } }


		/// <summary>
		/// 
		/// </summary>
		/// <param name="fullPath"></param>
		public VTTexture ( SectionData section, BuildContext context )
		{			
			this.context		=	context;
			const int pageSize	=	VTConfig.PageSize;

			KeyPath		=	section.SectionName;
			BaseColor	=   section.Keys["BaseColor"];
			NormalMap   =   section.Keys["NormalMap"];
			Metallic    =   section.Keys["Metallic" ];
			Roughness   =   section.Keys["Roughness"];
			Emission    =   section.Keys["Emission" ];

			if (string.IsNullOrWhiteSpace(BaseColor)) {	
				throw new BuildException("BaseColor must be specified for material '" + section.SectionName + "'");
			}
			
			var fullPath	=	context.ResolveContentPath( BaseColor );

			var size = TakeImageSize( KeyPath, fullPath, context );

			if (size.Height%pageSize!=0) {
				throw new BuildException(string.Format("Width of '{0}' must be multiple of {1}", fullPath, pageSize));
			}
			if (size.Width%pageSize!=0) {
				throw new BuildException(string.Format("Height of '{0}' must be multiple of {1}", fullPath, pageSize));
			}

			Width	=	size.Width;
			Height	=	size.Height;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="keyPath"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		Size2 TakeImageSize ( string mtrlName, string keyPath, BuildContext context )
		{
			var fullPath	=	context.ResolveContentPath( keyPath );
			var ext			=	Path.GetExtension(keyPath).ToLowerInvariant();

			using ( var stream = File.OpenRead( fullPath ) ) {
				if ( ext==".tga" ) {
					var header = Image.TakeTga( stream );
					return new Size2( header.width, header.height );
				} else
				if ( ext==".png" ) {
					return Image.TakePngSize( stream );
				} else
				if ( ext==".jpg" ) {
					return Image.TakeJpgSize( stream );
				} else {
					throw new BuildException("Material " + mtrlName + " must refer TGA or PNG image");
				}
			}
		}


		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="uv"></param>
		/// <returns></returns>
		public Vector2 RemapTexCoords ( Vector2 uv )
		{
			double size	= VTConfig.TextureSize;

			double u = ( MathUtil.Wrap(uv.X,0,1) * Width  + (double)TexelOffsetX ) / size;
			double v = ( MathUtil.Wrap(uv.Y,0,1) * Height + (double)TexelOffsetY ) / size;

			return new Vector2( (float)u, (float)v );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="pageTable"></param>
		public void SplitIntoPages ( BuildContext context, VTTextureTable pageTable, IStorage storage )
		{
			var pageSize			=	VTConfig.PageSize;
			var pageSizeBorder		=	VTConfig.PageSizeBordered;
			var border				=	VTConfig.PageBorderWidth;

			var colorMap			=	LoadTexture( BaseColor, Color.Gray );
			var normalMap			=	LoadTexture( NormalMap, Color.FlatNormals );
			var roughness			=	LoadTexture( Roughness, Color.Black );
			var metallic			=	LoadTexture( Metallic,	Color.Black );
			var emission			=	LoadTexture( Emission,	Color.Black );

			var pageCountX	=	colorMap.Width / pageSize;
			var pageCountY	=	colorMap.Height / pageSize;

			for (int x=0; x<pageCountX; x++) {
				for (int y=0; y<pageCountY; y++) {

					var pageC	=	new Image(pageSizeBorder, pageSizeBorder); 
					var pageN	=	new Image(pageSizeBorder, pageSizeBorder); 
					var pageS	=	new Image(pageSizeBorder, pageSizeBorder); 
					
					for ( int i=0; i<pageSizeBorder; i++) {
						for ( int j=0; j<pageSizeBorder; j++) {

							int srcX		=	x*pageSize + i - border;
							int srcY		=	y*pageSize + j - border;

							var c	=	colorMap.SampleClamp( srcX, srcY );
							var n	=	normalMap.SampleClamp( srcX, srcY );
							var r	=	roughness.SampleClamp( srcX, srcY ).R;
							var m	=	metallic.SampleClamp( srcX, srcY ).R;
							var e	=	emission.SampleClamp( srcX, srcY ).R;

							pageC.Write( i,j, c );
							pageN.Write( i,j, n );
							pageS.Write( i,j, new Color( r,m,e, (byte)255 ) );
						}
					}

					var address	=	new VTAddress( (short)(x + AddressX), (short)(y + AddressY), 0 );
					pageTable.Add( address );

					var tile	=	new VTTile( address, pageC, pageN, pageS );
					pageTable.SaveTile( address, storage, tile );
				}
			}
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="baseImage"></param>
		/// <param name="postfix"></param>
		/// <param name="defaultColor"></param>
		/// <returns></returns>
		Image LoadTexture ( string texturePath, Color defaultColor ) 
		{
			if ( string.IsNullOrWhiteSpace(texturePath) || !context.ContentFileExists( texturePath ) ) {
				return new Image( Width, Height, defaultColor );
			}

			var fullPath    =   context.ResolveContentPath( texturePath );
			var ext         =   Path.GetExtension(texturePath).ToLowerInvariant();

			Image image		=	null;

			using ( var stream = File.OpenRead( fullPath ) ) {
				if ( ext==".tga" ) {
					image = Image.LoadTga( stream );
				} else
				if ( ext==".png" ) {
					image = Image.LoadPng( stream );
				} else
				if ( ext==".jpg" ) {
					image = Image.LoadJpg( stream );
				} else {
					throw new BuildException( "Only TGA or PNG images are supported." );
				}
			}

			if ( image.Width!=Width || image.Height!=image.Height ) {
				Log.Warning( "Size of {0} is not equal to size of {1}. Default image is used.", texturePath, KeyPath );
				return new Image( Width, Height, defaultColor );
			}

			return image;
		}

	}
}
