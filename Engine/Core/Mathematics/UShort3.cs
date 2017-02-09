// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Globalization;
using System.Runtime.InteropServices;


namespace Fusion.Core.Mathematics
{
    /// <summary>
    /// Represents a three dimensional mathematical ushort vector.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct UShort3 : IEquatable<UShort3>, IFormattable
    {
        /// <summary>
        /// The size of the <see cref = "UShort3" /> type, in bytes.
        /// </summary>
        public static readonly int SizeInBytes = Marshal.SizeOf(typeof (UShort3));

        /// <summary>
        /// A <see cref = "UShort3" /> with all of its components set to zero.
        /// </summary>
        public static readonly UShort3 Zero = new UShort3();

        /// <summary>
        /// The X unit <see cref = "UShort3" /> (1, 0, 0).
        /// </summary>
        public static readonly UShort3 UnitX = new UShort3(1, 0, 0);

        /// <summary>
        /// The Y unit <see cref = "UShort3" /> (0, 1, 0).
        /// </summary>
        public static readonly UShort3 UnitY = new UShort3(0, 1, 0);

        /// <summary>
        /// The Z unit <see cref = "UShort3" /> (0, 0, 1).
        /// </summary>
        public static readonly UShort3 UnitZ = new UShort3(0, 0, 1);

        /// <summary>
        /// A <see cref = "UShort3" /> with all of its components set to one.
        /// </summary>
        public static readonly UShort3 One = new UShort3(1, 1, 1);

        /// <summary>
        /// The X component of the vector.
        /// </summary>
        public ushort X;

        /// <summary>
        /// The Y component of the vector.
        /// </summary>
        public ushort Y;

        /// <summary>
        /// The Z component of the vector.
        /// </summary>
        public ushort Z;

        /// <summary>
        /// Initializes a new instance of the <see cref = "UShort3" /> struct.
        /// </summary>
        /// <param name = "value">The value that will be assigned to all components.</param>
        public UShort3(ushort value)
        {
            
            X = value;
            Y = value;
            Z = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "UShort3" /> struct.
        /// </summary>
        /// <param name = "x">Initial value for the X component of the vector.</param>
        /// <param name = "y">Initial value for the Y component of the vector.</param>
        /// <param name = "z">Initial value for the Z component of the vector.</param>
        public UShort3(ushort x, ushort y, ushort z)
        {
            X = x;
            Y = y;
            Z = z;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref = "UShort3" /> struct.
        /// </summary>
        /// <param name = "values">The values to assign to the X, Y, Z, and W components of the vector. This must be an array with four elements.</param>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "values" /> is <c>null</c>.</exception>
        /// <exception cref = "ArgumentOutOfRangeException">Thrown when <paramref name = "values" /> contains more or less than four elements.</exception>
        public UShort3(ushort[] values)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            if (values.Length != 3)
                throw new ArgumentOutOfRangeException("values",
                                                      "There must be three and only three input values for Ushort3.");

            X = values[0];
            Y = values[1];
            Z = values[2];
        }

        /// <summary>
        /// Gets or sets the component at the specified index.
        /// </summary>
        /// <value>The value of the X, Y, Z, or W component, depending on the index.</value>
        /// <param name = "index">The index of the component to access. Use 0 for the X component, 1 for the Y component, 2 for the Z component, and 3 for the W component.</param>
        /// <returns>The value of the component at the specified index.</returns>
        /// <exception cref = "System.ArgumentOutOfRangeException">Thrown when the <paramref name = "index" /> is out of the range [0, 3].</exception>
        public ushort this[ushort index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return X;
                    case 1:
                        return Y;
                    case 2:
                        return Z;
                }

                throw new ArgumentOutOfRangeException("index", "Indices for Ushort3 run from 0 to 2, inclusive.");
            }

            set
            {
                switch (index)
                {
                    case 0:
                        X = value;
                        break;
                    case 1:
                        Y = value;
                        break;
                    case 2:
                        Z = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("index", "Indices for Ushort3 run from 0 to 2, inclusive.");
                }
            }
        }

