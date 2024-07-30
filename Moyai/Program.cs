using Moyai.Abstract;
using Moyai.Impl.Graphics;
using Moyai.Impl.Graphics.Widgets;
using Moyai.Impl.Input;
using Moyai.Impl.Math;

namespace Moyai
{
    class ExampleApp : RenderContext
	{
		public ConsoleBuffer MainBuf;
		public Frame Frame;
		public ExampleApp() : base()
		{
			MainBuf = new(new(30));
		}
		public override void Render()
		{
			MainBuf.Clear();

			MainBuf.BlitSymbString(
				Symbol.Text($"{InputHandler.MousePos(MainBuf)}"),
				Vec2.Zero
				);

			// Draw FPS
			{
				var text = Symbol.Text(
					$"{1 / TimeDelta} FPS",
					Impl.ConsoleColor.OnlyFg((255, 255, 255)));
				MainBuf.BlitSymbString(text, new(0, 11));
			}

			var pos = InputHandler.MousePos(MainBuf);
			if(pos.X < 0) { throw new Exception("WHAT"); }
			MainBuf[pos.X, pos.Y] = new Symbol('▓', new((255, 255, 255)));

			MainBuf.Render();
			base.Render();
		}
	}

	internal class Program
	{
		static void Main(string[] args)
		{
			new ExampleApp().Start();
		}
	}
}