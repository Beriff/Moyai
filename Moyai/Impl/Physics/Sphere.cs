using Moyai.Abstract.Physics;

namespace Moyai.Impl.Physics
{
	public class Sphere(Vec3F Center, float radius) : Body
	{
		public Vec3F Center { get; set; } = Center;
		public float Radius { get; set; } = radius;
		public override Vec3F[]? Intersection(Ray ray)
		{
			float B = 2 * (
			ray.Direction.X * (ray.Origin.X - Center.X) +
			ray.Direction.Y * (ray.Origin.Y - Center.Y) +
			ray.Direction.Z * (ray.Origin.Z - Center.Z));
			float C = (ray.Origin.X - Center.X) * (ray.Origin.X - Center.X) +
				(ray.Origin.Y - Center.Y) * (ray.Origin.Y - Center.Y) +
				(ray.Origin.Z - Center.Z) * (ray.Origin.Z - Center.Z)
				- Radius * Radius;

			//compute the discriminant
			float d = B * B - 4 * C;
			if (d < 0) { return null; }
			d = MathF.Sqrt(d);

			//compute the intersection points
			Vec3F t1 = ray.Point((-B - d) / 2);
			Vec3F t2 = ray.Point((-B + d) / 2);

			return [t1, t2];
		}
	}
}
