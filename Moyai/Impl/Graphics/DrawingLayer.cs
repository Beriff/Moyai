using Moyai.Abstract;
using Moyai.Impl.Graphics.Widgets;
using Moyai.Impl.Input;
using Moyai.Impl.Math;

namespace Moyai.Impl.Graphics
{
	public class DrawingLayer : IDelayableActionHolder
	{
		public ConsoleBuffer Buffer { get; set; }
		public List<IDrawable> Drawables { get; set; }
		public List<Action> ActionQueue { get; set; } = [];

		public (Window, InputConsumer) CreateDialogue(string label, Vec2I size)
		{
			var w = new Window(label, size);
			var c = new InputConsumer(w.Name + "_input", InputBus.HigherPriority("UI")) { Blocking = true };

			ActionQueue.Add(() =>
			{
				Drawables.Add(w);
				InputBus.AddConsumer("UI", c);

				w.OnClose = () =>
				{
					Drawables.Remove(w);
					InputBus.RemoveConsumer(c);
				};

				w.LocalInput = c;
			});

			return (w, c);
		}

		public (Window, InputConsumer) FileSelectDialogue(Vec2I size, string path, Action<string> callback, string? ext = null)
		{
			// Create the basic window to hold the file select dialogue
			var w = new Window("Select file", size);

			// Create a new input consumer that blocks input propogation to layers below
			var c = new InputConsumer(w.Name + "_input", InputBus.HigherPriority("UI"))
			{ Blocking = true };

			ActionQueue.Add(() => {
				string? selectedFile = null;

				Drawables.Add(w);
				var last = Drawables.Count;
				w.OnClick = (self) =>
				{
					if (self.LocalInput.MousePos().Equals(
						self.Position + new Vec2I(self.AbsoluteSize.X - 3, 0)
						)
					)
					{
						InputBus.RemoveConsumer(self.LocalInput);
						ActionQueue.Add(() => { Drawables.RemoveAt(last - 1); });
					}
				};

				InputBus.AddConsumer("UI", c);
				w.LocalInput = c;

				// Create a scrolling frame with no borders to hold a list of files and dirs
				var scroll = ScrollFrame.NoFrame(size - new Vec2I(1), new(1));
				scroll.LocalInput = c;
				w.AddChild(scroll);

				w.AddChild(new Button("Select", new((255, 255, 255)), new((127, 127, 127)), new(8, 1))
				{
					Position = size - new Vec2I(10, 1),
					LocalInput = c,
					OnClick = (w) =>
					{
						if (selectedFile != null)
						{
							InputBus.RemoveConsumer(c);
							ActionQueue.Add(() => { Drawables.RemoveAt(last - 1); });
							callback(selectedFile);
						}
					}
				});

				// Vertical list to align items automatically
				var list = new VerticalList(new(1), padding: 0) { LocalInput = c };
				scroll.AddChild(list);

				// Current directory indicator
				list.AddChild(new Label(Symbol.Text($"[{path}]"), w.Position + new Vec2I(1)) { LocalInput = c });

				// a function that highlights hovered file/dir
				void hovertoggle(Widget self)
				{
					var l = self as Label;
					l.Text = Symbol.Text(
						Symbol.StringFromText(l.Text),
						(i) =>
							{
								// invert fg and bg
								return new ConsoleColor((255, 255, 255)) - l.Text[i].Color;
							}
						);
				}

				// a function that populates the vertical list with files/dirs from dirname
				void populate(string dirname)
				{
					list.ActionQueue.Add(() => list.AddChild(
						new Label(
							new Symbol[] { new Symbol('*', new((255, 255, 255), (0, 0, 0))) }.Concat(Symbol.Text("..")).ToArray(),
							w.Position + new Vec2I(1))
						{ LocalInput = c, OnHover = hovertoggle, OnHoverEnd = hovertoggle, OnClick = ondirclick(Path.GetFullPath(Path.Join(dirname, ".."))) }
						));
					foreach (var s in Directory.GetFileSystemEntries(dirname))
					{
						FileAttributes fa = File.GetAttributes(s);
						Symbol q;
						if (fa.HasFlag(FileAttributes.Directory))
						{
							q = new Symbol('*', new((255, 255, 255), (0, 0, 0)));
							list.ActionQueue.Add(() => list.AddChild(
							new Label(
								new Symbol[] { q }.Concat(Symbol.Text(" " + new DirectoryInfo(s).Name)).ToArray(),
							w.Position + new Vec2I(1))
							{ LocalInput = c, OnHover = hovertoggle, OnHoverEnd = hovertoggle, OnClick = ondirclick(Path.Join(dirname, new DirectoryInfo(s).Name)) }
						));
						}
						else
						{
							if (ext != null && Path.GetExtension(s) != ext) continue;

							q = new Symbol('F', new((255, 255, 255), (0, 0, 0)));
							string filename = s == selectedFile ? $"[{Path.GetFileName(s)}]" : Path.GetFileName(s);
							list.ActionQueue.Add(() => list.AddChild(
							new Label(
								new Symbol[] { q }.Concat(Symbol.Text(" " + filename)).ToArray(),
							w.Position + new Vec2I(1))
							{
								LocalInput = c,
								OnHover = hovertoggle,
								OnHoverEnd = hovertoggle,
								OnClick = new Action<Widget>((w) => { selectedFile = s; }) + ondirclick(dirname) }
						));
						}
					}
				}

				// a function that creates an OnClick callback, that updates the dialogue
				// based on what directory you clicked
				Action<Widget> ondirclick(string dirname) {

					return (Widget self) => {
						scroll.Scroll = 0;
						list.ActionQueue.Add(() => list.Children.Clear());
						list.ActionQueue.Add(() => list.AddChild(new Label(Symbol.Text($"[{dirname}]"), w.Position + new Vec2I(1)) { LocalInput = c }));
						populate(dirname);

					};

				}
				populate(path);
			});
			return (w, c);
		}

		public void Update()
		{
			foreach (var act in ActionQueue) { act(); }
			ActionQueue.Clear();
			foreach (var d in Drawables)
			{
				if(d is Widget widget)
					widget.Update();
			}
		}
		public void Draw()
		{
			Buffer.Clear();
			Drawables = Drawables.OrderByDescending((d) => d.ZLayer).ToList();
			foreach (var d in Drawables)
				d.Draw(Buffer);
			Buffer.Render();
		}

		public DrawingLayer(ConsoleBuffer buf)
		{
			Buffer = buf;
			Drawables = new();
		}

		public static DrawingLayer operator +(DrawingLayer layer, IDrawable drawable)
		{
			layer.Drawables.Add(drawable);
			return layer;
		}
	}
}
