using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Fusion;
using Fusion.Core.Mathematics;
using Fusion.Engine.Common;
using Fusion.Core.Content;
using Fusion.Engine.Server;
using Fusion.Engine.Client;
using Fusion.Core.Extensions;
using IronStar.SFX;

namespace IronStar.Core {

	/// <summary>
	/// World represents entire game state.
	/// </summary>
	public partial class GameWorld {

		int sendSnapshotCounter = 1;


		/// <summary>
		/// Writes world state to stream writer.
		/// </summary>
		/// <param name="writer"></param>
		public virtual void WriteToSnapshot ( BinaryWriter writer )
		{
			var entArray = entities.OrderBy( pair => pair.Key ).ToArray();

			writer.Write( sendSnapshotCounter );
			sendSnapshotCounter++;

			//
			//	Write fat entities :
			//
			writer.WriteFourCC("ENT0");
			writer.Write( entArray.Length );

			foreach ( var ent in entArray ) {
				writer.Write( ent.Key );
				ent.Value.Write( writer );
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



		int recvSnapshotCounter = 0;

		/// <summary>
		/// Reads world state from stream reader.
		/// </summary>
		/// <param name="writer"></param>
		public virtual void ReadFromSnapshot ( BinaryReader reader, float lerpFactor )
		{
			int snapshotCounter			=	reader.ReadInt32();
			int snapshotCountrerDelta	=	snapshotCounter - recvSnapshotCounter;
			recvSnapshotCounter			=	snapshotCounter;

			reader.ExpectFourCC("ENT0", "Bad snapshot");

			int length	=	reader.ReadInt32();
			var oldIDs	=	entities.Select( pair => pair.Key ).ToArray();
			var newIDs	=	new uint[length];

			for ( int i=0; i<length; i++ ) {

				uint id		=	reader.ReadUInt32();
				newIDs[i]	=	id;

				if ( entities.ContainsKey(id) ) {

					//	Entity with given ID exists.
					//	Just update internal state.
					entities[id].Read( reader, lerpFactor );

				} else {
					
					//	Entity does not exist.
					//	Create new one.
					var ent = new Entity(id);

					ent.Read( reader, lerpFactor );
					entities.Add( id, ent );

					//ConstructEntity( ent );

					ReplicaSpawned?.Invoke( this, new EntityEventArgs( ent ) );
				}
			}

			//	Kill all stale entities :
			var staleIDs = oldIDs.Except( newIDs );

			foreach ( var id in staleIDs ) {
				KillImmediatly( id );
			}


			reader.ExpectFourCC("FXE0", "Bad snapshot");

			int count = reader.ReadInt32();

			for (int i=0; i<count; i++) {
				var fxe = new FXEvent();
				fxe.Read( reader );

				if (fxe.SendCount<=snapshotCountrerDelta) {
					RunFX( fxe );
				}
			}
		}
	}
}
