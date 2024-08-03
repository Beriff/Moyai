using Moyai.Abstract.Physics;
using Moyai.Impl.Graphics;
using Moyai.Impl.Math;

namespace Moyai.Impl.Physics
{
	public class Camera
	{
		public RectF Viewport { get; set; }
		public ConsoleBuffer Buffer { get; set; }
		public Vec3F Position { get; set; }
		public float ClipDistance { get; set; }

		public void Render(Body[] world)
		{
			//Console.WriteLine(Viewport.Size);
			for(float x = 0; x < Buffer.Size.X; x++)
			{
				for (float y = 0; y < Buffer.Size.Y; y++)
				{
					foreach(var body in world)
					{
						Ray ray = new(Position - Viewport[x / Buffer.Size.X, y / Buffer.Size.Y], Position);
						if (body.Intersection(ray) != null)
						{
							Buffer[(int)x,(int)y] = new('+', ConsoleColor.Default);
						}
					}
				}
			}
		}

		public Camera(Vec3F position, RectF viewport, Vec2 buf_size) 
		{ 
			Position = position;
			Viewport = viewport;
			Buffer = new(buf_size);
		}
	}
}
