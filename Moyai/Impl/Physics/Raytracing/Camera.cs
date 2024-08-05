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
        public ConsoleShader BackgroundShader { get; set; }
        public Vec3F RelativeRight { get => Look.Cross(new(0,1,0)); }
        public Vec3F Look
        {
            get
            {
                return (Viewport.Center - Position).Normalized;
            }
            /*set
            {
                var center = (Viewport.Center - Position).Normalized.Ro;
            }*/
        }

        public void Render(Body[] world)
        {
            //Console.WriteLine(Viewport.Size);
            for (float x = 0; x < Buffer.Size.X; x++)
            {
                for (float y = 0; y < Buffer.Size.Y; y++)
                {
					var screen_x = x / Buffer.Size.X;
					var screen_y = y / Buffer.Size.Y;
					Buffer[(int)x, (int)y] = BackgroundShader.Get(new(
                                    new((int)x, (int)y),
                                    Vec2F.Zero, Vec3F.Zero,
                                    new(screen_x, screen_y), null));
                    foreach (var body in world)
                    {
						
						Ray ray = new(Position - Viewport[screen_x, screen_y], Position);
                        var intersections = body.Intersection(ray);

						if (intersections != null)
                        {
                            var worldpos = intersections.OrderBy((i) => (Position - i).Length).First();
                            if (worldpos.Dot(Look) < 0 || (worldpos - Position).Length < ClipDistance)
                                continue;
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
            //ClipDistance = Look.Length / 2;
            Buffer = new(buf_size);
            Shader = new((_) => new('+', ConsoleColor.Default));
        }

        public void Move(Vec3F shift)
        {
            Position += shift;
            var v = Viewport;
            v.Position += shift;
            Viewport = v;
        }

        public void Rotate(Vec3F rotation)
        {
            var viewport = Viewport - Position;
            viewport = viewport.Rotate(rotation);
            viewport += Position;
            Viewport = viewport;
        }
    }
}
