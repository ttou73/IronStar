using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Core.Mathematics;
using Fusion.Core.Extensions;
using System.Diagnostics;
using System.IO;
using Fusion.Engine.Imaging;

namespace Fusion.Build.Mapping {


	/// <summary>
	/// http://www.memorymanagement.org/mmref/alloc.html
	/// </summary>
	public partial class Allocator2D {

		public readonly int Size;




		Block rootBlock;


		/// <summary>
		/// 
		/// </summary>
		/// <param name="size"></param>
		public Allocator2D ( int size = 1024 )
		{
			Size		=	size;
			rootBlock	=	new Block( new Int2(0,0), size, null, null );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		public Int2 Alloc ( int size, string tag )
		{
			if (tag==null) {
				throw new ArgumentNullException("tag");
			}
			if (size<=0) {
				throw new ArgumentOutOfRangeException("size");
			}

			size	=	MathUtil.RoundUpNextPowerOf2( size );

			var block = GetFreeBlock( size );
			block.Tag = tag;

			return block.Address;
		}



		/// <summary>
		/// Gets free block of appropriate size
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		Block GetFreeBlock ( int size )
		{
			var Q = new Stack<Block>();

			Q.Push( rootBlock );

			while ( Q.Any() ) {
				
				var block = Q.Pop();

				var state = block.State;

				// block is already allocated - skip
				if (state==BlockState.Allocated) {
					continue;
				}

				if (state==BlockState.Split) {
					if (block.Size/2>=size) {
						Q.Push( block.BottomRight );
						Q.Push( block.BottomLeft );
						Q.Push( block.TopRight	 );
						Q.Push( block.TopLeft	 );
					}
					continue;
				}

				if (state==BlockState.Free) {
					if (block.Size/2>=size) {
						block.Split();
						Q.Push( block.BottomRight );
						Q.Push( block.BottomLeft );
						Q.Push( block.TopRight	 );
						Q.Push( block.TopLeft	 );
					} else {
						return block;
					}
				}
			}

			throw new OutOfMemoryException(string.Format("No enough space in 2D allocator (size={0})", size));
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="address"></param>
		public int Free ( Int2 address )
		{
			var node = rootBlock;

			while (true) {
				
				if (node.Address==address) {
					if (node.State==BlockState.Allocated) {
						node.FreeAndMerge();
						return node.Size;
					}
				}

				if (node.IsAddressInside(address)) {

					if ( node.TopLeft.IsAddressInside(address) ) {
						node = node.TopLeft;
						continue;
					}	
					if ( node.TopRight.IsAddressInside(address) ) {
						node = node.TopRight;
						continue;
					}	
					if ( node.BottomLeft.IsAddressInside(address) ) {
						node = node.BottomLeft;
						continue;
					}	
					if ( node.BottomRight.IsAddressInside(address) ) {
						node = node.BottomRight;
						continue;
					}	

				} else {
					throw new InvalidOperationException(string.Format("Bad address [{0}, {1}]", address.X, address.Y));
				}
			}
		}



		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IEnumerable<BlockInfo> GetAllocatedBlockInfo ()
		{
			var list = new List<BlockInfo>();

			var S = new Stack<Block>();

			S.Push( rootBlock );

			while (S.Any()) {

				var block = S.Pop();

				if (block.State==BlockState.Allocated) {
					list.Add( new BlockInfo(block.Address, block.Size, block.Tag) );
				}

				if (block.State==BlockState.Split) {
					S.Push( block.BottomRight );
					S.Push( block.BottomLeft );
					S.Push( block.TopRight );
					S.Push( block.TopLeft );
				}
			}

			return list;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		public Size2 GetBlockSize ( Int2 address )
		{
			throw new NotImplementedException();
		}



		public static void SaveState ( Stream targetStream, Allocator2D allocator )
		{
			var writer = new BinaryWriter(targetStream, Encoding.Default, true);
			SaveState(  writer, allocator );
		}


		public static Allocator2D LoadState ( Stream sourceStream )
		{
			var reader = new BinaryReader(sourceStream, Encoding.Default, true);
			return LoadState( reader );
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="allocator"></param>
		/// <returns></returns>
		public static void SaveState ( BinaryWriter writer, Allocator2D allocator )
		{		
			//	write header:
			writer.WriteFourCC("MLC2");
			writer.WriteFourCC("1.00");

			writer.Write( allocator.Size );

			//	write nodes if depth-first order :
			var S = new Stack<Block>();

			S.Push( allocator.rootBlock );

			while (S.Any()) {

				var block = S.Pop();

				writer.WriteFourCC("BLCK");

				writer.Write( block.Address.X );
				writer.Write( block.Address.Y );
				writer.Write( block.Size );
				writer.Write( (int)block.State );

				if (block.State==BlockState.Allocated) {
					writer.Write( block.Tag );
				}

				if (block.State==BlockState.Split) {
					S.Push( block.BottomRight );
					S.Push( block.BottomLeft );
					S.Push( block.TopRight );
					S.Push( block.TopLeft );
				}
			}
		}



		public static Allocator2D LoadState ( BinaryReader reader )
		{
			//	read header:
			var fourcc  = reader.ReadFourCC();
			var version = reader.ReadFourCC();

			var size	= reader.ReadInt32();
			
			var allocator = new Allocator2D( size );


			//	read nodes if depth-first order :
			var S = new Stack<Block>();

			S.Push( allocator.rootBlock );

			while (S.Any()) {

				var block = S.Pop();

				var fcc = reader.ReadFourCC();

				block.Address.X = reader.ReadInt32();
				block.Address.Y = reader.ReadInt32();
				block.Size		= reader.ReadInt32();
				
				var state		=	(BlockState)reader.ReadInt32();

				if (state==BlockState.Allocated) {
					block.Tag = reader.ReadString();
				}

				if (state==BlockState.Split) {
					block.Split();
					S.Push( block.BottomRight );
					S.Push( block.BottomLeft );
					S.Push( block.TopRight );
					S.Push( block.TopLeft );
				}
			}

			return allocator;
		}



		/// <summary>
		/// Test method
		/// </summary>
		/// <param name="image"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		/// <param name="color"></param>
		static void DrawRectangle ( Image image, int x, int y, int w, int h, Color color, bool clear )
		{
			for (var i=x; i<x+w; i++) {
				for (var j=y; j<y+h; j++) {
					var c = image.Sample(i,j);

					if (!clear && c!=Color.Black) {
						Log.Warning("Overlap!");
					}
					image.Write( i,j, color );	
				}
			}
		}




		/// <summary>
		/// 
		/// </summary>
		/// <param name="dir"></param>
		public static void RunTest ( int size, int interations, string dir )
		{
			Log.Message("Allocator2D test: {0} {1} {2}", size, interations, dir );

			var dirInfo = Directory.CreateDirectory(dir);
			
			foreach (FileInfo file in dirInfo.GetFiles()) {
				file.Delete(); 
			}

			var alloc	= new Allocator2D(size);
			var image	= new Image(size,size, Color.Black);
			var rand	= new Random();

			var list    = new List<Int2>();

			for (int i=0; i<interations; i++) {

				Log.Message("{0,3:D3}/{1,3:D3}", i,interations);

				try {				

					bool allocNotFree = rand.NextFloat(0,1)<0.5f;
					bool reloadState  = rand.NextFloat(0,1)<0.1f;

					if (allocNotFree) {

						for (int j=0; j<rand.Next(1,16); j++) {
							//var sz = MathUtil.RoundUpNextPowerOf2( rand.Next(1,64) );
							var sz = rand.Next(1,64);

							var tag = string.Format("Block#{0,4:D4}##{1,4:D4}", i,sz);

							var a  = alloc.Alloc(sz, tag);

							list.Add(a);

							DrawRectangle( image, a.X, a.Y, sz,sz, rand.NextColor(), false );
						}

					} else {

						for (int j=0; j<rand.Next(1,16); j++) {
							
							if (!list.Any()) {
								break;
							}

							var id = rand.Next(list.Count);
							var fa = list[ id ];
							list.RemoveAt( id );

							var sz = alloc.Free(fa);

							DrawRectangle( image, fa.X, fa.Y, sz, sz, Color.Black, true );
						}
					}

					var imagePath = Path.Combine(dir, string.Format("allocTest{0,4:D4}.tga", i));
					var stackPath = Path.Combine(dir, string.Format("allocTest{0,4:D4}.stk", i));

					Image.SaveTga( image, imagePath );

					if (reloadState) {

						Log.Message("---- STATE RELOAD ----------------------------------");

						using ( var stream = File.OpenWrite( stackPath ) ) {
							Allocator2D.SaveState( stream, alloc );
						}

						using ( var stream = File.OpenRead( stackPath ) ) {
							alloc = Allocator2D.LoadState( stream );
						}
					}

				} catch ( Exception e ) {
					Log.Error("Exception: {0}", e.Message);
					continue;
				}
			}

			Log.Message("Done!");
		}
	}
}
