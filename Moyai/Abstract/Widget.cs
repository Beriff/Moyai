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

		public InputConsumer LocalInput { get; set; } = InputBus.Consumer("UI", "Master");
		public virtual Widget? Parent { get => _Parent; set => _Parent = value; }
		public virtual bool Active { get => _Active; set => _Active = value; }
		public virtual bool Visible { get => _Visible; set => _Visible = value; }
		public virtual Vec2I Position { get => _Position; set => _Position = value; }
		public virtual Vec2I Size { get => _Size; set => _Size = value; }
		public virtual Vec2I RelativeSize { get; set; }
		public virtual string Name { get; set; }
		public virtual Rect Bounds { get => new(Position, Position + AbsoluteSize - new Vec2I(1)); }
		public virtual bool Hovered { get => _Hovered; set => _Hovered = value; }
		public int ZLayer { get; set; }

		public Action<Widget> OnHover { get; set; } = (w) => { };
		public Action<Widget> OnHoverEnd { get; set; } = (w) => { };
		public Action<Widget> OnClick { get; set; } = (w) => { };
		public Action<Widget> OnUpdate { get; set; } = (w) => { };

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
			if (!Active) return;

			bool prev_hovered = Hovered;
			Hovered = Bounds.Contains(LocalInput.MousePos());
			if(!prev_hovered && Hovered) {
				OnHover(this);
			}
			else if (prev_hovered && !Hovered) { OnHoverEnd(this); }

			if(Hovered && LocalInput.KeyState(Keys.MouseLeft) == InputType.JustReleased)
			{
				OnClick(this);
				if (this is IFocusable)
					(this as IFocusable).Focused = true;
			} else if (LocalInput.KeyState(Keys.MouseLeft) == InputType.JustReleased)
			{
				if (this is IFocusable)
					(this as IFocusable).Focused = false;
			}

			OnUpdate(this);
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
			Name = GetHashCode().ToString() + GetType().Name;
		}
	}

	public abstract class ContainerWidget : Widget, IFocusable, IDelayableActionHolder
	{
		public virtual List<Widget> Children { get; private set; }
		public bool Focused { get; set; }
		public List<Action> ActionQueue { get; private set; } = [];

		public virtual void AddChild(Widget child)
		{
			Children.Add(child);
		}

		public virtual void AddChild(params Widget[] children)
		{
			foreach(var child in children) AddChild(child);
		}

		public virtual void RemoveChild(Widget child)
		{
			Children.Remove(child);
		}

		public Widget GetChild(string name) => Children.Find((x) => x.Name == name);

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
			if (!Active) return;

			foreach(var action in ActionQueue) { action(); }
			ActionQueue.Clear();
			foreach(var child in Children) { child.Update(); }
			base.Update();
		}

		public override void Draw(ConsoleBuffer buf)
		{
			foreach(var child in Children)
				child.Draw(buf);
		}

		protected ContainerWidget(Widget? parent, bool active, bool visible, Vec2I position, Vec2I size, Vec2I relsize)
			: base(parent, active, visible, position, size, relsize)
		{
			Children = new();
		}
	}
}
