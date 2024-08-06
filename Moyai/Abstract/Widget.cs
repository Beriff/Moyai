using Moyai.Impl.Graphics;
using Moyai.Impl.Input;
using Moyai.Impl.Math;

using System.Linq;

namespace Moyai.Abstract
{
    public abstract class Widget : IDrawable
	{
		protected Widget? _Parent;
		protected bool _Active;
		protected bool _Visible;
		protected Vec2I _Position;
		protected Vec2I _Size;
		protected bool _Hovered;

		public virtual Widget? Parent { get => _Parent; set => _Parent = value; }
		public virtual bool Active { get => _Active; set => _Active = value; }
		public virtual bool Visible { get => _Visible; set => _Visible = value; }
		public virtual Vec2I Position { get => _Position; set => _Position = value; }
		public virtual Vec2I Size { get => _Size; set => _Size = value; }
		public virtual Vec2I RelativeSize { get; set; }
		public virtual string Name { get => GetHashCode().ToString() + GetType().Name; }
		public virtual Rect Bounds { get => new(Position, Position + AbsoluteSize - new Vec2I(1)); }
		public virtual bool Hovered { get => _Hovered; set => _Hovered = value; }
		public int ZLayer { get; set; }

		public Action OnHover { get; set; } = () => { };
		public Action OnHoverEnd { get; set; } = () => { };
		public Action OnClick { get; set; } = () => { };

		public Vec2I AbsoluteSize
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

		public virtual void Update() 
		{

			bool prev_hovered = Hovered;
			Hovered = Bounds.Contains(InputHandler.CurrentMousePos);
			if(!prev_hovered && Hovered) {
				OnHover();
			}
			else if (prev_hovered && Hovered) { OnHoverEnd(); }

			if(Hovered && InputHandler.KeyState(Keys.MouseLeft) == InputType.JustReleased)
			{
				OnClick();
			}
		}
		public abstract void Draw(ConsoleBuffer buf);

		protected Widget(Widget? parent, bool active, bool visible, Vec2I position, Vec2I size, Vec2I relsize)
		{
			Parent = parent;
			Active = active;
			Visible = visible;
			Position = position;
			Size = size;
			RelativeSize = relsize;

			OnClick = () => { if (this is IFocusable) { (this as IFocusable).Focused = !(this as IFocusable).Focused; } };
		}
	}

	public abstract class ContainerWidget : Widget, IFocusable
	{
		public virtual List<Widget> Children { get; private set; }
		public bool Focused { get; set; }

		public override Vec2I Position 
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

		public override void Update()
		{
			foreach(var child in Children) { child.Update(); }
			base.Update();
		}

		protected ContainerWidget(Widget? parent, bool active, bool visible, Vec2I position, Vec2I size, Vec2I relsize)
			: base(parent, active, visible, position, size, relsize)
		{
			Children = new();
		}
	}
}
