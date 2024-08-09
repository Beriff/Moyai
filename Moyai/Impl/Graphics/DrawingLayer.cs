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

			//throw new Exception(InputBus.GetBus(c)[1].InputReceived.ToString());
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
