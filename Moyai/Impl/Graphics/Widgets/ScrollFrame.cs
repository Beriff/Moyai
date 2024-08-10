using Moyai.Impl.Input;
using Moyai.Impl.Math;

namespace Moyai.Impl.Graphics.Widgets
{
	public class ScrollFrame : Frame
	{
		public bool RenderFrame { get; set; } = true;
		public ScrollFrame(Symbol[] label, ConsoleColor border, Vec2I size) : base(label, border, size)
		{
			Buffer = new(AbsoluteSize - new Vec2I(1));
			OnClick = (w) => 
			{ 
				Focused = !Focused;
				if (Focused)
				{
					var str = Symbol.StringFromText(Label);
					str = "[" + str + "]";
					Label = Symbol.Text(str);
				} else
				{
					var str = Symbol.StringFromText(Label)[1..^1];
					Label = Symbol.Text(str);
				}
			};
		}

		public static ScrollFrame NoFrame(Vec2I size, Vec2I pos)
		{
			return new(Array.Empty<Symbol>(), new(), size) 
			{ 
				Position = pos, 
				RenderFrame = false 
			};
		}

		public ConsoleBuffer Buffer { get; private set; }
		public int Scroll { get; set; } = 0;
		public override void Draw(ConsoleBuffer buf)
		{
			if (!Visible) return;

			Buffer.Clear();
			foreach(var child in Children)
			{
				child.Position += new Vec2I(0, Scroll) - Position;
				child.Draw(Buffer);
				child.Position -= new Vec2I(0, Scroll) - Position;
			}

			Buffer.Blit(buf, Position + new Vec2I(1));

			if (RenderFrame)
				base.Draw(buf);
		}

		public override void Update()
		{
			foreach (var child in Children)
				child.Position += new Vec2I(0, Scroll);
			base.Update();
			foreach (var child in Children)
				child.Position -= new Vec2I(0, Scroll);

			if (InputHandler.KeyState(Keys.ArrowUp) == InputType.JustPressed)
			{
				Scroll++;
			}
			else if (InputHandler.KeyState(Keys.ArrowDown) == InputType.JustPressed)
			{
				Scroll--;
			}
			
		}

	}
}
