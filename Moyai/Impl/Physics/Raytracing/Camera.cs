using Moyai.Abstract.Physics;
using Moyai.Impl.Graphics;
using Moyai.Impl.Math;

namespace Moyai.Impl.Physics.Raytracing
{
    public class Camera
    {
        public RectF Viewport { get; set; }
        public ConsoleBuffer Buffer { get; set; }
        public Vec3F Position { get; set; }
        public float ClipDistance { get; set; }
        public ConsoleShader Shader { get; set; }

        public void Render(Body[] world)
        {
            //Console.WriteLine(Viewport.Size);
            for (float x = 0; x < Buffer.Size.X; x++)
            {
                for (float y = 0; y < Buffer.Size.Y; y++)
                {
                    foreach (var body in world)
                    {
						var screen_x = x / Buffer.Size.X;
						var screen_y = y / Buffer.Size.Y;
						Ray ray = new(Position - Viewport[screen_x, screen_y], Position);
                        var intersections = body.Intersection(ray);

						if (intersections != null)
                        {
                            var worldpos = intersections.OrderBy((i) => (Position - i).Length).First();
                            Buffer[(int)x, (int)y] = Shader.Get(
                                new(
                                    new((int)x, (int)y), 
                                    body.UV(worldpos), worldpos, 
                                    new(screen_x, screen_y), null)
                                );
                        }
                    }
                }
            }
        }

        public Camera(Vec3F position, RectF viewport, Vec2I buf_size)
        {
            Position = position;
            Viewport = viewport;
            Buffer = new(buf_size);
            Shader = new((_) => new('+', ConsoleColor.Default));
        }

        public void Move(Vec3F shift)
        {
            Position += shift;
            Viewport = new(Viewport.Start + shift, Viewport.End + shift);
        }

        public void Rotate(Vec3F rotation)
        {
            var viewport = Viewport - Position;
            viewport.Start *= rotation.Rotation;
            viewport.End *= rotation.Rotation;
            viewport += Position;
            Viewport = viewport;
        }
    }
}
