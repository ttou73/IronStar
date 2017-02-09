using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Core.Content;
using Fusion.Core.Mathematics;
using Fusion.Engine.Common;
using Fusion.Core.Extensions;
using IronStar.Core;
using IronStar.SFX;
using System.ComponentModel;
using Fusion.Engine.Graphics;


namespace IronStar.Entities {

	public class FuncDecal : EntityController {

		/// <summary>
		/// 
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="world"></param>
		/// <param name="factory"></param>
		public FuncDecal( Entity entity, GameWorld world, FuncDecalFactory factory ) : base( entity, world )
		{
			entity.Decal = world.Atoms[ factory.Decal ];
			entity.LinearVelocity	=	new Vector3( factory.Width, factory.Height, factory.Depth );
		}
	}



	public class FuncDecalFactory : EntityFactory {

		[Category("Decal")]
		[Description("Name of the decal object")]
		public string Decal { get; set; } = "";

		[Category("Decal")]
		public float Width { get; set; } = 0.5f;

		[Category("Decal")]
		public float Height { get; set; } = 0.5f;

		[Category("Decal")]
		public float Depth { get; set; } = 0.25f;


		public override void Draw( DebugRender dr, Matrix transform, Color color )
		{
			var c	= transform.TranslationVector 
					+ transform.Left * Width * 0.40f
					+ transform.Up   * Height * 0.40f;

			float len = Math.Min( Width, Height ) / 6;

			var x  = transform.Right * len;
			var y  = transform.Down * len;

			var p0 = Vector3.TransformCoordinate( new Vector3(  Width/2,  Height/2, 0 ), transform ); 
			var p1 = Vector3.TransformCoordinate( new Vector3( -Width/2,  Height/2, 0 ), transform ); 
			var p2 = Vector3.TransformCoordinate( new Vector3( -Width/2, -Height/2, 0 ), transform ); 
			var p3 = Vector3.TransformCoordinate( new Vector3(  Width/2, -Height/2, 0 ), transform ); 

			var p4 = Vector3.TransformCoordinate( new Vector3( 0, 0,  Depth ), transform ); 
			var p5 = Vector3.TransformCoordinate( new Vector3( 0, 0, -Depth ), transform ); 

			dr.DrawLine( p0, p1, color, color, 1, 1 );
			dr.DrawLine( p1, p2, color, color, 1, 1 );
			dr.DrawLine( p2, p3, color, color, 1, 1 );
			dr.DrawLine( p3, p0, color, color, 1, 1 );

			dr.DrawLine( c, c+x, Color.Red  , Color.Red  , 2, 2 );
			dr.DrawLine( c, c+y, Color.Lime , Color.Lime , 2, 2 );

			dr.DrawLine( p4, p5, color, color, 2, 2 );
		}


		public override EntityController Spawn( Entity entity, GameWorld world )
		{
			return new FuncDecal( entity, world, this );
		}
	}
}
