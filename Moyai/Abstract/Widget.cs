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
		protected Vec2 _Position;
		protected Vec2 _Size;
		protected bool _Hovered;

		public virtual Widget? Parent { get => _Parent; set => _Parent = value; }
		public virtual bool Active { get => _Active; set => _Active = value; }
		public virtual bool Visible { get => _Visible; set => _Visible = value; }
		public virtual Vec2 Position { get => _Position; set => _Position = value; }
		public virtual Vec2 Size { get => _Size; set => _Size = value; }
		public virtual Vec2 RelativeSize { get; set; }
		public virtual string Name { get => GetHashCode().ToString() + GetType().Name; }
		public Rect Bounds { get => new(Position, Position + AbsoluteSize - new Vec2(1)); }
		public virtual bool Hovered { get => _Hovered; set => _Hovered = value; }

		public Action OnHover { get; set; } = () => { };
		public Action OnHoverEnd { get; set; } = () => { };
		public Action OnClick { get; set; } = () => { };

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

		public virtual void Update() 
		{

			bool prev_hovered = Hovered;
			Hovered = Bounds.Contains(InputHandler.MousePos());
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

		protected Widget(Widget? parent, bool active, bool visible, Vec2 position, Vec2 size, Vec2 relsize)
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

		public override void Update()
		{
			foreach(var child in Children) { child.Update(); }
			base.Update();
		}

		protected ContainerWidget(Widget? parent, bool active, bool visible, Vec2 position, Vec2 size, Vec2 relsize)
			: base(parent, active, visible, position, size, relsize)
		{
			Children = new();
		}
	}
}
