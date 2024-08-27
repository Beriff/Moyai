
using Moyai.Abstract;
using Moyai.Impl.Input;
using Moyai.Impl.Math;

namespace Moyai.Impl.Graphics.Widgets
{
	public class InputBox : Frame
	{
		protected int _Cursor = 0;
		public string Text { get; set; }
		public Predicate<char>? Validate { get; set; }
		public static Predicate<char> NumericValidator
		{
			get => (c) => int.TryParse(c.ToString(), out int _);
		}
		public int Cursor { get => _Cursor; set
			{
				_Cursor = value;
				if (Cursor - Shift > AbsoluteSize.X) Shift++;
				else if (Cursor - Shift < 0) Shift--;
			}
		}
		public int Shift { get; set; } = 0;
		public override Vec2I Size { get => new(_Size.X, 3); set => _Size = value; }
		public override void Draw(ConsoleBuffer buf)
		{
			int end_i = System.Math.Clamp(Text.Length, 0, Shift + AbsoluteSize.X - 2);
			base.Draw(buf);
			buf.BlitSymbString(Symbol.Text(Text[Shift..end_i], (index) => 
				Cursor - Shift - 1 == index && Focused ? new((255, 255, 255), (0, 0, 0)) : ConsoleColor.Default
			), Position + new Vec2I(1,1) );
		}
		public bool Empty { get => Text == ""; }
		public override void Update()
		{
			base.Update();

			if (!Focused) return;
			foreach(var key in LocalInput.KeysOfState(InputType.JustPressed))
			{
				switch(key)
				{
					case Keys.ArrowLeft:
						Cursor = System.Math.Max(Cursor - 1, 0);
						break;
					case Keys.ArrowRight:
						Cursor = System.Math.Min(Cursor + 1, Text.Length - 1);
						break;
					case Keys.Backspace:
						if (Text.Length == 0) break;
						Cursor--;
						Text = Text[0..^1];
						break;
					default:
						char? k = InputHandler.LetterKey(key);
						if (k == null) break;
						string s = k.ToString()!;
						if(Validate == null || (Validate != null && Validate((char)k)) )
						{
							Text = Text.Insert(Cursor, s!);
							Cursor++;
						}
						break;
				}
			}
		}
		public InputBox(string label, int size, Predicate<char>? validate = null)
			: base(Symbol.Text(label), ConsoleColor.Default, new(size, 3))
		{
			Text = "";
			Validate = validate;
		}
	}
}
