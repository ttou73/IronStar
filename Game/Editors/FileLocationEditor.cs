using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Fusion.Core.Content;
using Fusion.Build;

namespace IronStar.Editors {

	public class FileLocationEditor : UITypeEditor {

		public virtual string Filter {
			get {
				return null;
			}
		}

		public virtual string InitialDirectory {
			get {
				return null;
			}
		}

		//
		public override UITypeEditorEditStyle GetEditStyle( ITypeDescriptorContext context )
		{
			return UITypeEditorEditStyle.Modal;
		}

		public override object EditValue( ITypeDescriptorContext context, IServiceProvider provider, object value )
		{
			using ( OpenFileDialog ofd = new OpenFileDialog() ) {

				ofd.InitialDirectory	= 	Path.Combine(Builder.FullInputDirectory, InitialDirectory??"");
				ofd.RestoreDirectory	=	true;
				ofd.Filter				=	Filter;

				// set file filter info here
				if ( ofd.ShowDialog() == DialogResult.OK ) {

					var fileName = ofd.FileName;

					if (Path.IsPathRooted(fileName)) {
						fileName = ContentUtils.MakeRelativePath( Builder.FullInputDirectory + @"\", fileName );
					}

					return fileName;
				}
			}
			return value;
		}
	}


	public class FbxFileLocationEditor : FileLocationEditor {
		public override string Filter {
			get {
				return "FBX Files (*.fbx)|*.fbx";
			}
		}
	}


	public class ImageFileLocationEditor : FileLocationEditor {
		public override string Filter {
			get {
				return "Image Files (*.tga;*.jpg;*.png)|*.tga;*.jpg;*.png";
			}
		}
	}


	public class SpriteFileLocationEditor : FileLocationEditor {
		public override string Filter {
			get {
				return "TGA Images (*.tga)|*.tga";
			}
		}
		public override string InitialDirectory {
			get {
				return "sprites";
			}
		}

		public override object EditValue( ITypeDescriptorContext context, IServiceProvider provider, object value )
		{
			return Path.GetFileNameWithoutExtension( (string)base.EditValue( context, provider, value ) );
		}
	}


	public class SoundFileLocationEditor : FileLocationEditor {
		public override string Filter {
			get {
				return "WAV Sounds (*.wav)|*.wav";
			}
		}
		public override string InitialDirectory {
			get {
				return "sound";
			}
		}
	}
}
