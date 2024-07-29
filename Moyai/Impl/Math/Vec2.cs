namespace Moyai.Impl.Math
{
    public struct Vec2
    {
        public int X { get; set; }
        public int Y { get; set; }
        public static Vec2 Zero { get => new(0, 0); }

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
    }
}
