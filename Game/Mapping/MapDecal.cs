using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Fusion.Core.Mathematics;
using IronStar.Core;
using Fusion.Engine.Graphics;
using IronStar.SFX;
using Fusion.Development;
using System.Drawing.Design;

namespace IronStar.Mapping {
	public class MapDecal : MapNode {


		[Category("Decal Image")]
		[Editor( typeof( DecalFileLocationEditor ), typeof( UITypeEditor ) )]
		public string ImageName { get; set; } = "";

		/// <summary>
		/// 
		/// </summary>
		[Category("Decal Size")]
		public float Width { get; set;} = 0.5f;

		/// <summary>
		/// 
		/// </summary>
		[Category("Decal Size")]
		public float Height { get; set;} = 0.5f;

		/// <summary>
		/// 
		/// </summary>
		[Category("Decal Size")]
		public float Depth { get; set;} = 0.25f;

		/// <summary>
		/// Decal emission intensity
		/// </summary>
		[Category("Decal Material")]
		public Color4 Emission { get; set;} = Color4.Zero;

		/// <summary>
		/// Decal base color
		/// </summary>
		[Category("Decal Material")]
		public Color BaseColor { get; set;} = new Color(128,128,128,255);

		/// <summary>
		/// Decal roughness
		/// </summary>
		[Category("Decal Material")]
		public float Roughness { get; set;}= 0.5f;

		/// <summary>
		/// Decal meatllic
		/// </summary>
		[Category("Decal Material")]
		public float Metallic { get; set;} = 0.5f;

		/// <summary>
		/// Color blend factor [0,1]
		/// </summary>
		[Category("Decal Material")]
		public float ColorFactor { get; set;} = 1.0f;

		/// <summary>
		/// Roughmess and specular blend factor [0,1]
		/// </summary>
		[Category("Decal Material")]
		public float SpecularFactor { get; set;} = 1.0f;

		/// <summary>
		/// Normalmap blend factor [-1,1]
		/// </summary>
		[Category("Decal Material")]
		public float NormalMapFactor { get; set;} = 1.0f;

		/// <summary>
		/// Normalmap blend factor [-1,1]
		/// </summary>
		[Category("Decal Material")]
		public float FalloffFactor { get; set;} = 0.5f;
		

		Decal decal = null;


		/// <summary>
		/// 
		/// </summary>
		public MapDecal ()
		{
		}



		public override void SpawnNode( GameWorld world )
		{
			if (!world.IsPresentationEnabled) {
				return;
			}

			if ( string.IsNullOrWhiteSpace(ImageName)) {
				return;
			}

			decal	=	new Decal();

			var rw	=	world.Game.RenderSystem.RenderWorld;
			var ls	=	rw.LightSet;

			decal.DecalMatrix		=	Matrix.Scaling( Width/2, Height/2, Depth/2 ) * WorldMatrix;
			decal.DecalMatrixInverse=	Matrix.Invert( decal.DecalMatrix );
									
			decal.Emission			=	Emission;
			decal.BaseColor			=	new Color4( BaseColor.R/255.0f, BaseColor.G/255.0f, BaseColor.B/255.0f, 1 );
			
			decal.Metallic			=	Metallic;
			decal.Roughness			=	Roughness;
			decal.ImageRectangle	=	ls.DecalAtlas.GetNormalizedRectangleByName( ImageName );

			decal.ColorFactor		=	ColorFactor;
			decal.SpecularFactor	=	SpecularFactor;
			decal.NormalMapFactor	=	NormalMapFactor;
			decal.FalloffFactor		=	FalloffFactor;

			world.Game.RenderSystem.RenderWorld.LightSet.Decals.Add( decal );
		}



		public override void ActivateNode()
		{
		}



		public override void DrawNode( DebugRender dr, Color color, bool selected )
		{
			var transform	=	WorldMatrix;

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



		public override void ResetNode( GameWorld world )
		{
			decal.DecalMatrix		=	Matrix.Scaling( Width/2, Height/2, Depth/2 ) * WorldMatrix;
			decal.DecalMatrixInverse=	Matrix.Invert( decal.DecalMatrix );
		}



		public override void HardResetNode( GameWorld world )
		{
			KillNode( world );
			SpawnNode( world );
		}



		public override void KillNode( GameWorld world )
		{
			world.Game.RenderSystem.RenderWorld.LightSet.Decals.Remove( decal );
		}


		public override MapNode DuplicateNode()
		{
			var newNode = (MapDecal)MemberwiseClone();
			newNode.decal = null;
			return newNode;
		}
	}
}
