using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Fusion;
using Fusion.Core;
using Fusion.Core.Content;
using Fusion.Core.Mathematics;
using Fusion.Engine.Common;
using Fusion.Engine.Input;
using Fusion.Engine.Client;
using Fusion.Engine.Server;
using Fusion.Engine.Graphics;
using IronStar.Core;
using Fusion.Engine.Audio;

namespace IronStar.SFX {
	public class DecalInstance {

		readonly Matrix preTransform;
		readonly DecalManager decalManager;
		readonly Entity entity;

		Decal decal;

		public bool Killed {
			get; private set;
		}						  


		public Decal Decal {
			get {
				return decal;
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="modelManager"></param>
		/// <param name="descriptor"></param>
		/// <param name="scene"></param>
		/// <param name="entity"></param>
		/// <param name="matrix"></param>
		public DecalInstance ( DecalManager decalManager, DecalFactory factory, Entity entity )
		{
			this.decalManager   =   decalManager;
			this.entity			=	entity;

			decal				=	new Decal();

			var scale			=	entity.LinearVelocity;

			decal.DecalMatrix		=	entity.GetWorldMatrix(1) * Matrix.Scaling( scale.X, scale.Y, scale.Z );
			
			decal.Emission			=	factory.Emission;
			decal.BaseColor			=	new Color4( factory.BaseColor.R/255.0f, factory.BaseColor.G/255.0f, factory.BaseColor.B/255.0f, 1 );
			
			decal.Metallic			=	factory.Metallic;
			decal.Roughness			=	factory.Roughness;
			decal.ImageRectangle	=	decalManager.GetImageRectangleByName( factory.ImageName );

			decal.ColorFactor		=	factory.ColorFactor;
			decal.SpecularFactor	=	factory.SpecularFactor;
			decal.NormalMapFactor	=	factory.NormalMapFactor;
			decal.FalloffFactor		=	0.5f;

			decalManager.rw.LightSet.Decals.Add( decal );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="lerpFactor"></param>
		public void Update ( float dt, float lerpFactor )
		{
			var scale			=	entity.LinearVelocity;
			decal.DecalMatrix	=	entity.GetWorldMatrix(1) * Matrix.Scaling( scale.X, scale.Y, scale.Z );
		}



		/// <summary>
		/// Marks current decal instance to remove.
		/// </summary>
		public void Kill ()
		{
			decalManager.rw.LightSet.Decals.Remove( decal );
			
			Killed = true;
		}


	}
}
