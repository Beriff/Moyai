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
			var w = new Window("Select file", size);
			Drawables.Add(w);

			var c = new InputConsumer(w.Name + "_input", InputBus.HigherPriority("UI"))
			{ Blocking = true };
			InputBus.AddConsumer("UI", c);
			w.InputUI = c;

			//w.AddChild()

			var scroll = ScrollFrame.NoFrame(size - new Vec2I(1), new(1));
			scroll.InputUI = c;
			w.AddChild(scroll);
			w.AddChild(new Button("Select", new((255, 255, 255)), new((127, 127, 127)), new(8, 1))
			{ Position = size - new Vec2I(10, 1), InputUI = c });

			var list = new VerticalList(new(1), padding: 0) { InputUI = c };
			scroll.AddChild(list);

			list.AddChild(new Label(Symbol.Text($"[{path}]"), new(1)) { InputUI = c });
			list.AddChild(
				new Label(
					new Symbol[] { new Symbol('D', new((255, 255, 255), (0, 0, 0))) }.Concat(Symbol.Text("..")).ToArray(), 
					new(1))
				{ InputUI = c }
				);

			Action<Widget> hovertoggle = (Widget self) =>
			{
				var l = self as Label;
				l.Text = Symbol.Text(
					Symbol.StringFromText(l.Text),
					(i) =>
						{
							return new ConsoleColor((255,255,255)) - l.Text[i].Color;
						}
					);
			};

			foreach(var s in Directory.GetFileSystemEntries(path))
			{
				FileAttributes fa = File.GetAttributes(s);
				Symbol q;
				if (fa.HasFlag(FileAttributes.Directory))
				{
					q = new Symbol('D', new((255, 255, 255), (0, 0, 0)));
					list.AddChild(
					new Label(
						new Symbol[] { q }.Concat(Symbol.Text(" " + new DirectoryInfo(s).Name)).ToArray(),
					new(1))
					{ InputUI = c, OnHover = hovertoggle, OnHoverEnd = hovertoggle }
				);
				}
				else
				{
					q = new Symbol('F', new((255, 255, 255), (0, 0, 0)));
					list.AddChild(
					new Label(
						new Symbol[] { q }.Concat(Symbol.Text(" " + Path.GetFileName(s))).ToArray(),
					new(1))
					{ InputUI = c, OnHover = hovertoggle, OnHoverEnd = hovertoggle }
				);
				}
			}

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
