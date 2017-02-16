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
	public partial class Map : IPrecachable {


		/// <summary>
		/// 
		/// </summary>
		public MapEnvironment Environment { get; set; }


		/// <summary>
		/// List of nodes
		/// </summary>
		public List<MapNode> Nodes { get; set; }


		/// <summary>
		/// 
		/// </summary>
		public Map ()
		{
			Nodes		=	new List<MapNode>();
			Environment	=	new MapEnvironment();
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="content"></param>
		public void Precache( ContentManager content )
		{
			//content.Precache<Scene>( ScenePath );
		}


		/// <summary>
		/// 
		/// </summary>
		public void ActivateMap ( GameWorld gameWorld, bool activateEntities )
		{
			UpdateEnvironment( gameWorld );

			foreach ( var node in Nodes ) {
				
				if (node is MapEntity && !activateEntities) {
					continue;
				}

				node.SpawnNode( gameWorld );
			}
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameWorld"></param>
		public void UpdateEnvironment ( GameWorld gameWorld )
		{
			gameWorld.Physics.Gravity			=	Environment.Gravity;

			gameWorld.environment.FogDensity	=	Environment.FogDensity;
			gameWorld.environment.Gravity		=	Environment.Gravity;
			gameWorld.environment.SunIntensity	=	Environment.SunIntensity;
			gameWorld.environment.SunDirection	=	Environment.SunPosition;
			gameWorld.environment.Turbidity		=	Environment.SkyTrubidity;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static Map LoadFromXml ( Stream stream )
		{
			var extraTypes = new List<Type>();
			extraTypes.AddRange( Misc.GetAllSubclassesOf( typeof( MapNode ) ) );
			extraTypes.AddRange( Misc.GetAllSubclassesOf( typeof( EntityFactory ) ) );

			return (Map)Misc.LoadObjectFromXml( typeof( Map ), stream, extraTypes.ToArray() );
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="stream"></param>
		public static void SaveToXml ( Map map, Stream stream )
		{
			var extraTypes = new List<Type>();
			extraTypes.AddRange( Misc.GetAllSubclassesOf( typeof( MapNode ) ) );
			extraTypes.AddRange( Misc.GetAllSubclassesOf( typeof( EntityFactory ) ) );
			
			Misc.SaveObjectToXml( map, typeof( Map ), stream, extraTypes.ToArray() );
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
			return Map.LoadFromXml( stream );
		}
	}
}
