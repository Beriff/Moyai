using Moyai.Abstract;
using Moyai.Impl.Graphics.Widgets;
using Moyai.Impl.Input;
using Moyai.Impl.Math;

namespace Moyai.Impl.Graphics
{
	public class DrawingLayer
	{
		public ConsoleBuffer Buffer { get; set; }
		public List<IDrawable> Drawables { get; set; }

		public (Window, InputConsumer) CreateDialogue(string label, Vec2I size)
		{
			var w = new Window(label, size);
			Drawables.Add(w);
			
			var c = new InputConsumer(w.Name + "_input", InputBus.HigherPriority("UI"))
			{ Blocking = true };
			InputBus.AddConsumer("UI", c);

			w.OnClose = () =>
			{
				Drawables.Remove(w);
				InputBus.RemoveConsumer(c);
			};

			w.InputUI = c;
			return (w, c);
		}

		public (Window, InputConsumer) FileSelectDialogue(Vec2I size, string path, string? ext = null)
		{
			// Create the basic window to hold the file select dialogue
			var w = new Window("Select file", size);
			Drawables.Add(w);

			// Create a new input consumer that blocks input propogation to layers below
			var c = new InputConsumer(w.Name + "_input", InputBus.HigherPriority("UI"))
			{ Blocking = true };
			InputBus.AddConsumer("UI", c);
			w.InputUI = c;

			// Create a scrolling frame with no borders to hold a list of files and dirs
			var scroll = ScrollFrame.NoFrame(size - new Vec2I(1), new(1));
			scroll.InputUI = c;
			w.AddChild(scroll);

			w.AddChild(new Button("Select", new((255, 255, 255)), new((127, 127, 127)), new(8, 1))
			{ Position = size - new Vec2I(10, 1), InputUI = c });

			// Vertical list to align items automatically
			var list = new VerticalList(new(1), padding: 0) { InputUI = c };
			scroll.AddChild(list);

			// Current directory indicator
			list.AddChild(new Label(Symbol.Text($"[{path}]"), new(1)) { InputUI = c });

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
				list.QueueChildAction(() => list.AddChild(
					new Label(
						new Symbol[] { new Symbol('*', new((255, 255, 255), (0, 0, 0))) }.Concat(Symbol.Text("..")).ToArray(),
						new(1))
					{ InputUI = c,OnHover = hovertoggle, OnHoverEnd = hovertoggle, OnClick = ondirclick(Path.GetFullPath(Path.Join(dirname, ".."))) }
					));
				foreach (var s in Directory.GetFileSystemEntries(dirname))
				{
					FileAttributes fa = File.GetAttributes(s);
					Symbol q;
					if (fa.HasFlag(FileAttributes.Directory))
					{
						q = new Symbol('*', new((255, 255, 255), (0, 0, 0)));
						list.QueueChildAction(() => list.AddChild(
						new Label(
							new Symbol[] { q }.Concat(Symbol.Text(" " + new DirectoryInfo(s).Name)).ToArray(),
						new(1))
						{ InputUI = c, OnHover = hovertoggle, OnHoverEnd = hovertoggle, OnClick = ondirclick(Path.Join(dirname, new DirectoryInfo(s).Name) ) }
					));
					}
					else
					{
						q = new Symbol('F', new((255, 255, 255), (0, 0, 0)));
						list.QueueChildAction(() => list.AddChild(
						new Label(
							new Symbol[] { q }.Concat(Symbol.Text(" " + Path.GetFileName(s))).ToArray(),
						new(1))
						{ InputUI = c, OnHover = hovertoggle, OnHoverEnd = hovertoggle }
					));
					}
				}
			}

			// a function that creates an OnClick callback, that updates the dialogue
			// based on what directory you clicked
			Action<Widget> ondirclick(string dirname) {

				return (Widget self) => {
					scroll.Scroll = 0;
					list.QueueChildAction(() => list.Children.Clear());
					list.QueueChildAction(() => list.AddChild(new Label(Symbol.Text($"[{dirname}]"), new(1)) { InputUI = c }));
					populate(dirname);

				};

			}

			populate(path);

			return (w, c);
		}

		public void Update()
		{
			foreach(var d in Drawables)
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
