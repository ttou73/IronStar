using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using Fusion.Core.Mathematics;

namespace ShooterDemo {

	[Flags]
	public enum UserCtrlFlags : int {

		None			=	0,

		Forward			=	1 << 0,
		Backward		=	1 << 1,
		StrafeLeft		=	1 << 2,
		StrafeRight		=	1 << 3,
		Crouch			=	1 << 4,
		Jump			=	1 << 5,
		Zoom			=	1 << 6,
		Attack			=	1 << 7,

		/// <summary>
		/// To clear weapon activation request.
		/// </summary>
		AllWeapon		=	0x7FFF0000,

		Machinegun		=	1 << 16,
		Shotgun			=	1 << 17,
		SuperShotgun	=	1 << 18,
		GrenadeLauncher	=	1 << 19,
		RocketLauncher	=	1 << 20,
		HyperBlaster	=	1 << 21,
		Chaingun		=	1 << 22,
		Railgun			=	1 << 23,
		BFG				=	1 << 24,
	}	

	
	/// <summary>
	/// Represents an instant user action and intention.
	/// </summary>
	public struct UserCommand {

		/// <summary>
		/// Current user yaw.
		/// </summary>
		public float Yaw;

		/// <summary>
		/// Current user pitch.
		/// </summary>
		public float Pitch;

		/// <summary>
		/// Current user roll.
		/// </summary>
		public float Roll;

		/// <summary>
		/// Set of user control flags.
		/// </summary>
		public UserCtrlFlags CtrlFlags;

		
		/// <summary>
		/// Gets user command's bytes.
		/// </summary>
		/// <param name="userCmd"></param>
		/// <returns></returns>
		static public byte[] GetBytes(UserCommand userCmd) 
		{
			int size = Marshal.SizeOf(userCmd);
			byte[] array = new byte[size];

			IntPtr ptr = Marshal.AllocHGlobal(size);
			Marshal.StructureToPtr(userCmd, ptr, true);
			Marshal.Copy(ptr, array, 0, size);
			Marshal.FreeHGlobal(ptr);
			return array;
		}


		/// <summary>
		/// Gets user command from bytes
		/// </summary>
		/// <param name="array"></param>
		/// <returns></returns>
		static public UserCommand FromBytes(byte[] array) 
		{
			var userCmd = new UserCommand();

			int size = Marshal.SizeOf(userCmd);
			IntPtr ptr = Marshal.AllocHGlobal(size);

			Marshal.Copy(array, 0, ptr, size);

			userCmd = (UserCommand)Marshal.PtrToStructure(ptr, userCmd.GetType());
			Marshal.FreeHGlobal(ptr);

			return userCmd;
		}


		public override string ToString ()
		{
			return string.Format("Angles:[{0} {1} {2}] Ctrl:[{3}]", Yaw, Pitch, Roll, CtrlFlags );
		}
	}
}
