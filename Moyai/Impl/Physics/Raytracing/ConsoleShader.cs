using Moyai.Impl.Graphics;
using Moyai.Impl.Math;

using static Moyai.Impl.Physics.Raytracing.ConsoleShader;

namespace Moyai.Impl.Physics.Raytracing
{
    public class ConsoleShader(Func<ShaderInput, Symbol> code)
	{
		public readonly struct ShaderInput
		{
			public ShaderInput(Vec2I pixelCoord, Vec2F uVCoord, Vec3F worldCoord, Vec2F screenCoord, Symbol? pixel)
			{
				PixelCoord = pixelCoord;
				UVCoord = uVCoord;
				WorldCoord = worldCoord;
				ScreenCoord = screenCoord;
				Pixel = pixel;
			}

			public readonly Vec2I PixelCoord { get; }
			public readonly Vec2F UVCoord { get; }
			public readonly Vec3F WorldCoord { get; }
			public readonly Vec2F ScreenCoord { get; }
			public readonly Symbol? Pixel { get; }
		}

		public static ConsoleShader TextureUV(Texture t)
		{
			return new((input) =>
			{
				return t.FromUV(input.UVCoord);
			});
		}

		protected Func<ShaderInput, Symbol> Code {  get; set; } = code;

		public Symbol Get(ShaderInput s) { return Code(s); }

		public static ConsoleShader operator +(ConsoleShader og, ConsoleShader nw)
		{
			return new(
				(input) => nw.Code(
					new(
						input.PixelCoord, 
						input.UVCoord, 
						input.WorldCoord,
						input.ScreenCoord,
						og.Code(input)
						)
					)
				);
		}
	}
}
