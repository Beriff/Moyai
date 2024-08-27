using Moyai.Impl.Graphics;
using Moyai.Impl.Graphics.Widgets;

namespace MoyaiPaint
{
	public partial class MoyaiPaint
	{
		public void OpenFile()
		{
			void fileSelected(string filepath)
			{
				OpenedFile = Path.GetFileName(filepath);
			}

			UI.FileSelectDialogue(new(40, 20), @"C:\", fileSelected);
		}

		public void CreateFile()
		{
			(var window, var input_handle) = UI.CreateDialogue("New File", new(40, 20),
				["OK"], (self, _) => { self.CloseAsDialogue(UI)(); });

			window.ActionQueue.Add(() =>
			{
				window.AddChild(
					new InputBox("Width", 10, InputBox.NumericValidator)
					{ Position = new(1, 1), LocalInput = input_handle },

					new InputBox("Height", 10, InputBox.NumericValidator)
					{ Position = new(12, 1), LocalInput = input_handle }

					);
			});

		}

		public MoyaiPaint()
		{
			void dispatch_fileMenu_event(string @event)
			{
				switch (@event)
				{
					case "New": CreateFile(); break;
					case "Open": OpenFile(); break;
				}
			}

			OpenedFile = null;
			UI = new(Main);
			_ = UI
				+ new Rectangle(new(0), new(SIZE.X - 1, 0), new('▓', new((255, 255, 255))))

				+ new HorizontalList(new(0), [
					new ExpandingSelection(
					Symbol.Text("File", new Moyai.Impl.ConsoleColor((255, 255, 255), (0, 0, 0))),
					["New", "Open", "Close"], dispatch_fileMenu_event, new(0))
				]);
		}
	}
}
