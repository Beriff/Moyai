using Moyai.Abstract;
using Moyai.Impl.Math;

namespace Moyai.Impl.Graphics.Widgets
{
	public class HorizontalList : ContainerWidget
	{
		public int Padding { get; set; }

		public override Vec2I Size 
		{ 
			get
			{
				int x = Position.X;
				foreach(var item in Children)
				{
					x += (item.AbsoluteSize.X + Padding);
				}
				return new(x, Children.Max((w) => w.Size.Y));
			}
			set { } 
		}

		public override void AddChild(Widget child)
		{
			if(Children.Count != 0)
			{
				var c = Children[^1];
				child.Position = child.Position with { X = Padding + c.Position.X + c.Size.X };
			} else
			{
				child.Position = Position;
			}
			
			base.AddChild(child);
		}

		public HorizontalList(Vec2I pos, Widget[]? widgets = null, int padding = 1)
			: base(null, true, true, pos, new(0), new(0))
		{
			if(widgets != null)
			{
				foreach(var widget in widgets)
					AddChild(widget);
			}
			Padding = padding;
		}
	}
}
