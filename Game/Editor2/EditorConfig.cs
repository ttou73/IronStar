using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Engine.Graphics;
using Fusion.Core.Mathematics;
using Fusion.Core.Configuration;
using Fusion.Engine.Common;
using Fusion;
using System.ComponentModel;

namespace IronStar.Editor2 {

	public enum AxisMode {
		Global,
		Local,
	}

	public enum SnapMode {
		None,
		Absolute,
		Relative,
	}

	public class EditorConfig {

		[Config]
		[Category("Move Tool")]
		public AxisMode MoveToolAxisMode { get; set; } = AxisMode.Global;

		[Config]
		[Category("Move Tool")]
		public SnapMode MoveToolSnapMode { get; set; } = SnapMode.None;

		[Config]
		[Category("Move Tool")]
		public float MoveToolSnapValue { get; set; } = 1.0f;

		[Config]
		[Category("Rotate Tool")]
		public AxisMode RotateToolAxisMode { get; set; } = AxisMode.Global;

		[Config]
		[Category("Rotate Tool")]
		public SnapMode RotateToolSnapMode { get; set; } = SnapMode.None;

		[Config]
		[Category("Rotate Tool")]
		public float RotateToolSnapValue { get; set; } = 15.0f;
	}
}
