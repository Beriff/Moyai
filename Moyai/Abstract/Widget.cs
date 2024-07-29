using Moyai.Impl.Graphics;
using Moyai.Impl.Math;

using System.Linq;

namespace Moyai.Abstract
{
    public abstract class Widget : IDrawable
	{
		protected Widget? _Parent;
		protected bool _Active;
		protected bool _Visible;
		protected Vec2 _Position;
		protected Vec2 _Size;

		public virtual Widget? Parent { get => _Parent; set => _Parent = value; }
		public virtual bool Active { get => _Active; set => _Active = value; }
		public virtual bool Visible { get => _Visible; set => _Visible = value; }
		public virtual Vec2 Position { get => _Position; set => _Position = value; }
		public virtual Vec2 Size { get => _Size; set => _Size = value; }
		public virtual Vec2 RelativeSize { get; set; }
		public virtual string Name { get => GetHashCode().ToString() + GetType().Name; }
		public Vec2 AbsoluteSize
		{
			get
			{
				if(Parent == null) { return Size; }
				else
				{
					return Parent.AbsoluteSize * RelativeSize + Size;
				}
			}
		}

		public virtual void Update() { }
		public abstract void Draw(ConsoleBuffer buf);

		protected Widget(Widget? parent, bool active, bool visible, Vec2 position, Vec2 size, Vec2 relsize)
		{
			Parent = parent;
			Active = active;
			Visible = visible;
			Position = position;
			Size = size;
			RelativeSize = relsize;
		}
	}

	public abstract class ContainerWidget : Widget, IFocusable
	{
		public virtual List<Widget> Children { get; private set; }
		public bool Focused { get => false; set 
			{
				foreach(var child in Children)
				{
					if(child is IFocusable) 
					{
						(child as IFocusable).Focused = value;
						return;
					}
				}
			} 
		}

		public override Vec2 Position 
		{ 
			get => _Position;
			set
			{
				if (Children == null) { _Position += value; return; }
				var shift = value - _Position;
				_Position += shift;
				foreach(var child in Children) { child.Position += shift; }
			}
		}

		protected ContainerWidget(Widget? parent, bool active, bool visible, Vec2 position, Vec2 size, Vec2 relsize)
			: base(parent, active, visible, position, size, relsize)
		{
			Children = new();
		}
	}
}
