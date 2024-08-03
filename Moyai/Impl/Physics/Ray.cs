using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moyai.Impl.Physics
{
	public struct Ray
	{
		public Vec3F Direction {  get; set; }
		public Vec3F Origin { get; set; }

		public readonly Vec3F Point(float t)
		{
			return Origin + Direction * t;
		}

		public Ray(Vec3F direction, Vec3F origin)
		{
			Direction = direction.Normalized;
			Origin = origin;
		}

		public override string ToString()
		{
			return $"({Origin} -> {Direction})";
		}
	}
}
