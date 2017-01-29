using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Engine.Graphics;
using Fusion.Core.Mathematics;
using Fusion.Engine.Common;
using Fusion;
using IronStar.Mapping;

namespace IronStar.Editor2 {
	static class Utils {

		readonly public static Color SelectColor		=	new Color(255,211,149);
		readonly public static Color GridColor			=	new Color(64,64,64);
		readonly public static Color WireColor			=	new Color(0,4,96);
		readonly public static Color WireColorSelected	=	new Color(67,255,163);

		
		public static Ray TransformRay ( Matrix m, Ray r )
		{
			return new Ray( Vector3.TransformCoordinate( r.Position, m ), Vector3.TransformNormal( r.Direction, m ).Normalized() );
		}




        public static float RayIntersectsRay(ref Ray ray1, ref Ray ray2, out Vector3 point1, out Vector3 point2 )
		{
			float t1, t2;
			return RayIntersectsRay( ref ray1, ref ray2, out point1, out point2, out t1, out t2 );
		}

        public static float RayIntersectsRay(ref Ray ray1, ref Ray ray2, out Vector3 point1, out Vector3 point2, out float t1, out float t2 )
        {
            //Source: Real-Time Rendering, Third Edition
            //Reference: Page 780
			t1 = t2 = 0;

            Vector3 cross;

            Vector3.Cross(ref ray1.Direction, ref ray2.Direction, out cross);
            float denominator = cross.Length();

            //Lines are parallel.
            if (MathUtil.IsZero(denominator))
            {
                //Lines are parallel and on top of each other.
                if (MathUtil.NearEqual(ray2.Position.X, ray1.Position.X) &&
                    MathUtil.NearEqual(ray2.Position.Y, ray1.Position.Y) &&
                    MathUtil.NearEqual(ray2.Position.Z, ray1.Position.Z))
                {
                    point1 = ray1.Position;
                    point2 = ray2.Position;
                    return 0;
                } else {
                    point1 = ray1.Position;
                    point2 = ray2.Position;
					return Vector3.Distance( ray2.Position, ray1.Position );
				}
            }

            denominator = denominator * denominator;

            //3x3 matrix for the first ray.
            float m11 = ray2.Position.X - ray1.Position.X;
            float m12 = ray2.Position.Y - ray1.Position.Y;
            float m13 = ray2.Position.Z - ray1.Position.Z;
            float m21 = ray2.Direction.X;
            float m22 = ray2.Direction.Y;
            float m23 = ray2.Direction.Z;
            float m31 = cross.X;
            float m32 = cross.Y;
            float m33 = cross.Z;

            //Determinant of first matrix.
            float dets =
                m11 * m22 * m33 +
                m12 * m23 * m31 +
                m13 * m21 * m32 -
                m11 * m23 * m32 -
                m12 * m21 * m33 -
                m13 * m22 * m31;

            //3x3 matrix for the second ray.
            m21 = ray1.Direction.X;
            m22 = ray1.Direction.Y;
            m23 = ray1.Direction.Z;

            //Determinant of the second matrix.
            float dett =
                m11 * m22 * m33 +
                m12 * m23 * m31 +
                m13 * m21 * m32 -
                m11 * m23 * m32 -
                m12 * m21 * m33 -
                m13 * m22 * m31;

            //t values of the point of intersection.
            float s = dets / denominator;
            float t = dett / denominator;

			t1 = s;
			t2 = t;

            //The points of intersection.
            point1 = ray1.Position + (s * ray1.Direction);
            point2 = ray2.Position + (t * ray2.Direction);

            //If the points are not equal, no intersection has occurred.
            if (!MathUtil.NearEqual(point2.X, point1.X) ||
                !MathUtil.NearEqual(point2.Y, point1.Y) ||
                !MathUtil.NearEqual(point2.Z, point1.Z))
            {
                return Vector3.Distance( point1, point2 );
            } else {
				return 0;
			}
        }
	}
}
