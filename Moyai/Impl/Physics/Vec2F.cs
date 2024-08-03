using Moyai.Impl.Math;
using Moyai.Impl.Physics.Raytracing;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moyai.Impl.Physics
{
	public struct Vec2F
	{
		public float X { get; set; }
		public float Y { get; set; }
		public static Vec2F Zero { get => new(0); }

		public readonly float Length
		{
			get => MathF.Sqrt(X * X + Y * Y);
		}
		public readonly float LengthSquared
		{
			get => X * X + Y * Y;
		}

		public readonly float Dot(Vec2F v)
		{
			return X * v.X + Y * v.Y;
		}
		public readonly Vec2F Lerp(Vec2F v, float a)
		{
			return this + (v - this) * a;
		}

		public Vec2F(float x, float y)
		{
			X = x;
			Y = y;
		}
		public Vec2F(float s)
		{
			X = Y = s;
		}

		public static Vec2F operator +(Vec2F v1, Vec2F v2) => new(v1.X + v2.X, v1.Y + v2.Y);
		public static Vec2F operator -(Vec2F v1, Vec2F v2) => new(v1.X - v2.X, v1.Y - v2.Y);
		public static Vec2F operator *(Vec2F v1, Vec2F v2) => new(v1.X * v2.X, v1.Y * v2.Y);
		public static Vec2F operator *(Vec2F v1, float s) => new(v1.X * s, v1.Y * s);
		public static Vec2F operator /(Vec2F v1, Vec2F v2) => new(v1.X / v2.X, v1.Y / v2.Y);
		public static Vec2F operator /(Vec2F v1, float s) => new(v1.X / s, v1.Y / s);
	}
}
