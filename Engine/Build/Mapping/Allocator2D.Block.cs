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


		enum BlockState {
			Free = 0,
			Split = 1,
			Allocated = 2,
		}


		public class BlockInfo {

			public BlockInfo( Int2 address, int size, string tag )
			{
				Address	=	address;
				Size	=	size;
				Tag		=	tag;
			}

			public readonly Int2	Address;
			public readonly int		Size;
			public readonly string	Tag;
		}



		class Block {

			public Int2	Address;
			public int	Size;

			public Block TopLeft;
			public Block TopRight;
			public Block BottomLeft;
			public Block BottomRight;
			public Block Parent;

			public string Tag {
				get; set;
			}


			public BlockState State {
				get {
					if (TopLeft==null && BottomLeft==null && TopLeft==null && TopLeft==null) {
						if (Tag==null) {
							return BlockState.Free;
						} else {
							return BlockState.Allocated;
						}
					} else {
						if (Tag==null) {
							return BlockState.Split;
						} else {
							throw new InvalidOperationException("Bad block state");
						}
					}
				}
			}


			
			public Block ( Int2 address, int size, Block parent, string tag )
			{
				Address	=	address;
				Size	=	size;
				Tag		=	null;
				Parent	=	parent;
			}



			public Block Split ()
			{
				if (State!=BlockState.Free) {
					throw new InvalidOperationException(string.Format("{0} block could not be split", State));
				}

				var addr	=	Address;
				var size	=	Size / 2;

				TopLeft		=	new Block( new Int2( addr.X,		addr.Y			), size, this, null );
				TopRight	=	new Block( new Int2( addr.X + size, addr.Y			), size, this, null );
				BottomLeft	=	new Block( new Int2( addr.X,		addr.Y + size	), size, this, null );
				BottomRight	=	new Block( new Int2( addr.X + size, addr.Y + size	), size, this, null );

				return TopLeft;
			}



			public void FreeAndMerge ()
			{
				if (State==BlockState.Allocated) {

					Tag = null;

					var parent = Parent;

					while (parent!=null && parent.TryMerge()) {
						parent = parent.Parent;
					}

				} else {
					throw new InvalidOperationException(string.Format("Can not free {0} block", State));
				}
			}



			public bool TryMerge ()
			{
				if (State==BlockState.Split) {
					if (   TopLeft.State==BlockState.Free 
						&& TopRight.State==BlockState.Free 
						&& BottomLeft.State==BlockState.Free 
						&& BottomRight.State==BlockState.Free ) {

						TopLeft = null;
						TopRight = null;
						BottomLeft = null;
						BottomRight = null;

						return true;
					} else {
						return false;
					}
				} else {
					return false;
				}
			}



			public bool IsAddressInside ( Int2 address )
			{
				return ( address.X >= Address.X )
					&& ( address.Y >= Address.Y )
					&& ( address.X < Address.X + Size )
					&& ( address.Y < Address.Y + Size );
			}



		}
	}
}
