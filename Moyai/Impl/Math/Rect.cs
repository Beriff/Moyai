using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moyai.Impl.Math
{
	public class Rect
	{
		public Vec2I Start { get; set; }
		public Vec2I End { get; set; }
		public Vec2I Center { get => Start + (End - Start) / 2; }

		public static Rect FromSize(Vec2I start, Vec2I size)
		{
			return new(start, start + size);
		}

		public static Rect FromSize(Vec2I start, int size)
		{
			return new(start, start + new Vec2I(size));
		}

		public bool Contains(Vec2I point)
		{
			return point.X >= Start.X && point.X <= End.X
				&& point.Y >= Start.Y && point.Y <= End.Y;
		}

		public Rect(Vec2I start, Vec2I end)
		{
			Start = start;
			End = end;
		}

		public override string ToString()
		{
			return $"{{{Start}|{End}}}";
		}
	}
}
