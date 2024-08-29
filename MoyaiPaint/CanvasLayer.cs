using Moyai.Abstract;
using Moyai.Impl.Graphics;
using Moyai.Impl.Math;

namespace MoyaiPaint
{
	public class CanvasLayer
	{
		public ConsoleBuffer Buffer;
		public string Name;
		public bool Hidden;
		public List<CanvasLayer> Children;

		public bool IsGroup {  get => Children.Count != 0; }

		public CanvasLayer(string name, bool hidden, Vec2I size)
		{
			Name = name;
			Hidden = hidden;
			Buffer = new(size);
		}
	}
}
