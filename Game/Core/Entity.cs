using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion;
using Fusion.Core;
using Fusion.Core.Utils;
using Fusion.Core.Mathematics;
using Fusion.Core.Extensions;
using System.IO;
using IronStar.SFX;
using IronStar.SFX;
using Fusion.Engine.Graphics;
using Fusion.Core.Content;
using Fusion.Engine.Common;

namespace IronStar.Core {
	public class Entity {


		/// <summary>
		/// Entity ID
		/// </summary>
		public readonly uint ID;

		/// <summary>
		/// Players guid. Zero if no player.
		/// </summary>
		public Guid UserGuid;// { get; private set; }

		/// <summary>
		/// Gets entity state
		/// </summary>
		public EntityState State;

		/// <summary>
		///	Gets parent's ID. 
		///	Zero value means no parent.
		/// </summary>
		public uint ParentID { get; private set; }

		/// <summary>
		/// Classname atoms.
		/// </summary>
		public short ClassID;

		/// <summary>
		/// Teleportation counter.
		/// Used to prevent interpolation in discreete movement.
		/// </summary>
		public byte TeleportCount;

		/// <summary>
		/// Entity position
		/// </summary>
		public Vector3 Position;

		/// <summary>
		/// Entity position
		/// </summary>
		public Vector3 PositionOld;

		/// <summary>
		/// Entity's angle
		/// </summary>
		public Quaternion Rotation;

		/// <summary>
		/// Entity's angle
		/// </summary>
		public Quaternion RotationOld;

		/// <summary>
		/// Control flags.
		/// </summary>
		public UserCtrlFlags UserCtrlFlags;

		/// <summary>
		/// Linear object velocity
		/// </summary>
		public Vector3 LinearVelocity;

		/// <summary>
		/// Angular object velocity
		/// </summary>
		public Vector3 AngularVelocity;


		/// <summary>
		/// Inventory
		/// </summary>
		readonly short[] inventory = new short[(byte)Inventory.Max];

		/// <summary>
		/// Currently active item.
		/// </summary>
		public Inventory ActiveItem;


		/// <summary>
		/// Visible model
		/// </summary>
		public short Model {
			get { return model; }
			set { model = value; modelDirty = true; }
		}
		private short model = -1;
		private bool modelDirty = true;

		/// <summary>
		/// Visible special effect
		/// </summary>
		public short Sfx {
			get { return sfx; }
			set { 
				sfxDirty = sfx != value; 
				sfx = value; 
			}
		}
		private short sfx = -1;
		private bool sfxDirty = true;


		/// <summary>
		/// 
		/// </summary>
		public FXInstance FXInstance { get; private set; }


		/// <summary>
		/// 
		/// </summary>
		public MeshInstance MeshInstance { get; private set; }


		/// <summary>
		/// Used to replicate entity on client side.
		/// </summary>
		/// <param name="id"></param>
		public Entity ( uint id )
		{
			ID	=	id;
			RotationOld		=	Quaternion.Identity;
			Rotation		=	Quaternion.Identity;
			TeleportCount	=	0xFF;
		}


