using Moyai.Impl.Graphics;
using Moyai.Impl.Graphics.Widgets;

using System.Text.Json;

namespace MoyaiPaint
{
	public partial class MoyaiPaint
	{
		public void OpenFile()
		{
			void fileSelected(string filepath)
			{
				OpenedFile = Path.GetFileName(filepath);
				InitializeFile(filepath);
			}

			UI.FileSelectDialogue(new(40, 20), @"C:\", fileSelected);
		}

		public void CreateFile()
		{
			(var window, var input_handle) = UI.CreateDialogue("New File", new(40, 15),
				["OK", "Cancel"], (self, option) => 
				{
					switch(option) {
						case "Cancel": self.CloseAsDialogue(UI)(); break;
						case "OK":
							foreach(var child in self.Children)
							{
								if (child is InputBox && (child as InputBox).Empty)
									return;
							}
							self.CloseAsDialogue(UI)();
							UI.FileCreateDialogue(new(40, 20), @"C:\", "json", (_) => { });
						
						break;
					}
				});

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

		public void InitializeFile(string filepath)
		{
			var text = File.ReadAllText(filepath);
			var dsprite = JsonSerializer.Deserialize<DeserializedSprite>(text);
			if(dsprite != null)
			{
				Canvas canvas = new(new(0, 3), dsprite);
				UI.ActionQueue.Add(() => _=UI + canvas);
				(UI.Get("CanvasTabs") as TabContainer).AddTab(dsprite.name);
			}
				
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

				+ new TabContainer(new(0, 1), new(SIZE.X - 1, SIZE.Y - 1), [])
				{ Name = "CanvasTabs" }

				+ new HorizontalList(new(0), [
					new ExpandingSelection(
					Symbol.Text("File", new Moyai.Impl.ConsoleColor((255, 255, 255), (0, 0, 0))),
					["New", "Open", "Save", "Close"], dispatch_fileMenu_event, new(0))
				]);
		}
	}
}
