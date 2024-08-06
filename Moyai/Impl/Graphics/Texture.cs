using Moyai.Impl.Math;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moyai.Impl.Graphics
{
    public class Texture
    {
        public ConsoleColor[,] Bitmap;
        public Vec2I Size { get; private set; }

        protected Texture(ConsoleColor[,] bitmap, Vec2I size)
        {
            Bitmap = bitmap;
            Size = size;
        }

        public Symbol FromUV(Vec2F uv)
        {
            return new('▓', Bitmap[(int)(uv.X * (Size.X - 1)), (int)(uv.Y * (Size.Y - 1))]);
        }

        public static Texture LoadBitmap(string path)
        {
            Bitmap image = new(path);
            var data = new ConsoleColor[image.Width, image.Height];
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    var px = image.GetPixel(x, y);
                    data[x, y] = new((px.R, px.G, px.B));
                }
            }
            return new(data, new(image.Width, image.Height));
        }
    }
}
