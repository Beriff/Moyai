namespace Moyai.Impl
{
	using ColorPack = (byte R, byte G, byte B);
	public struct ConsoleColor
	{
		public static string Reset { get => "\u001b[0m"; }
		public static ConsoleColor OnlyBg(ColorPack bg) => new(bg, (0, 0, 0));
		public static ConsoleColor OnlyFg(ColorPack fg) => new((0, 0, 0), fg);

		public ColorPack Background { get; set; }
		public ColorPack Foreground { get; set; }

		public override string ToString() => 
			$"\x1b[38;2;{Foreground.R};{Foreground.G};{Foreground.B}m" +
			$"\x1b[48;2;{Background.R};{Background.G};{Background.B}m";

		public ConsoleColor(ColorPack bg, ColorPack fg) 
		{
			Background = bg;
			Foreground = fg;
		}
		public ConsoleColor(ColorPack col)
		{
			Background = Foreground = col;
		}

		public static ColorPack Contrast(ColorPack a)
		{
			return ((byte,byte,byte))((255 - a.R) / 2, (255 - a.G) / 2, (255 - a.B) / 2);
		}

		public static ConsoleColor Default { get => OnlyFg((255, 255, 255)); }

		public static implicit operator ConsoleColor((int, int, int) a)
		{
			return new(((byte)a.Item1, (byte)a.Item2, (byte)a.Item3));
		}
		public static ConsoleColor operator + (ConsoleColor rhs, ConsoleColor lhs)
		{
			return new ConsoleColor(
				((byte R, byte G, byte B))(rhs.Background.R + lhs.Background.R,
				rhs.Background.G + lhs.Background.G,
				rhs.Background.B + lhs.Background.B),
				((byte R, byte G, byte B))(rhs.Foreground.R + lhs.Foreground.R,
				rhs.Foreground.G + lhs.Foreground.G,
				rhs.Foreground.B + lhs.Foreground.B)
				);
		}
		public static ConsoleColor operator -(ConsoleColor rhs, ConsoleColor lhs)
		{
			return new ConsoleColor(
				((byte R, byte G, byte B))(rhs.Background.R - lhs.Background.R,
				rhs.Background.G - lhs.Background.G,
				rhs.Background.B - lhs.Background.B),
				((byte R, byte G, byte B))(rhs.Foreground.R - lhs.Foreground.R,
				rhs.Foreground.G - lhs.Foreground.G,
				rhs.Foreground.B - lhs.Foreground.B)
				);
		}

		public static ConsoleColor operator *(ConsoleColor rhs, ConsoleColor lhs)
		{
			return new ConsoleColor(
				((byte R, byte G, byte B))(rhs.Background.R * lhs.Background.R,
				rhs.Background.G * lhs.Background.G,
				rhs.Background.B * lhs.Background.B),
				((byte R, byte G, byte B))(rhs.Foreground.R * lhs.Foreground.R,
				rhs.Foreground.G * lhs.Foreground.G,
				rhs.Foreground.B * lhs.Foreground.B)
				);
		}

		public static ConsoleColor operator /(ConsoleColor rhs, ConsoleColor lhs)
		{
			return new ConsoleColor(
				((byte R, byte G, byte B))(rhs.Background.R / lhs.Background.R,
				rhs.Background.G / lhs.Background.G,
				rhs.Background.B / lhs.Background.B),
				((byte R, byte G, byte B))(rhs.Foreground.R / lhs.Foreground.R,
				rhs.Foreground.G / lhs.Foreground.G,
				rhs.Foreground.B / lhs.Foreground.B)
				);
		}
	}
}
