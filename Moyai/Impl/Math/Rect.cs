using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moyai.Impl.Math
{
	public class Rect
	{
		public Vec2 Start { get; set; }
		public Vec2 End { get; set; }
		public Vec2 Center { get => Start + (End - Start) / 2; }

		public static Rect FromSize(Vec2 start, Vec2 size)
		{
			return new(start, start + size);
		}

		public static Rect FromSize(Vec2 start, int size)
		{
			return new(start, start + new Vec2(size));
		}

		public Rect(Vec2 start, Vec2 end)
		{
			Start = start;
			End = end;
		}
	}
}
