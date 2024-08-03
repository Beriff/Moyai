using Moyai.Abstract;
using Moyai.Impl.Math;

namespace Moyai.Impl.Graphics.Widgets
{
	public class Button : Widget
	{
		public string Label;
		public ConsoleColor MainColor;
		public ConsoleColor HoverColor;

		public override void Draw(ConsoleBuffer buf)
		{
			var size = AbsoluteSize;
			for (int x = Position.X; x < Position.X + size.X; x++)
			{
				for (int y = Position.Y; y < Position.Y + size.Y; y++)
				{
					buf[x,y] = new Symbol('▓', Hovered ? HoverColor : MainColor);
				}
			}

			var active_col = Hovered ? HoverColor.Background : MainColor.Background;
			var color = new ConsoleColor(
				active_col,
				ConsoleColor.Contrast(active_col));

			buf.BlitSymbString(
				Symbol.Text(Label, color), 
				Bounds.Center - new Vec2(Label.Length / 2, 0));
		}

		public Button(string label, ConsoleColor maincol, ConsoleColor hovercol, Vec2 size)
			: base(null, true, true, new(0), size, new(0))
		{
			Label = label;
			MainColor = maincol;
			HoverColor = hovercol;
		}
	}
}