        /// <summary>
        /// Creates an array containing the elements of the vector.
        /// </summary>
        /// <returns>A four-element array containing the components of the vector.</returns>
        public ushort[] ToArray()
        {
            return new ushort[] {X, Y, Z};
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name = "left">The first vector to add.</param>
        /// <param name = "right">The second vector to add.</param>
        /// <param name = "result">When the method completes, contains the sum of the two vectors.</param>
        public static void Add(ref UShort3 left, ref UShort3 right, out UShort3 result)
        {
            result = new UShort3((ushort)(left.X + right.X), (ushort)(left.Y + right.Y), (ushort)(left.Z + right.Z));
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name = "left">The first vector to add.</param>
        /// <param name = "right">The second vector to add.</param>
        /// <returns>The sum of the two vectors.</returns>
        public static UShort3 Add(UShort3 left, UShort3 right)
        {
            return new UShort3((ushort)(left.X + right.X), (ushort)(left.Y + right.Y), (ushort)(left.Z + right.Z));
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name = "left">The first vector to subtract.</param>
        /// <param name = "right">The second vector to subtract.</param>
        /// <param name = "result">When the method completes, contains the difference of the two vectors.</param>
        public static void Subtract(ref UShort3 left, ref UShort3 right, out UShort3 result)
        {
            result = new UShort3((ushort)(left.X - right.X), (ushort)(left.Y - right.Y), (ushort)(left.Z - right.Z) );
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name = "left">The first vector to subtract.</param>
        /// <param name = "right">The second vector to subtract.</param>
        /// <returns>The difference of the two vectors.</returns>
        public static UShort3 Subtract(UShort3 left, UShort3 right)
        {
            return new UShort3((ushort)(left.X - right.X), (ushort)(left.Y - right.Y), (ushort)(left.Z - right.Z));
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name = "value">The vector to scale.</param>
        /// <param name = "scale">The amount by which to scale the vector.</param>
        /// <param name = "result">When the method completes, contains the scaled vector.</param>
        public static void Multiply(ref UShort3 value, ushort scale, out UShort3 result)
        {
            result = new UShort3((ushort)(value.X*scale), (ushort)(value.Y*scale), (ushort)(value.Z*scale));
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name = "value">The vector to scale.</param>
        /// <param name = "scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static UShort3 Multiply(UShort3 value, ushort scale)
        {
            return new UShort3((ushort)(value.X * scale), (ushort)(value.Y * scale), (ushort)(value.Z * scale));
        }

        /// <summary>
        /// Modulates a vector with another by performing component-wise multiplication.
        /// </summary>
        /// <param name = "left">The first vector to modulate.</param>
        /// <param name = "right">The second vector to modulate.</param>
        /// <param name = "result">When the method completes, contains the modulated vector.</param>
        public static void Modulate(ref UShort3 left, ref UShort3 right, out UShort3 result)
        {
            result = new UShort3((ushort)(left.X*right.X), (ushort)(left.Y*right.Y), (ushort)(left.Z*right.Z));
        }

        /// <summary>
        /// Modulates a vector with another by performing component-wise multiplication.
        /// </summary>
        /// <param name = "left">The first vector to modulate.</param>
        /// <param name = "right">The second vector to modulate.</param>
        /// <returns>The modulated vector.</returns>
        public static UShort3 Modulate(UShort3 left, UShort3 right)
        {
            return new UShort3((ushort)(left.X*right.X), (ushort)(left.Y*right.Y), (ushort)(left.Z*right.Z));
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name = "value">The vector to scale.</param>
        /// <param name = "scale">The amount by which to scale the vector.</param>
        /// <param name = "result">When the method completes, contains the scaled vector.</param>
        public static void Divide(ref UShort3 value, ushort scale, out UShort3 result)
        {
            result = new UShort3((ushort)(value.X/scale), (ushort)(value.Y/scale), (ushort)(value.Z/scale));
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name = "value">The vector to scale.</param>
        /// <param name = "scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static UShort3 Divide(UShort3 value, ushort scale)
        {
            return new UShort3((ushort)(value.X/scale), (ushort)(value.Y/scale), (ushort)(value.Z/scale));
        }

        /// <summary>
        /// Reverses the direction of a given vector.
        /// </summary>
        /// <param name = "value">The vector to negate.</param>
        /// <param name = "result">When the method completes, contains a vector facing in the opposite direction.</param>
        public static void Negate(ref UShort3 value, out UShort3 result)
        {
            result = new UShort3((ushort)(-value.X), (ushort)(-value.Y), (ushort)(-value.Z));
        }

        /// <summary>
        /// Reverses the direction of a given vector.
        /// </summary>
        /// <param name = "value">The vector to negate.</param>
        /// <returns>A vector facing in the opposite direction.</returns>
        public static UShort3 Negate(UShort3 value)
        {
            return new UShort3((ushort)(-value.X), (ushort)(-value.Y), (ushort)(-value.Z));
        }

        /// <summary>
        /// Restricts a value to be within a specified range.
        /// </summary>
        /// <param name = "value">The value to clamp.</param>
        /// <param name = "min">The minimum value.</param>
        /// <param name = "max">The maximum value.</param>
        /// <param name = "result">When the method completes, contains the clamped value.</param>
        public static void Clamp(ref UShort3 value, ref UShort3 min, ref UShort3 max, out UShort3 result)
        {
            ushort x = value.X;
            x = (x > max.X) ? max.X : x;
            x = (x < min.X) ? min.X : x;

            ushort y = value.Y;
            y = (y > max.Y) ? max.Y : y;
            y = (y < min.Y) ? min.Y : y;

            ushort z = value.Z;
            z = (z > max.Z) ? max.Z : z;
            z = (z < min.Z) ? min.Z : z;

            result = new UShort3(x, y, z);
        }

        /// <summary>
        /// Restricts a value to be within a specified range.
        /// </summary>
        /// <param name = "value">The value to clamp.</param>
        /// <param name = "min">The minimum value.</param>
        /// <param name = "max">The maximum value.</param>
        /// <returns>The clamped value.</returns>
        public static UShort3 Clamp(UShort3 value, UShort3 min, UShort3 max)
        {
            UShort3 result;
            Clamp(ref value, ref min, ref max, out result);
            return result;
        }

        /// <summary>
        /// Returns a vector containing the smallest components of the specified vectors.
        /// </summary>
        /// <param name = "left">The first source vector.</param>
        /// <param name = "right">The second source vector.</param>
        /// <param name = "result">When the method completes, contains an new vector composed of the largest components of the source vectors.</param>
        public static void Max(ref UShort3 left, ref UShort3 right, out UShort3 result)
        {
            result.X = (left.X > right.X) ? left.X : right.X;
            result.Y = (left.Y > right.Y) ? left.Y : right.Y;
            result.Z = (left.Z > right.Z) ? left.Z : right.Z;
        }

        /// <summary>
        /// Returns a vector containing the largest components of the specified vectors.
        /// </summary>
        /// <param name = "left">The first source vector.</param>
        /// <param name = "right">The second source vector.</param>
        /// <returns>A vector containing the largest components of the source vectors.</returns>
        public static UShort3 Max(UShort3 left, UShort3 right)
        {
            UShort3 result;
            Max(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Returns a vector containing the smallest components of the specified vectors.
        /// </summary>
        /// <param name = "left">The first source vector.</param>
        /// <param name = "right">The second source vector.</param>
        /// <param name = "result">When the method completes, contains an new vector composed of the smallest components of the source vectors.</param>
        public static void Min(ref UShort3 left, ref UShort3 right, out UShort3 result)
        {
            result.X = (left.X < right.X) ? left.X : right.X;
            result.Y = (left.Y < right.Y) ? left.Y : right.Y;
            result.Z = (left.Z < right.Z) ? left.Z : right.Z;
        }

        /// <summary>
        /// Returns a vector containing the smallest components of the specified vectors.
        /// </summary>
        /// <param name = "left">The first source vector.</param>
        /// <param name = "right">The second source vector.</param>
        /// <returns>A vector containing the smallest components of the source vectors.</returns>
        public static UShort3 Min(UShort3 left, UShort3 right)
        {
            UShort3 result;
            Min(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name = "left">The first vector to add.</param>
        /// <param name = "right">The second vector to add.</param>
        /// <returns>The sum of the two vectors.</returns>
        public static UShort3 operator +(UShort3 left, UShort3 right)
        {
            return new UShort3((ushort)(left.X + right.X), (ushort)(left.Y + right.Y), (ushort)(left.Z + right.Z));
        }

        /// <summary>
        /// Assert a vector (return it unchanged).
        /// </summary>
        /// <param name = "value">The vector to assert (unchanged).</param>
        /// <returns>The asserted (unchanged) vector.</returns>
        public static UShort3 operator +(UShort3 value)
        {
            return value;
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name = "left">The first vector to subtract.</param>
        /// <param name = "right">The second vector to subtract.</param>
        /// <returns>The difference of the two vectors.</returns>
        public static UShort3 operator -(UShort3 left, UShort3 right)
        {
            return new UShort3((ushort)(left.X - right.X), (ushort)(left.Y - right.Y), (ushort)(left.Z - right.Z));
        }

        /// <summary>
        /// Reverses the direction of a given vector.
        /// </summary>
        /// <param name = "value">The vector to negate.</param>
        /// <returns>A vector facing in the opposite direction.</returns>
        public static UShort3 operator -(UShort3 value)
        {
            return new UShort3((ushort)(-value.X), (ushort)(-value.Y), (ushort)(-value.Z));
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name = "value">The vector to scale.</param>
        /// <param name = "scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static UShort3 operator *(ushort scale, UShort3 value)
        {
            return new UShort3((ushort)(value.X*scale), (ushort)(value.Y*scale), (ushort)(value.Z*scale));
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name = "value">The vector to scale.</param>
        /// <param name = "scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static UShort3 operator *(UShort3 value, ushort scale)
        {
            return new UShort3((ushort)(value.X*scale), (ushort)(value.Y*scale), (ushort)(value.Z*scale));
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name = "value">The vector to scale.</param>
        /// <param name = "scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static UShort3 operator /(UShort3 value, ushort scale)
        {
            return new UShort3((ushort)(value.X/scale), (ushort)(value.Y/scale), (ushort)(value.Z/scale));
        }

        /// <summary>
        /// Tests for equality between two objects.
        /// </summary>
        /// <param name = "left">The first value to compare.</param>
        /// <param name = "right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name = "left" /> has the same value as <paramref name = "right" />; otherwise, <c>false</c>.</returns>
        public static bool operator ==(UShort3 left, UShort3 right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Tests for inequality between two objects.
        /// </summary>
        /// <param name = "left">The first value to compare.</param>
        /// <param name = "right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name = "left" /> has a different value than <paramref name = "right" />; otherwise, <c>false</c>.</returns>
        public static bool operator !=(UShort3 left, UShort3 right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref = "UShort3" /> to <see cref = "Vector2" />.
        /// </summary>
        /// <param name = "value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Vector2(UShort3 value)
        {
            return new Vector2(value.X, value.Y);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref = "UShort3" /> to <see cref = "Vector3" />.
        /// </summary>
        /// <param name = "value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Vector3(UShort3 value)
        {
            return new Vector3(value.X, value.Y, value.Z);
        }

        /// <summary>
        /// Returns a <see cref = "System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref = "System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1} Z:{2}", X, Y, Z);
        }

        /// <summary>
        /// Returns a <see cref = "System.String" /> that represents this instance.
        /// </summary>
        /// <param name = "format">The format.</param>
        /// <returns>
        /// A <see cref = "System.String" /> that represents this instance.
        /// </returns>
        public string ToString(string format)
        {
            if (format == null)
                return ToString();

            return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1} Z:{2}",
                                 X.ToString(format, CultureInfo.CurrentCulture),
                                 Y.ToString(format, CultureInfo.CurrentCulture),
                                 Z.ToString(format, CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Returns a <see cref = "System.String" /> that represents this instance.
        /// </summary>
        /// <param name = "formatProvider">The format provider.</param>
        /// <returns>
        /// A <see cref = "System.String" /> that represents this instance.
        /// </returns>
        public string ToString(IFormatProvider formatProvider)
        {
            return string.Format(formatProvider, "X:{0} Y:{1} Z:{2}", X, Y, Z);
        }

        /// <summary>
        /// Returns a <see cref = "System.String" /> that represents this instance.
        /// </summary>
        /// <param name = "format">The format.</param>
        /// <param name = "formatProvider">The format provider.</param>
        /// <returns>
        /// A <see cref = "System.String" /> that represents this instance.
        /// </returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (format == null)
                ToString(formatProvider);

            return string.Format(formatProvider, "X:{0} Y:{1} Z:{2}", X.ToString(format, formatProvider),
                                 Y.ToString(format, formatProvider), Z.ToString(format, formatProvider));
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int)X;
                hashCode = (hashCode * 397) ^ Y;
                hashCode = (hashCode * 397) ^ Z;
                return hashCode;
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref = "UShort3" /> is equal to this instance.
        /// </summary>
        /// <param name = "other">The <see cref = "UShort3" /> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref = "UShort3" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(UShort3 other)
        {
            return other.X == X && other.Y == Y && other.Z == Z;
        }

        /// <summary>
        /// Determines whether the specified <see cref = "System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name = "value">The <see cref = "System.Object" /> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref = "System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object value)
        {
            if (value == null)
                return false;

            if (!ReferenceEquals(value.GetType(), typeof(UShort3)))
                return false;

            return Equals((UShort3) value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ushort"/> array to <see cref="UShort3"/>.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator UShort3(ushort[] input)
        {
            return new UShort3(input);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="UShort3"/> to <see cref="System.Ushort32"/> array.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ushort[](UShort3 input)
        {
            return input.ToArray();
        }
    }
}
