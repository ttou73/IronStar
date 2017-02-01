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

namespace IronStar.Core {

	public abstract class EntityFactory {
		
		public abstract EntityController Spawn (Entity entity, GameWorld world);

		[XmlIgnore]
		[Browsable(false)]
		public virtual BoundingBox BoundingBox {
			get {
				return new BoundingBox( Vector3.One * (-0.25f), Vector3.One * 0.25f );
			}
		}

		public override string ToString()
		{
			return GetType().Name;
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
