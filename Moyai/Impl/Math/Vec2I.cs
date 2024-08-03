namespace Moyai.Impl.Math
{
    public struct Vec2I
    {
        public int X { get; set; }
        public int Y { get; set; }
        public static Vec2I Zero { get => new(0, 0); }

        public readonly Vec2I Clamp(Vec2I min, Vec2I max)
        {
            var n = this;
            n.X = System.Math.Clamp(n.X, min.X, max.X);
			n.Y = System.Math.Clamp(n.Y, min.Y, max.Y);

			return n;
		}

        public Vec2I(int x, int y)
        {
            X = x;
            Y = y;
        }
        public Vec2I(int s)
        {
            X = Y = s;
        }

        public static Vec2I operator +(Vec2I v1, Vec2I v2) => new(v1.X + v2.X, v1.Y + v2.Y);
        public static Vec2I operator -(Vec2I v1, Vec2I v2) => new(v1.X - v2.X, v1.Y - v2.Y);
        public static Vec2I operator *(Vec2I v1, Vec2I v2) => new(v1.X * v2.X, v1.Y * v2.Y);
        public static Vec2I operator *(Vec2I v1, float s) => new((int)(v1.X * s), (int)(v1.Y * s));
        public static Vec2I operator /(Vec2I v1, Vec2I v2) => new(v1.X / v2.X, v1.Y / v2.Y);
        public static Vec2I operator /(Vec2I v1, float s) => new((int)(v1.X / s), (int)(v1.Y / s));

		public override string ToString()
		{
            return $"({X};{Y})";
		}
	}
}
