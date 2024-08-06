using Moyai.Impl.Math;

namespace Moyai.Impl.Physics.Raytracing
{
    public struct RectF
    {
        public Vec3F Position;
        public Vec2F Size;
        public Vec3F Down = -Vec3F.Up;
        public Vec3F Right = Vec3F.Right;

        public readonly RectF Rotate(Vec3F rotation)
        {
            var mat = rotation.Rotation;
            var rect = this;
            rect.Position *= mat;
            rect.Down *= mat;
            rect.Right *= mat;
            return rect;
        }
        public readonly Vec3F Center
        {
            get => this[.5f, .5f];
        }

        public RectF(Vec3F pos, Vec2F size, Vec3F? down = null, Vec3F? right = null)
        {
            Position = pos;
            Size = size;
            Down = down ?? -Vec3F.Up;
            Right = right ?? Vec3F.Right;
        }

        public readonly Vec3F this[float tx, float ty]
        {
            get => Position + (Down * ty * Size.Y) + (Right * tx * Size.X);
        }

        public static RectF operator + (RectF r, Vec3F v)
        {
            var nr = r;
            r.Position += v;
            return nr;
        }
		public static RectF operator -(RectF r, Vec3F v)
		{
			var nr = r;
			r.Position -= v;
			return nr;
		}
	}
}
