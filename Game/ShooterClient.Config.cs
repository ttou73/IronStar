using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Engine.Input;
using Fusion.Core.Configuration;

namespace ShooterDemo {
	public partial class ShooterClient : Fusion.Engine.Client.GameClient {

		[Config] public float Sensitivity { get; set; }
		[Config] public bool InvertMouse { get; set; }
		[Config] public float PullFactor { get; set; }
		[Config] public bool ThirdPerson { get; set; }
		
		[Config] public float ZoomFov { get; set; }
		[Config] public float Fov { get; set; }
		[Config] public float BobHeave	{ get; set; }
		[Config] public float BobPitch	{ get; set; }
		[Config] public float BobRoll	{ get; set; }
		[Config] public float BobStrafe  { get; set; }
		[Config] public float BobJump	{ get; set; }
		[Config] public float BobLand	{ get; set; }

		[Config] public Keys	MoveForward		{ get; set; }
		[Config] public Keys	MoveBackward	{ get; set; }
		[Config] public Keys	StrafeRight		{ get; set; }
		[Config] public Keys	StrafeLeft		{ get; set; }
		[Config] public Keys	Jump			{ get; set; }
		[Config] public Keys Crouch			{ get; set; }
		[Config] public Keys Walk			{ get; set; }

		[Config] public Keys Attack			{ get; set; }
		[Config] public Keys Zoom			{ get; set; }
		[Config] public Keys ThrowGrenade	{ get; set; }

		[Config] public Keys UseWeapon1		{ get; set; }
		[Config] public Keys UseWeapon2		{ get; set; }
		[Config] public Keys UseWeapon3		{ get; set; }
		[Config] public Keys UseWeapon4		{ get; set; }
		[Config] public Keys UseWeapon5		{ get; set; }
		[Config] public Keys UseWeapon6		{ get; set; }
		[Config] public Keys UseWeapon7		{ get; set; }
		[Config] public Keys UseWeapon8		{ get; set; }
		[Config] public Keys UseWeapon9		{ get; set; }

		/// <summary>
		/// 
		/// </summary>
		void SetDefaults ()
		{
			Sensitivity	=	5;
			InvertMouse	=	false;
			PullFactor	=	1;

			Fov			=	90.0f;
			ZoomFov		=	30.0f;

			BobHeave	=	0.05f;
			BobPitch	=	1.0f;
			BobRoll		=	2.0f;
			BobStrafe  	=	5.0f;
			BobJump		=	5.0f;
			BobLand		=	5.0f;


			MoveForward		=	Keys.S;
			MoveBackward	=	Keys.Z;
			StrafeRight		=	Keys.X;
			StrafeLeft		=	Keys.A;
			Jump			=	Keys.RightButton;
			Crouch			=	Keys.LeftAlt;
			Walk			=	Keys.LeftShift;
							
			Attack			=	Keys.LeftButton;
			Zoom			=	Keys.D;
			ThrowGrenade	=	Keys.G;
								
			UseWeapon1		=	Keys.D1;
			UseWeapon2		=	Keys.D2;
			UseWeapon3		=	Keys.D3;
			UseWeapon4		=	Keys.D4;
			UseWeapon5		=	Keys.D5;
			UseWeapon6		=	Keys.D6;
			UseWeapon7		=	Keys.D7;
			UseWeapon8		=	Keys.D8;
			UseWeapon9		=	Keys.D9;
		}
	}
}
