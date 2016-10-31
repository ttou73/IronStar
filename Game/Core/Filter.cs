using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShooterDemo.Core {
	class Filter {

		readonly int frameSize;
		readonly Queue<float> values;


		/// <summary>
		/// 
		/// </summary>
		/// <param name="frameSize"></param>
		public Filter ( int frameSize )
		{
			this.frameSize	=	frameSize;
			this.values		=	new Queue<float>( frameSize + 1 );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		public void Push ( float value )
		{
			values.Enqueue(value);
			while ( values.Count>frameSize ) {
				values.Dequeue();
			}
		}



		/// <summary>
		/// Gets filtered value
		/// </summary>
		public float Value {
			get {
				return values.Average();
			}
		}

	}
}
