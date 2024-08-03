using Moyai.Impl.Math;

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
                var cos = MathF.Cos;
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
                return m;
            }

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
            m[0, 0] * v.X + m[1, 0] * v.Y + m[2, 0] * v.Z,
            m[0, 1] * v.X + m[1, 1] * v.Y + m[2, 1] * v.Z,
            m[0, 2] * v.X + m[1, 2] * v.Y + m[2, 2] * v.Z
            );
        }
        public static Vec3F operator /(Vec3F v1, Vec3F v2) => new(v1.X / v2.X, v1.Y / v2.Y, v1.Z / v2.Z);
        public static Vec3F operator /(Vec3F v1, float s) => new(v1.X / s, v1.Y / s, v1.Z / s);

        public override string ToString()
        {
            return $"({X};{Y};{Z})";
        }
    }
}
