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
using Fusion.Engine.Graphics;
using Fusion.Core.Content;
using Fusion.Engine.Common;
using Fusion.Engine.Storage;
using System.ComponentModel;
using System.Xml.Serialization;
using IronStar.Editor2;

namespace IronStar.Core {

	public abstract class EntityFactory {
		
		public abstract EntityController Spawn (Entity entity, GameWorld world);

		/// <summary>
		/// Draws entity in editor
		/// </summary>
		/// <param name="dr"></param>
		/// <param name="transform"></param>
		/// <param name="color"></param>
		public virtual void Draw ( DebugRender dr, Matrix transform, Color color )
		{
			dr.DrawBox(	MapEditor.DefaultBox, transform, color );
		}


		public override string ToString()
		{
			return GetType().Name;
		}



		public virtual EntityFactory Duplicate ()
		{	
			return (EntityFactory)MemberwiseClone();
		}


		static Type[] factories = null;

		public static Type[] GetFactoryTypes ()
		{
			if (factories==null) {
				factories = Misc.GetAllSubclassesOf( typeof(EntityFactory) );
			}
			return factories;
		}
	}



	[ContentLoader( typeof( EntityFactory ) )]
	public sealed class EntityFactoryLoader : ContentLoader {

		static Type[] extraTypes;

		public override object Load( ContentManager content, Stream stream, Type requestedType, string assetPath, IStorage storage )
		{
			if (extraTypes==null) {
				extraTypes = Misc.GetAllSubclassesOf( typeof(EntityFactory) );
			}

			return Misc.LoadObjectFromXml( typeof(EntityFactory), stream, extraTypes );
		}
	}
}
