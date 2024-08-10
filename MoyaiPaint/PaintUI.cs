using Moyai.Impl.Graphics.Widgets;
using Moyai.Impl.Graphics;

namespace MoyaiPaint
{
	public partial class MoyaiPaint
	{
		public MoyaiPaint()
		{
			var dispatch_file_event = (string @event) =>
			{
				switch(@event)
				{
				}
			};

			OpenedFile = null;
			UI = new(Main);
			_ = UI
				+ new Rectangle(new(0), new(SIZE.X - 1, 0), new('▓', new((255, 255, 255))))

				+ new HorizontalList(new(0), [
					new ExpandingSelection(
					Symbol.Text("File", new Moyai.Impl.ConsoleColor((255, 255, 255), (0, 0, 0))), ["New", "Close"], (s) => { }, new(0))
				]);

			//+ new Window("Test", new(20, 7)) { Position = new(2) };

			(var window, var uihandle) = UI.FileSelectDialogue(new(40, 20), @"C:\");
		}
	}
}
