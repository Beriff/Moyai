using Moyai.Abstract;
using Moyai.Impl.Math;

namespace Moyai.Impl.Graphics
{
	public class Rectangle : IDrawable
	{
		public Rect MathRect { get; set; }
		public Symbol Fill { get; set; }

		public void Draw(ConsoleBuffer buf)
		{
			for(int x = MathRect.Start.X; x <= MathRect.End.X; x++)
			{
				for(int y = MathRect.Start.Y; y <= MathRect.End.Y; y++)
				{
					buf[x, y] = Fill;
				}
			}
		}

		public Rectangle(Vec2I start, Vec2I end, Symbol fill)
		{
			MathRect = new(start, end);
			Fill = fill;
		}
	}
}
