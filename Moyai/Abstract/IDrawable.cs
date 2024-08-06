using Moyai.Impl.Graphics;

namespace Moyai.Abstract
{
    public interface IDrawable
	{
		public int ZLayer { get; set; }
		public void Draw(ConsoleBuffer buf);
	}
}
