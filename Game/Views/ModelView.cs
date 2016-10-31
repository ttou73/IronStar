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
using ShooterDemo.Core;


namespace ShooterDemo.Views {
	public class ModelView : EntityView {

		readonly public string scenePath;
		readonly public string nodeName;
		readonly public Matrix preTransform;
		readonly public Matrix postTransform;
		public MeshInstance meshInstance;


		/// <summary>
		/// 
		/// </summary>
		/// <param name="game"></param>
		public ModelView ( Entity entity, World world, string scenePath, string nodeName, Matrix preTransform, Matrix postTransform ) 
		 : base(entity,world)
		{
			this.scenePath		=	scenePath;
			this.nodeName		=	nodeName;
			this.preTransform	=	preTransform;
			this.postTransform	=	postTransform;

			Reload ( Game.RenderSystem, World.Content );

			Game.Reloading += Game_Reloading;
		}



		void Game_Reloading ( object sender, EventArgs e )
		{
			Reload( Game.RenderSystem, World.Content );
		}



		void Reload ( RenderSystem rs, ContentManager content )
		{
			if (meshInstance!=null) {
				if (!rs.RenderWorld.Instances.Remove( meshInstance )) {
					Log.Warning("Failed to remove {0}|{1}", scenePath, nodeName );
				}
			}

			var scene = content.Load<Scene>( scenePath, (Scene)null );

			if (scene==null) {
				return;
			}

			var node  = scene.Nodes.FirstOrDefault( n => n.Name == nodeName );

			if (node==null) {
				Log.Warning("Scene '{0}' does not contain node '{1}'", scenePath, nodeName );
				return;
			}

			if (node.MeshIndex<0) {
				Log.Warning("Node '{0}|{1}' does not contain mesh", scenePath, nodeName );
				return;
			}

			var mesh		=	scene.Meshes[node.MeshIndex];

			meshInstance		= new MeshInstance( rs, scene, mesh );

			rs.RenderWorld.Instances.Add( meshInstance );
		}




		/// <summary>
		/// Updates visible meshes
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update ( float elapsedTime, float lerpFactor )
		{
			meshInstance.World		=	preTransform * Entity.GetWorldMatrix(lerpFactor) * postTransform;
			meshInstance.Visible	=	Entity.UserGuid != World.GameClient.Guid;
		}



		/// <summary>
		/// Removes entity
		/// </summary>
		/// <param name="id"></param>
		public override void Killed ()
		{	
			Game.RenderSystem.RenderWorld.Instances.Remove( meshInstance );
			Game.Reloading -= Game_Reloading;
		}

	}
}
