using Moyai.Abstract;
using Moyai.Abstract.Physics;
using Moyai.Impl.Graphics;
using Moyai.Impl.Graphics.Widgets;
using Moyai.Impl.Input;
using Moyai.Impl.Math;
using Moyai.Impl.Physics.Raytracing;

using static Moyai.Impl.Physics.Raytracing.ConsoleShader;

namespace Moyai
{
    class ExampleApp : RenderContext
	{
		Camera camera;
		Body[] scene;
		float camspeed = 5;

		public ExampleApp() : base()
		{
			// create a physical scene
			var clip_dist = 10;
			var vp_size = (x: 80, y: 30);

			camera = new(
				/*position*/
				Vec3F.Zero,
				/*viewport*/
				new(
					new(clip_dist, vp_size.y / 2f, -vp_size.x / 2f),
					new(clip_dist, -vp_size.y / 2f, vp_size.x / 2f)
				),
				/*output buf size*/
				new(vp_size.x, vp_size.y)
			)
			{
				Shader = TextureUV(Texture.LoadBitmap(@"C:\Users\Maxim\Desktop\test.bmp"))
			};

			scene = [new Sphere(new Vec3F(clip_dist + 15, 0, 0), 15)];
		}
		public override void Update()
		{
			float mult = camspeed * (float)TimeDelta;
			if (InputHandler.KeyPressed(Keys.D))
				camera.Move(new Vec3F(0, 0, 1) * mult);
			if (InputHandler.KeyPressed(Keys.A))
				camera.Move(new Vec3F(0, 0, -1) * mult);
			if (InputHandler.KeyPressed(Keys.W))
				camera.Move(new Vec3F(1, 0, 0) * mult);
			if (InputHandler.KeyPressed(Keys.S))
				camera.Move(new Vec3F(-1, 0, 0) * mult);

			if (InputHandler.KeyPressed(Keys.ArrowRight))
				camera.Rotate(new Vec3F(0, 0, 0.01f));

			base.Update();
		}
		public override void Render()
		{
			camera.Buffer.Clear();

			// Draw FPS
			{
				var text = Symbol.Text(
					$"{1 / TimeDelta} FPS",
					Impl.ConsoleColor.OnlyFg((255, 255, 255)));
				camera.Buffer.BlitSymbString(text, Vec2I.Zero);
			}

			camera.Render(scene);
			camera.Buffer.Render();
			base.Render();
		}
	}

	internal class Program
	{
		static void Main(string[] args)
		{
			new ExampleApp().Start();
		}
	}
}