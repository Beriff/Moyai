using Moyai.Abstract;
using Moyai.Impl.Math;

using Tab = (string title, System.Collections.Generic.List<Moyai.Abstract.Widget> ui);

namespace Moyai.Impl.Graphics.Widgets
{
    public class TabContainer : Widget
    {
        public int ActiveTabIndex { get; set; } = 0;
		public List<Tab> Tabs { get; set; } = [];
		public int TabsOffset { get; set; } = 0;
		public Tab? ActiveTab { get => Tabs.Count != 0 ? Tabs[ActiveTabIndex] : null; }

		public List<Widget> AddTab(string tabname)
		{
			List<Widget> widgets = [];
			Tabs.Add((tabname, widgets));
			return widgets;
		}
		public Tab GetTab(string tabname) { return Tabs.Find(t => t.title == tabname); }
		public override void Draw(ConsoleBuffer buf)
		{
			int offset = 0;
			for (int i = TabsOffset; i < Tabs.Count; i++)
			{
				var tabtext = Symbol.Text($"x[{Tabs[i].title}]",
					i == ActiveTabIndex ?
					new ConsoleColor((127, 127, 127), (0, 0, 0)) :
					new ConsoleColor((0, 0, 0), (127, 127, 127))
					);
				buf.BlitSymbString(tabtext, new(offset, Position.Y));
				offset += tabtext.Length + 1;
			}
			buf.BlitSymbString(Symbol.Text("◄|►"), Position + new Vec2I(AbsoluteSize.X - 2, 0));
			ActiveTab?.ui.ForEach(w => w.Draw(buf));
		}
		public override void Update()
		{
			base.Update();
			ActiveTab?.ui.ForEach(w => w.Update());
		}

		public TabContainer(Vec2I pos, Vec2I size, string[] tabs)
			: base(null, true, true, pos, size, new(1,1) )
		{
			foreach(var tab in tabs) AddTab(tab);
		}
	}
}
