using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fusion.Core.Mathematics
{

    [Serializable]
    public struct Segment : IEquatable<Segment>
    {
        /// <summary>
        /// Start point of segment
        /// </summary>
        [XmlAttribute]
        public Vector3 Start;

        /// <summary>
        /// End point of segment
        /// </summary>
        [XmlAttribute]
        public Vector3 End;


        public Segment(Vector3 start, Vector3 end)
        {
            Start = start;
            End = end;
        }

        public bool Equals(Segment other)
        {
            return Start.Equals(other.Start) && End.Equals(other.End);
        }

        public override int GetHashCode()
        {
            return Start.GetHashCode() * 31 + End.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("[ {0} - {1} ]", Start.ToString(), End.ToString());
        }
    }
}
