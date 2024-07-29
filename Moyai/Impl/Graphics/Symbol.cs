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
                symbols[i] = new(text[i], color);
            }
            return symbols;
        }

        public static Symbol[] Text(string text, Func<int, ConsoleColor> f)
        {
            var symbols = new Symbol[text.Length];
            for (int i = text.Length; i < text.Length;)
            {
                symbols[i] = new(text[i], f(i));
            }
            return symbols;
        }
    }
}
