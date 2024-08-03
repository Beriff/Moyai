using Moyai.Abstract;
using Moyai.Impl.Math;
using System.Text;

namespace Moyai.Impl.Graphics
{
    public class ConsoleBuffer
    {
        public Vec2 Size { get; private set; }
        public Symbol[,] Grid { get; private set; }
        public Rect Area { get => new(Vec2.Zero, Size); }
        private int LinearSize { get => Size.X * Size.Y + Size.X - 1; }

        public void Render()
        {
            StringBuilder str = new(LinearSize);
            for (int y = 0; y < Size.Y; y++)
            {
                for (int x = 0; x < Size.X; x++)
                {
                    str.Append(Grid[x, y].Transparent ? ' ' : Grid[x, y]);
                }
                str.Append('\n');
            }
            Console.Write(str.ToString());
            Console.SetCursorPosition(0, 0);
        }

        public void Blit(ConsoleBuffer accepting, Vec2 pos)
        {
            for (int y = 0; y < Size.Y; y++)
            {
                for (int x = 0; x < Size.X; x++)
                {
                    if (
                        x + pos.X <= accepting.Size.X &&
                        y + pos.Y <= accepting.Size.Y &&
                        !this[x, y].Transparent
                        )
                    {
                        accepting[x + pos.X - 1, y + pos.Y - 1] = this[x, y];
                    }
                }
            }
        }

        public void BlitSymbString(Symbol[] str, Vec2 pos)
        {
            for (int i = 0; i < str.Length; i++)
            {
                this[pos.X + i, pos.Y] = str[i];
            }
        }

        public void Fill(Symbol s)
        {
            for (int x = 0; x < Size.X; x++)
            {
                for (int y = 0; y < Size.Y; y++)
                {
                    this[x, y] = s;
                }
            }
        }

        public void Clear()
        {
            for (int x = 0; x < Size.X; x++)
            {
                for (int y = 0; y < Size.Y; y++)
                {
                    this[x, y] = new Symbol(' ', ConsoleColor.OnlyFg((0, 0, 0)));
                }
            }
        }

        public Symbol this[int x, int y]
        {
            get => Grid[x, y];
            set
            {
                if (x >= 0 && x < Size.X && y >= 0 && y < Size.Y)
                {
                    Grid[x, y] = value;
                }
            }
        }

        public ConsoleBuffer(Vec2 size)
        {
            Size = size;
            Grid = new Symbol[size.X, size.Y];
            Fill(new(' ', ConsoleColor.Default));
        }
    }
}
