using Moyai.Abstract.Physics;

namespace Moyai.Impl.Physics.Raytracing
{
    public class Sphere(Vec3F Center, float radius) : Body
    {
        public Vec3F Center { get; set; } = Center;
        public float Radius { get; set; } = radius;
        public override List<Vec3F>? Intersection(Ray ray)
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
            Vec3F point1 = ray.Point((-B - d) / 2);
            Vec3F point2 = ray.Point((-B + d) / 2);

            return new([point1, point2]);
        }

		public override Vec2F UV(Vec3F surface)
		{
			var unit = (surface - Center).Normalized;
            float u = 0.5f + MathF.Atan2(unit.X, unit.Z) / (2 * MathF.PI);
            float v = 0.5f + MathF.Asin(unit.Y) / -MathF.PI;
            return new(u, v);
		}
	}
}