		/// <summary>
		/// Used to spawn entity on server side.
		/// </summary>
		/// <param name="id"></param>
		public Entity ( uint id, short classId, uint parentId, Vector3 position, Quaternion rotation )
		{
			ClassID		=	classId;
			this.ID		=	id;

			TeleportCount	=	0;

			RotationOld		=	Quaternion.Identity;
			PositionOld		=	Vector3.Zero;

			UserGuid		=	new Guid();
			ParentID		=	parentId;

			Rotation		=	rotation;
			UserCtrlFlags	=	UserCtrlFlags.None;
			Position		=	position;
			PositionOld		=	position;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="fxPlayback"></param>
		public void UpdateRenderState ( FXPlayback fxPlayback, AtomCollection atoms, RenderSystem rs, ContentManager content )
		{
			if (sfxDirty) {
				sfxDirty = false;

				FXInstance?.Kill();
				FXInstance = null;

				if (sfx>=0) {
					var fxe = new FXEvent( sfx, ID, Position, LinearVelocity, Rotation );
					FXInstance = fxPlayback.RunFX( fxe );
				}
			}

			if (modelDirty) {
				modelDirty = false;

				try {
					rs.RenderWorld.Instances.Remove( MeshInstance );
					MeshInstance = null;

					MeshInstance	=	MeshInstance.FromScene( rs, content, atoms[model] );
				} catch ( Exception e ) {
					Log.Warning("{0}", e.Message );
					MeshInstance = null;
				}
			}
		}



		public void DestroyRenderState ( FXPlayback fxPlayback )
		{
			FXInstance?.Kill();
			FXInstance = null;
		}



		/// <summary>
		/// sets item count 
		/// </summary>
		/// <param name="item"></param>
		/// <param name="count"></param>
		public void SetItemCount ( Inventory item, short count ) 
		{
			if (count<0 && item!=Inventory.Health) {
				//	only health could be negative
				Log.Warning("SetItemCount: count of {0} < 0. Forced zero.", item);
				count = 0;
			}
			inventory[(byte)item] = count;
		}


		/// <summary>
		/// Gets item count
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public short GetItemCount ( Inventory item ) 
		{
			return inventory[(byte)item];
		}


		/// <summary>
		/// Consumes specified amount of items if inventory containg enough.
		/// Returns True if consumption was successfull. False otherwice.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="amount"></param>
		/// <returns></returns>
		public bool ConsumeItem ( Inventory item, short amount )
		{
			if (inventory[(byte)item] >= amount) {
				inventory[(byte)item] -= amount;
				return true;
			} else {
				return false;
			}
		}





		/// <summary>
		/// Immediatly put entity in given position without interpolation :
		/// </summary>
		/// <param name="position"></param>
		/// <param name="orient"></param>
		void SetPose ( Vector3 position, Quaternion orient )
		{
			TeleportCount++;
			TeleportCount &= 0x7F;

			Position		=	position;
			Rotation		=	orient;
			PositionOld		=	position;
			RotationOld		=	orient;
		}



		/// <summary>
		/// Moves entity to given position with interpolation :
		/// </summary>
		/// <param name="position"></param>
		/// <param name="orient"></param>
		public void Move ( Vector3 position, Quaternion orient, Vector3 velocity )
		{
			Position		=	position;
			Rotation		=	orient;
			LinearVelocity	=	velocity;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		public void Write ( BinaryWriter writer )
		{
			writer.Write( UserGuid.ToByteArray() );

			writer.Write( ParentID );
			writer.Write( (int)State );
			writer.Write( ClassID );

			writer.Write( TeleportCount );

			writer.Write( Position );
			writer.Write( Rotation );
			writer.Write( (int)UserCtrlFlags );
			writer.Write( LinearVelocity );
			writer.Write( AngularVelocity );

			for (int i=0; i<inventory.Length; i++) {
				writer.Write( inventory[i] );
			}

			writer.Write( (byte)ActiveItem );

			writer.Write( Model );
			writer.Write( Sfx );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		public void Read ( BinaryReader reader, float lerpFactor )
		{
			//	keep old teleport counter :
			var oldTeleport	=	TeleportCount;

			//	set old values :
			PositionOld		=	LerpPosition( lerpFactor );
			RotationOld		=	LerpRotation( lerpFactor );

			//	read state :
			UserGuid		=	new Guid( reader.ReadBytes(16) );
								
			ParentID		=	reader.ReadUInt32();
			State			=	(EntityState)reader.ReadInt32();
			ClassID			=	reader.ReadInt16();

			TeleportCount	=	reader.ReadByte();

			Position		=	reader.Read<Vector3>();	
			Rotation		=	reader.Read<Quaternion>();	
			UserCtrlFlags	=	(UserCtrlFlags)reader.ReadInt32();
			LinearVelocity	=	reader.Read<Vector3>();
			AngularVelocity	=	reader.Read<Vector3>();	


			for (int i=0; i<inventory.Length; i++) {
				inventory[i]	=	reader.ReadInt16();
			}

			ActiveItem	=	(Inventory)reader.ReadByte();

			Model		=	reader.ReadInt16();
			Sfx			=	reader.ReadInt16();


			//	entity teleported - reset position and rotation :
			if (oldTeleport!=TeleportCount) {
				PositionOld	=	Position;
				RotationOld	=	Rotation;
			}
		}



		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Matrix GetWorldMatrix (float lerpFactor)
		{
			return Matrix.RotationQuaternion( LerpRotation(lerpFactor) ) 
					* Matrix.Translation( LerpPosition(lerpFactor) );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="lerpFactor"></param>
		/// <returns></returns>
		public Quaternion LerpRotation ( float lerpFactor )
		{
			//return Position;
			return Quaternion.Slerp( RotationOld, Rotation, MathUtil.Clamp(lerpFactor,0,1f) );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="lerpFactor"></param>
		/// <returns></returns>
		public Vector3 LerpPosition ( float lerpFactor )
		{
			//return Position;
			return Vector3.Lerp( PositionOld, Position, MathUtil.Clamp(lerpFactor,0,2f) );
		}


		/*-----------------------------------------------------------------------------------------------
		 * 
		 *	Entity controllers :
		 * 
		-----------------------------------------------------------------------------------------------*/
		EntityController controller;

		public EntityController	Controller {
			get {
				return controller;
			}
			set {
				controller = value;
			}
		}



		/// <summary>
		/// Iterates all controllers
		/// </summary>
		/// <param name="action"></param>
		public void ForeachController ( Action<EntityController> action )
		{
			if (controller!=null) {
				action(controller);
			}
		}
	}
}
