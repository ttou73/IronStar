﻿using System;
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

namespace IronStar.Core {

	public abstract class EntityFactory {

		[Category("Common")]
		public string Targetname { get; set; }

		public abstract EntityController Spawn (Entity entity, GameWorld world);

		public override string ToString()
		{
			return GetType().Name;
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