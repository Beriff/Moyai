﻿namespace Moyai.Impl
{
	using ColorPack = (byte R, byte G, byte B);
	public class ConsoleColor
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

		public static ConsoleColor Default { get => OnlyFg((255, 255, 255)); }
	}
}
