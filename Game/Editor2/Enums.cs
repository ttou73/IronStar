using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronStar.Editor2 {

	public enum Manipulation {
		None, 
		Translating, 
		Rotating, 
		Zooming,
	}


	public enum ManipulatorMode {
		None,
		TranslationLocal,
		TranslationGlobal,
		RotationLocal,
		RotationGlobal,
		Scaling,
	}
}
