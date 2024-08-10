using Moyai.Abstract;
using Moyai.Impl.Math;

public class VerticalList : ContainerWidget
{
	public int Padding { get; set; }

	public override Vec2I Size
	{
		get
		{
			int y = Position.Y;
			foreach (var item in Children)
			{
				y += (item.AbsoluteSize.Y + Padding);
			}
			return new(Children.Max((w) => w.Size.X), y);
		}
		set { }
	}

	public override void AddChild(Widget child)
	{
		if (Children.Count != 0)
		{
			var c = Children[^1];
			child.Position = child.Position with { Y = Padding + c.Position.Y + c.Size.Y };
		}
		else
		{
			child.Position = Position;
		}

		base.AddChild(child);
	}

	//public override Vec2I Position { get => _Position; set => _Position = value; }

	public VerticalList(Vec2I pos, Widget[]? widgets = null, int padding = 1)
		: base(null, true, true, pos, new(0), new(0))
	{
		if (widgets != null)
		{
			foreach (var widget in widgets)
				AddChild(widget);
		}
		Padding = padding;
	}
}