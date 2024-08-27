
using Moyai.Abstract;
using Moyai.Impl.Input;
using Moyai.Impl.Math;

namespace Moyai.Impl.Graphics.Widgets
{
	public class Window : Frame
	{
		public Action OnClose { get; set; } = () => { };
		protected bool Held { get; set; }
		public Action CloseAsDialogue(DrawingLayer dl) => () =>
		{
			InputBus.RemoveConsumer(LocalInput);
			dl.ActionQueue.Add(() =>
			{
				dl.Drawables.Remove(this);
			});
		};
		public override void Draw(ConsoleBuffer buf)
		{
			base.Draw(buf);
			buf[Position.X + AbsoluteSize.X - 3, Position.Y] = new('X', ConsoleColor.Default);
		}
		public override void Update()
		{
			base.Update();

			if(LocalInput.KeyState(Keys.MouseLeft) == InputType.JustReleased)
			{
				if(LocalInput.MousePos().Equals(new Vec2I(Position.X + AbsoluteSize.X - 3, Position.Y)))
				{
					OnClose();
				}
			}

			if(Held)
			{
				Position += LocalInput.MouseDelta;
				if(LocalInput.KeyState(Keys.MouseLeft) == InputType.JustReleased)
				{
					Held = false;
				}
			}
			if(LocalInput.KeyState(Keys.MouseLeft) == InputType.JustPressed)
			{
				if(new Rect(Position, new(Position.X+AbsoluteSize.X, Position.Y+2)).Contains(LocalInput.MousePos()))
				{
					Held = true;
				}
			}
		}

		public void PositionActionButtons(params Button[] buttons)
		{
			int dist = Position.X + AbsoluteSize.X - 1;
			foreach(var button in buttons)
			{
				dist -= button.Size.X + 1;
				AddChild(button);
				button.LocalInput = LocalInput;
				button.Position = new(dist, 
					Position.Y + AbsoluteSize.Y - 1);
			}
		}

		public Window(string label, Vec2I size)
			: base(Symbol.Text(label), ConsoleColor.Default, size)
		{

		}
	}
}
