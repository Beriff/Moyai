using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moyai.Impl.Physics
{
	public struct RectF
	{
		public Vec3F Start;
		public Vec3F End;

		public Vec3F Size
		{
			get => End - Start;
		}
		public static RectF FromCenter(Vec3F center, float size, Vec3F? bottomright = null)
		{
			bottomright ??= new(1, -1, 0);
			return new(
				center - (Vec3F)bottomright * size / 2,
				center + (Vec3F)bottomright * size / 2
				);
		}
		public readonly Vec3F Center
		{
			get => Start.Lerp(End, .5f);
		}

		public RectF(Vec3F start, Vec3F end)
		{
			Start = start;
			End = end;
		}

		public readonly Vec3F this[float tx, float ty]
		{
			get
			{
				var x = Start.Lerp(new Vec3F(End.X, Start.Y, End.Z), tx);
				var y = Start.Lerp(new Vec3F(Start.X, End.Y, Start.Z), ty);
				return new(x.X, y.Y, x.Z);
			}
		}
	}
}
