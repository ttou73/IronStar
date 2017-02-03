using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Core.Extensions;
using IronStar.SFX;

namespace IronStar.Core {
	public class SnapshotWriter {

		int sendSnapshotCounter = 1;

		/// <summary>
		/// 
		/// </summary>
		public SnapshotWriter ()
		{
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="snapshotStream"></param>
		/// <param name="entities"></param>
		/// <param name="fxEvents"></param>
		public void Write<T> ( Stream snapshotStream, ref T header, Dictionary<uint,Entity> entities, List<FXEvent> fxEvents ) where T: struct
		{
			using ( var writer = new BinaryWriter( snapshotStream ) ) {

				writer.Write<T>( header );

				var entityArray = entities.OrderBy( pair => pair.Value.ID ).Select( pair1 => pair1.Value ).ToArray();

				writer.Write( sendSnapshotCounter );
				sendSnapshotCounter++;

				//
				//	Write fat entities :
				//
				writer.WriteFourCC("ENT0");
				writer.Write( entityArray.Length );

				foreach ( var ent in entityArray ) {
					writer.Write( ent.ID );
					ent.Write( writer );
				}


				//
				//	Write FX events :
				//
				writer.WriteFourCC("FXE0");
				writer.Write( fxEvents.Count );
			
				foreach ( var fxe in fxEvents ) {
					fxe.SendCount ++;
					fxe.Write( writer );
				}

				fxEvents.RemoveAll( fx => fx.SendCount >= 3 );
			}
		}
	}
}
