using Moyai.Abstract;
using Moyai.Impl.Graphics;
using Moyai.Impl.Graphics.Widgets;
using Moyai.Impl.Math;

namespace MoyaiPaint
{
	public class MoyaiPaint : RenderContext
	{
		private static Vec2I SIZE = new(140, 50);

		public ConsoleBuffer? Canvas;
		public ConsoleBuffer CanvasAcceptor = new(new(SIZE.Y,SIZE.Y));
		public ConsoleBuffer Main = new(SIZE);

		public DrawingLayer UI;
		public bool Saved = true;
		private string? _OpenedFile;
		public string? OpenedFile
		{
			get => _OpenedFile;
			set
			{
				_OpenedFile = value;
				if (value == null)
					Console.Title = "MoyaiPaint // No file ";
				else
					Console.Title = $"MoyaiPaint // {OpenedFile}";
			}
		}

		public MoyaiPaint()
		{
			OpenedFile = null;
			UI = new(Main);
			_ = UI
				+ new Rectangle(new(0), new(SIZE.X - 1, 0), new('▓',new((255,255,255))))
				+ new ExpandingSelection(
					Symbol.Text("File", new Moyai.Impl.ConsoleColor((255,255,255), (0,0,0))), ["New", "Close"], (s) => { }, new(0));
		}
		public override void Render()
		{
			UI.Draw();
			base.Render();
		}
		public override void Update()
		{
			UI.Update();
			base.Update();
		}
	}

	internal class Program
	{
		static void Main(string[] args)
		{
			new MoyaiPaint().Start();
		}
	}
}