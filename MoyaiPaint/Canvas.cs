using Moyai.Abstract;
using Moyai.Impl.Graphics;
using Moyai.Impl.Math;


namespace MoyaiPaint
{
	class Canvas : Widget
	{
		public List<CanvasLayer> Layers { get; private set; } = [];
		public ConsoleBuffer Viewport { get; set; }
		public Vec2I Offset;

		public override void Draw(ConsoleBuffer buf)
		{
			void d(CanvasLayer l)
			{
				if(l.IsGroup)
					l.Children.ForEach(d);
				else
				{
					l.Buffer.Blit(Viewport, Offset + new Vec2I(1, 1));
				}
					
			}
			Layers.ForEach(d);
			
			// emulate "transparency mask", showing transparent pixels
			for (int x = 0; x < Viewport.Size.X; x++)
			{
				for(int y = 0; y < Viewport.Size.Y; y++)
				{
					if (Viewport[x, y].Transparent)
						Viewport[x, y] = new('░', new((0,0,0),(127, 127, 127)));
				}
			}

			Viewport.Blit(buf, Position + new Vec2I(1,0) );
			Viewport.Clear();
		}

		public Canvas(Vec2I pos, Vec2I size) :
			base(null, true, true, pos, size, new(1,1))
		{
			Offset = Vec2I.Zero;
		}

		protected static CanvasLayer DeserializeLayer(Vec2I size, DeserializedLayer l)
		{
			var layer = new CanvasLayer(l.name, l.hidden, size) { Children = [] };
			if (l.children.Length != 0)
			{
				foreach(var child in l.children)
					layer.Children.Add(DeserializeLayer(size, child));
			} else
			{
				layer.Buffer = new(size);
				for(int y = 0; y < size.Y; y++)
				{
					for (int x = 0; x < size.X; x++)
					{
						if(y >= l.data.Length || x >= l.data[y].Length)
						{
							layer.Buffer[x, y] = new() { Transparent = true };
							continue;
						}

						layer.Buffer[x, y] =
							new(
								l.data[y][x].character[0],
								new(
									(
										(byte)l.data[y][x].color[0],
										(byte)l.data[y][x].color[1],
										(byte)l.data[y][x].color[2]
									),
									(
										(byte)l.data[y][x].color[3],
										(byte)l.data[y][x].color[4],
										(byte)l.data[y][x].color[5]
									)
									)
								);
					}
				}
			}
			return layer;
		}

		public Canvas(Vec2I pos, DeserializedSprite sprite)
			: this(pos, new Vec2I(0))
		{
			Size = new(sprite.size[0], sprite.size[1]);
			Viewport = new(Size);
			foreach(var dlayer in sprite.layers)
			{
				Layers.Add(DeserializeLayer(Size, dlayer));
			}
		}
	}
}
