using Moyai.Impl.Math;

using System.Numerics;

namespace Moyai.Impl.Physics.Raytracing
{
    public struct Vec3F
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public readonly float Yaw { get => X; }
        public readonly float Pitch { get => Z; }
        public readonly float Roll { get => Y; }


        public static Vec3F Zero { get => new(0); }
        public static Vec3F One { get => new(1); }
        public static Vec3F Up { get => new(0, 1, 0); }
        public static Vec3F Forward { get => new(1, 0, 0); }
        public static Vec3F Right { get => new(0,0,1); }

        public readonly float Length
        {
            get => MathF.Sqrt(X * X + Y * Y + Z * Z);
        }
        public readonly float LengthSquared
        {
            get => X * X + Y * Y + Z * Z;
        }
        public readonly Vec3F Normalized
        {
            get
            {
                float l = Length;
                return new(X / l, Y / l, Z / l);
            }
        }
        public readonly float Dot(Vec3F v)
        {
            return X * v.X + Y * v.Y + Z * v.Z;
        }
        public readonly Vec3F Cross(Vec3F v)
        {
            return new(Z * v.Y - Y * v.Z, X * v.Z - Z * v.X, Y * v.X - X * v.Y);
        }
        public readonly Vec3F Lerp(Vec3F v, float a)
        {
            return this + (v - this) * a;
        }
        public readonly Matrix3x3 Rotation
        {
            get
            {
				/*var cos = MathF.Cos;
                var sin = MathF.Sin;
                Matrix3x3 m = new() { Values = new float[3,3] };
                m[0, 0] = cos(Yaw) * cos(Pitch);
                m[1, 0] = cos(Yaw) * sin(Pitch) * sin(Roll) - sin(Yaw) * cos(Roll);
                m[2, 0] = cos(Yaw) * sin(Pitch) * cos(Roll) + sin(Yaw) * sin(Roll);

                m[0, 1] = sin(Yaw) * cos(Pitch);
                m[1, 1] = sin(Yaw) * sin(Pitch) * sin(Roll) + cos(Yaw) * cos(Roll);
                m[2, 1] = sin(Yaw) * sin(Pitch) * cos(Roll) - cos(Yaw) * sin(Roll);

                m[0, 2] = -sin(Pitch);
                m[1, 2] = cos(Pitch) * sin(Roll);
                m[2, 2] = cos(Pitch) * cos(Roll);
                return m;*/

				float cy = MathF.Cos(Yaw);
				float sy = MathF.Sin(Yaw);
				float cp = MathF.Cos(Pitch);
				float sp = MathF.Sin(Pitch);
				float cr = MathF.Cos(Roll);
				float sr = MathF.Sin(Roll);

                Matrix3x3 m = new() { Values = new float[3, 3] };
				m[0, 0] = cy * cp;
				m[0, 1] = cy * sp * sr - sy * cr;
				m[0, 2] = cy * sp * cr + sy * sr;

				m[1, 0] = sy * cp;
				m[1, 1] = sy * sp * sr + cy * cr;
				m[1, 2] = sy * sp * cr - cy * sr;

				m[2, 0] = -sp;
				m[2, 1] = cp * sr;
				m[2, 2] = cp * cr;

				return m;

			}

        }

        public readonly Vec3F RotateAround(Vec3F axis, float angle)
        {
            float cos_theta = MathF.Cos(angle);
            float sin_theta = MathF.Sin(angle);
            Vec3F r = this - axis;

            Vec3F rotated = r * cos_theta +
                axis * (axis.Dot(r) * (1 - cos_theta)) +
                axis.Cross(r) * sin_theta;

            return rotated + axis;
        }

        public readonly float Min
        {
            get => MathF.Min(MathF.Min(X, Y), Z);
        }
        public readonly float Max
        {
            get => MathF.Max(MathF.Max(X, Y), Z);
        }

        public Vec3F(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Vec3F(float s)
        {
            X = Y = Z = s;
        }

        public static Vec3F operator +(Vec3F v1, Vec3F v2) => new(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        public static Vec3F operator -(Vec3F v1, Vec3F v2) => new(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        public static Vec3F operator *(Vec3F v1, Vec3F v2) => new(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);
        public static Vec3F operator *(Vec3F v1, float s) => new(v1.X * s, v1.Y * s, v1.Z * s);
        public static Vec3F operator *(Vec3F v, Matrix3x3 m)
        {
            return new(
			m[0, 0] * v.X + m[0, 1] * v.Y + m[0, 2] * v.Z,
			m[1, 0] * v.X + m[1, 1] * v.Y + m[1, 2] * v.Z,
			m[2, 0] * v.X + m[2, 1] * v.Y + m[2, 2] * v.Z
            );
		}
		public static Vec3F operator /(Vec3F v1, Vec3F v2) => new(v1.X / v2.X, v1.Y / v2.Y, v1.Z / v2.Z);
        public static Vec3F operator /(Vec3F v1, float s) => new(v1.X / s, v1.Y / s, v1.Z / s);
        public static Vec3F operator -(Vec3F v) { return new(-v.X, -v.Y, -v.Z); }

        public override string ToString()
        {
            return $"({X};{Y};{Z})";
        }
    }
}
