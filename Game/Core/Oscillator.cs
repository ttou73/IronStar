using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShooterDemo.Core {
	class Oscillator {

		public float Offset { get { return offset; } }
		public float Target = 0;

		readonly float damping;
		readonly float stiffness;
		float offset	=	0;
		float velocity	=	0;
		const float mass = 1;

		public Oscillator ( float stiffness, float damping )
		{
			this.stiffness	=	stiffness;
			this.damping	=	damping;
		}

		public void Kick ( float velocity )
		{
			this.velocity	+=	velocity;
		}


		public void Update ( float elapsed )
		{
			float force =	(Target-offset) * stiffness - velocity * damping;
			velocity	=	velocity + (force/mass) * elapsed;
			offset		=	offset + velocity * elapsed;
		}

		public void Stop ()
		{
			offset		=	0;
			Target		=	0;
			velocity	=	0;
		}
	}
}
