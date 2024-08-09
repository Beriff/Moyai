using Moyai.Abstract;
using Moyai.Impl.Math;

namespace Moyai.Impl.Graphics.Widgets
{
	public class ProgressBar : Widget
	{
		public float MaxValue { get; set; }
		public float MinValue { get; set; }
		public float Value { get; set; }
		public Symbol Back { get; set; }
		public Symbol Front { get; set; }
		public string? Label { get; set; }

		public override void Draw(ConsoleBuffer buf)
		{
			if (!Visible) return;

			float p = (Value - MinValue) / (MaxValue - MinValue);

			new Rectangle(Position, Position + AbsoluteSize, Back).Draw(buf);
			new Rectangle(Position, 
				new( (int)(Position.X + AbsoluteSize.X * p), Position.Y + AbsoluteSize.Y), Front)
				.Draw(buf);

			var s = Label is null ? $"{(int)(p * 100)}%" : Label;
			buf.BlitSymbString(
				Symbol.Text(s,
				new ConsoleColor(Front.Color.Background, ConsoleColor.Contrast(Front.Color.Background))),
				Bounds.Center - new Vec2I(s.Length / 2, 0));
			buf.BlitSymbString(
				Symbol.Text(s,
				new ConsoleColor(Front.Color.Background, ConsoleColor.Contrast(Front.Color.Background))),
				Bounds.Center - new Vec2I(s.Length / 2, 0));
		}

		public ProgressBar(float max, float min, Symbol back, Symbol front, string? label, Vec2I size)
			: base(null, true, true, new(0), size, new(0))
		{
			Label = label;
			Back = back;
			Front = front;
			MaxValue = max;
			MinValue = min;
			Value = 0;
		}
	}
}
