using Moyai.Abstract;
using Moyai.Impl.Graphics;
using Moyai.Impl.Graphics.Widgets;
using Moyai.Impl.Math;

namespace MoyaiPaint
{
	public partial class MoyaiPaint : RenderContext
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