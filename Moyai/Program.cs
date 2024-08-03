using Moyai.Abstract;
using Moyai.Abstract.Physics;
using Moyai.Impl.Graphics;
using Moyai.Impl.Graphics.Widgets;
using Moyai.Impl.Input;
using Moyai.Impl.Math;
using Moyai.Impl.Physics;

namespace Moyai
{
    class ExampleApp : RenderContext
	{
		Camera camera;
		Body[] scene;

		public ExampleApp() : base()
		{
			// create a physical scene
			var clip_dist = 10;
			var vp_size = (x: 50, y: 10);

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
			);

			scene = [new Sphere(new Vec3F(clip_dist + 5, 0, 0), 5)];
		}
		public override void Update()
		{
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
				camera.Buffer.BlitSymbString(text, Vec2.Zero);
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