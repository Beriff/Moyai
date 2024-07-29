using Moyai.Abstract;
using Moyai.Impl.Graphics;
using Moyai.Impl.Graphics.Widgets;
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
			Frame = new(
				Symbol.Text("Shrimple frame", new Impl.ConsoleColor((255,255,255), (0,0,0))),
				new('▓', new((255,255,255))),
				new(20, 10));

			for (int a = 0; a < 30; a++)
			{
				for (int b = 0; b < 30; b++)
				{
					var col = ((byte)255, (byte)(255 * a / 30f), (byte)(255 * b / 30f));
					MainBuf[a, b] = new(
						'▓',
						new(col, col)
						);
				}
			}
		}
		public override void Render()
		{
			MainBuf.Clear();

			Frame.Draw(MainBuf);

			{
				var text = Symbol.Text(
					$"{1 / TimeDelta} FPS",
					Impl.ConsoleColor.OnlyFg((255, 255, 255)));
				MainBuf.BlitSymbString(text, new(0, 11));
			}

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