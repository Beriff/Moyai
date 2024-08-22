using Moyai.Abstract;
using Moyai.Impl.Graphics;
using Moyai.Impl.Math;

namespace Moyai.Impl.Graphics.Widgets
{
    public class Frame : ContainerWidget
    {
        public Symbol[] Label { get; set; }
        public ConsoleColor Border { get; set; }
        public ConsoleColor BorderColor { get => Focused ?  Border / 2 : Border; }

        public override void Draw(ConsoleBuffer buf)
        {
			if (!Visible) return;

			//Frame's size does account for border. actual empty frame area is `Size - Vec2(1,1)`
			var size = AbsoluteSize;
            for (int x = Position.X; x < Position.X + size.X; x++)
            {
                for (int y = Position.Y; y < Position.Y + size.Y; y++)
                {
                    if(x == Position.X)
                    {
                        if (y == Position.Y)
                        {
							buf[x, y] = new Symbol('┌', BorderColor);
						}  
                        else if (y == Position.Y + size.Y - 1)
                        {
							buf[x, y] = new Symbol('└', BorderColor);
						}  
                        else
                        {
							buf[x, y] = new Symbol('│', BorderColor);
						}                        
					}
                    else if (x == Position.X + size.X - 1)
                    {
                        if (y == Position.Y)
                        {
							buf[x, y] = new Symbol('┐', BorderColor);
						}
                        else if (y == Position.Y + size.Y - 1)
                        {
							buf[x, y] = new Symbol('┘', BorderColor);
						}
                        else
					    {
							buf[x, y] = new Symbol('│', BorderColor);
						}
					}
                    else
                    {
						if (y == Position.Y || y == Position.Y + size.Y - 1)
					    {
							buf[x, y] = new Symbol('─', BorderColor);
						}
					}
                }
            }

            if (Label.Length > Size.X - 2)
                Label = Label[..(Size.X - 2)];

            for (int i = 0; i < Label.Length; i++)
            {
                buf[Position.X + 1 + i, Position.Y] = Label[i];
            }

            foreach (var c in Children)
                c.Draw(buf);
        }

        public Frame(Symbol[] label, ConsoleColor border, Vec2I size)
            : base(null, true, true, new(0), size, new(0))
        {
            Label = label;
            Border = border;
        }
    }
}
