using Moyai.Impl.Math;
using Moyai.Impl.Physics.Raytracing;

namespace Moyai.Abstract.Physics
{
    public abstract class Body
	{
		public abstract List<Vec3F>? Intersection(Ray ray);
		public abstract Vec2F UV(Vec3F surface);
	}
}
