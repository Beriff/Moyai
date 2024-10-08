﻿using Moyai.Abstract;
using Moyai.Impl.Input;
using Moyai.Impl.Math;

namespace Moyai.Impl.Graphics.Widgets
{
	public class ExpandingSelection : Widget
	{
		public Symbol[] MainText { get; set; }
		public List<string> Options { get; set; }

		public Action<string> OnOptionClick { get; set; }

		public delegate void EventDispatcher(string option);

		protected bool HasExpanded;

		public ExpandingSelection(Symbol[] text, string[] options, Action<string> onOptionClick, Vec2I size)
			: base(null, true, true, new(0), size, new(1))
		{
			MainText = text;
			Options = options.ToList();
			OnOptionClick = onOptionClick;
			HasExpanded = false;
		}
		public override Vec2I Size { get => new(MainText.Length, 1); set { } }
		public override Rect Bounds => new(Position, Position + new Vec2I(MainText.Length, 0));
		public override void Update()
		{
			base.Update();
			var mpos = InputHandler.CurrentMousePos;
			if (Hovered)
			{
				HasExpanded = true;
			}
			
			var bounds = Bounds;
			bounds.End = Bounds.End with { Y = Bounds.End.Y + Options.Count };

			if (!bounds.Contains(mpos))
			{
				HasExpanded = false;
			}

			if(HasExpanded && LocalInput.KeyState(Keys.MouseLeft) == InputType.JustReleased && mpos.Y != Position.Y)
				OnOptionClick(Options[mpos.Y - Position.Y - 1]);
		}

		public override void Draw(ConsoleBuffer buf)
		{
			if (!Visible) return;

			var mpos = InputHandler.CurrentMousePos;

			buf.BlitSymbString(MainText, Position);
			if(HasExpanded)
			{
				var selection_index = mpos.Y - Position.Y - 1;
				for (int i = 0; i < Options.Count; i++)
				{
					Symbol[] text;
					if(selection_index == i)
					{
						text = Symbol.Text(Options[i], new ConsoleColor((255, 255, 255), (0, 0, 0)));
					} else
					{
						text = Symbol.Text(Options[i], new ConsoleColor((0,0,0), (255, 255, 255)));
					}
					buf.BlitSymbString(text, Position + new Vec2I(0, i + 1));
				}
			}
		}
	}
}
