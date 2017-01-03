using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Reflection;
using Native.Fbx;
using IronStar.Entities;
using Fusion.Core.Content;
using System.IO;
using IronStar.Core;
using Fusion.Engine.Storage;
using Fusion.Core.Extensions;
using Fusion.Engine.Graphics;
using BEPUphysics.BroadPhaseEntries;
using Fusion.Core.Mathematics;
using Fusion;

namespace IronStar.Mapping {
	public class Map {

		/// <summary>
		/// Base scene path
		/// </summary>
		[XmlAttribute]
		public string ScenePath { get; set; }


		/// <summary>
		/// List of nodes
		/// </summary>
		public List<MapFactory> Factories { get; set; }

	
		List<MeshInstance> instances;
		List<StaticMesh> collisionMeshes;
		List<SpawnInfo> spawnInfos;

		/// <summary>
		/// Gets base scene
		/// </summary>
		[XmlIgnore]
		public Scene Scene {
			get {
				return scene;
			}
		}

		Scene scene = null;


		/// <summary>
		/// Gets list of mesh instances.
		/// </summary>
		[XmlIgnore]
		public IEnumerable<MeshInstance> MeshInstance {
			get {
				return instances;
			}
		}


		/// <summary>
		/// Gets list of static collision meshes
		/// </summary>
		[XmlIgnore]
		public IEnumerable<StaticMesh> StaticCollisionMeshes {
			get {
				return collisionMeshes;
			}
		}


		/// <summary>
		/// Gets list of spawn infos
		/// </summary>
		[XmlIgnore]
		public IEnumerable<SpawnInfo> SpawnInfos {
			get {
				return spawnInfos;
			}
		}


		/// <summary>
		/// Spawn info
		/// </summary>
		public class SpawnInfo {
			public SpawnInfo( string classname, Vector3 origin, Quaternion rotation )
			{
				Classname   =   classname;
				Origin      =   origin;
				Rotation    =   rotation;
			}

			public readonly string Classname;
			public readonly Vector3 Origin;
			public readonly Quaternion Rotation;
		}



		/// <summary>
		/// 
		/// </summary>
		public Map ()
		{
		}



		/// <summary>
		/// 
		/// </summary>
		public void ActivateMap ( GameWorld gameWorld )
		{
			var content	=	gameWorld.Content;
			scene		=	content.Load<Scene>( ScenePath );

			instances       =   new List<MeshInstance>();
			collisionMeshes =   new List<StaticMesh>();
			spawnInfos      =   new List<SpawnInfo>();

			//	compute absolute transforms :
			var transforms  = new Matrix[ scene.Nodes.Count ];
			scene.ComputeAbsoluteTransforms( transforms );

			var nodePathMap	=	scene.GetPathNodeMapping();

			//	iterate through the scene's nodes :
			for ( int i = 0; i<scene.Nodes.Count; i++ ) {

				var node    =   scene.Nodes[i];
				var world   =   transforms[i];
				var name    =   node.Name;
				var mesh    =   node.MeshIndex < 0 ? null : scene.Meshes[ node.MeshIndex ];
			}


			foreach ( var factory in Factories ) {

				Node node;

				if (nodePathMap.TryGetValue( factory.NodePath, out node ) ) {

					var modeIndex	=	scene.Nodes.IndexOf( node );
					var meshIndex	=	node.MeshIndex;
					var transform	=	transforms[ modeIndex ];
					var position	=	transform.TranslationVector;
					var rotation	=	Quaternion.RotationMatrix( transform );

					if (factory.Factory is StaticModelFactory) {
						var smf	=	(StaticModelFactory)factory.Factory;

						smf.CreateStaticCollisionModel( gameWorld, scene, node, transform );
						smf.CreateStaticVisibleModel( gameWorld, scene, node, transform );
							
						continue;
					}

					if (factory.Factory is WorldspawnFactory) {
						var wsf =	(WorldspawnFactory)factory.Factory;

						wsf.SetupWorldPhysics( gameWorld );

						continue;
					}

					var entity		=	gameWorld.Spawn( factory.Factory, -1, 0, position, rotation );
					
				} else {
					Log.Warning("Missing referenced node : {0}", factory.NodePath );
				}
			}
		}

	}



	/// <summary>
	/// Map loader
	/// </summary>
	[ContentLoader( typeof( Map ) )]
	public sealed class MapLoader : ContentLoader {

		static Type[] extraTypes;

		public override object Load( ContentManager content, Stream stream, Type requestedType, string assetPath, IStorage storage )
		{
			if ( extraTypes==null ) {
				extraTypes = Misc.GetAllSubclassesOf( typeof( EntityFactory ) );
			}

			var map = (Map)Misc.LoadObjectFromXml( typeof( Map ), stream, extraTypes );

			//	preload scene :
			content.Load<Scene>( map.ScenePath );

			return map;
		}
	}
}
