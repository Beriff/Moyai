using System.Text;

namespace Moyai.Impl.Graphics
{
    public struct Symbol
    {
        public char Character { get; set; }
        public ConsoleColor Color { get; set; }
        public bool Transparent { get; set; } = false;

        public override string ToString() => $"{Color}{Character}{ConsoleColor.Reset}";

        public Symbol(char character, ConsoleColor color)
        {
            Character = character;
            Color = color;
        }

        public static Symbol[] Text(string text, ConsoleColor? color = null)
        {
            color ??= ConsoleColor.OnlyFg((255, 255, 255));
            var symbols = new Symbol[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                symbols[i] = new(text[i], (ConsoleColor)color);
            }
            return symbols;
        }

        public static Symbol[] Text(string text, Func<int, ConsoleColor> f)
        {
            var symbols = new Symbol[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                symbols[i] = new(text[i], f(i));
            }
            return symbols;
        }

        public static string StringFromText(Symbol[] text)
        {
            var str = new StringBuilder();
            foreach(Symbol symbol in text) { str.Append(symbol.Character); }
            return str.ToString();
        }
    }
}
