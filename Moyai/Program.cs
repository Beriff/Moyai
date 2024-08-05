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
			var vp_size = (x: 70, y: 50);

			camera = new(
				/*position*/
				Vec3F.Zero,
				/*viewport*/
				new(
					new(clip_dist, vp_size.y / 2f, -vp_size.x / 2f),
					new(vp_size.x, vp_size.y)
				),
				/*output buf size*/
				new(vp_size.x, vp_size.y)
			)
			{
				Shader = TextureUV(Texture.LoadBitmap(@"C:\Users\Maxim\Desktop\test.bmp")),
				BackgroundShader = new((input) =>
				{
					if (new Random((int)MathHelper.Pairing(input.PixelCoord.X, input.PixelCoord.Y)).NextSingle() < 0.03f)
						return new('*', new((0, 0, 0), (255, 255, 255)));
					else
						return new(' ', new((0, 0, 0)));
				})
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
				camera.Move(camera.Look * mult);
			if (InputHandler.KeyPressed(Keys.S))
				camera.Move(-camera.Look * mult);

			if (!InputHandler.KeyPressed(Keys.Esc))
				camera.Rotate(new Vec3F(InputHandler.MouseDelta.Y / 25f, 0, InputHandler.MouseDelta.X / 25f) * mult);
			//if (!InputHandler.KeyPressed(Keys.Esc))
				//camera.Rotate(new Vec3F(InputHandler.MouseDelta.Y / 15f, 0, 0) * mult);

			base.Update();

			if (!InputHandler.KeyPressed(Keys.Esc))
				InputHandler.SnapCursor();
		}
		public override void Render()
		{
			camera.Buffer.Clear();

			

			camera.Render(scene);
			

			// Draw FPS
			{
				var text = Symbol.Text(
					$"{Math.Truncate(1 / TimeDelta)} FPS",
					Impl.ConsoleColor.OnlyFg((255, 255, 255)));
				camera.Buffer.BlitSymbString(text, Vec2I.Zero);
			}
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