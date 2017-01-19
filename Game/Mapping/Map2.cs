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
	public class Map2 : IPrecachable {

		/// <summary>
		/// 
		/// </summary>
		public Map2 ()
		{
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
		/// <param name="stream"></param>
		/// <returns></returns>
		public static Map2 LoadFromXml ( Stream stream )
		{
			var	extraTypes = Misc.GetAllSubclassesOf( typeof( EntityFactory ) );
			return (Map2)Misc.LoadObjectFromXml( typeof( Map2 ), stream, extraTypes );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="stream"></param>
		public static void SaveToXml ( Map2 map, Stream stream )
		{
			var	extraTypes = Misc.GetAllSubclassesOf( typeof( EntityFactory ) );
			Misc.SaveObjectToXml( map, typeof( Map2 ), stream, extraTypes );
		}
	}


	/// <summary>
	/// Map loader
	/// </summary>
	[ContentLoader( typeof( Map2 ) )]
	public sealed class Map2Loader : ContentLoader {

		static Type[] extraTypes;

		public override object Load( ContentManager content, Stream stream, Type requestedType, string assetPath, IStorage storage )
		{
			if ( extraTypes==null ) {
				extraTypes = Misc.GetAllSubclassesOf( typeof( EntityFactory ) );
			}

			var map = (Map2)Misc.LoadObjectFromXml( typeof( Map2 ), stream, extraTypes );

			return map;
		}
	}
}
