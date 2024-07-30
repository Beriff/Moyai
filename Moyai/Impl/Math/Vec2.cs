namespace Moyai.Impl.Math
{
    public struct Vec2
    {
        public int X { get; set; }
        public int Y { get; set; }
        public static Vec2 Zero { get => new(0, 0); }

        public readonly Vec2 Clamp(Vec2 min, Vec2 max)
        {
            var n = this;
            n.X = System.Math.Clamp(n.X, min.X, max.X);
			n.Y = System.Math.Clamp(n.Y, min.Y, max.Y);

			return n;
		}

        public Vec2(int x, int y)
        {
            X = x;
            Y = y;
        }
        public Vec2(int s)
        {
            X = Y = s;
        }

        public static Vec2 operator +(Vec2 v1, Vec2 v2) => new(v1.X + v2.X, v1.Y + v2.Y);
        public static Vec2 operator -(Vec2 v1, Vec2 v2) => new(v1.X - v2.X, v1.Y - v2.Y);
        public static Vec2 operator *(Vec2 v1, Vec2 v2) => new(v1.X * v2.X, v1.Y * v2.Y);
        public static Vec2 operator *(Vec2 v1, float s) => new((int)(v1.X * s), (int)(v1.Y * s));
        public static Vec2 operator /(Vec2 v1, Vec2 v2) => new(v1.X / v2.X, v1.Y / v2.Y);
        public static Vec2 operator /(Vec2 v1, float s) => new((int)(v1.X / s), (int)(v1.Y / s));

		public override string ToString()
		{
            return $"({X};{Y})";
		}
	}
}
