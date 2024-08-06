using Moyai.Abstract;
using Moyai.Impl.Math;

namespace Moyai.Impl.Graphics.Widgets
{
	public class Label : Widget
	{
		public Symbol[] Text;

		public override Vec2I Size 
		{
			get => new(Text.Length, 1);
			set { }
		}

		public override void Draw(ConsoleBuffer buf)
		{
			buf.BlitSymbString(Text, Position);
		}

		public Label(Symbol[] text, Vec2I position)
			: base(null, true, true, position, Vec2I.Zero, new(1))
		{
			Text = text;
		}
	}
}
