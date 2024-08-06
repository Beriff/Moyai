using Moyai.Abstract;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moyai.Impl.Graphics
{
	public class DrawingLayer
	{
		public ConsoleBuffer Buffer { get; set; }
		public List<IDrawable> Drawables { get; set; }

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
