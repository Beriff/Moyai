
using System.Linq;

using Moyai.Impl.Graphics;
using Moyai.Impl.Math;

namespace Moyai.Impl.Input
{
	public class InputConsumer
	{
		public int Priority { get; set; }
		public string Name {  get; set; }
		public bool Blocking { get; set; }
		public bool InputReceived { get; set; }

		public InputConsumer(string name, int priority)
		{
			Name = name;
			Priority = priority;
			Blocking = false;
			InputReceived = true;
		}

		public InputType KeyState(Keys key)
		{
			if (InputReceived)
				return InputHandler.KeyState(key);
			else
				return InputType.Released;
		}

		public bool KeyPressed(Keys key)
		{
			if (InputReceived)
				return InputHandler.KeyPressed(key);
			else
				return false;
		}

		public Vec2I MousePos()
		{
			if (InputReceived)
				return InputHandler.MousePos();
			else
				return new(-1);
		}

		public Vec2I MousePos(ConsoleBuffer buf)
		{
			if (InputReceived)
				return InputHandler.MousePos(buf);
			else
				return new(-1);
		}

		public Vec2I MouseDelta
		{
			get { if(InputReceived) { return InputHandler.MouseDelta; } else { return new(0); } }
		}
	}
	public static class InputBus
	{
		public static Dictionary<string, List<InputConsumer>> Consumers { get; } = new();

		public static int HigherPriority(string busname)
		{
			return Consumers[busname][0].Priority + 1;
		}

		public static InputConsumer Consumer(string busname, string conname)
		{
			return Consumers[busname].Find((a) => a.Name == conname);
		}

		static InputBus()
		{
			Consumers["UI"] = new()
			{
				new("Master", 0)
			};
		}

		public static void AddConsumer(string name, InputConsumer consumer)
		{
			Consumers[name].Add(consumer);
			Consumers[name] = Consumers[name].OrderByDescending((x)=>x.Priority).ToList();
			UpdateBus(Consumers[name]);
		}
		public static void RemoveConsumer(InputConsumer c)
		{
			var list = (from consumer in Consumers
						where consumer.Value.Contains(c)
						select consumer).ToList()[0];

			list.Value.Remove(c);
			Consumers[list.Key] = list.Value.OrderByDescending((x) => x.Priority).ToList();
			UpdateBus(list.Value);
		}
		public static List<InputConsumer>? GetBus(InputConsumer c)
		{
			var list = (from consumer in Consumers
						where consumer.Value.Contains(c)
						select consumer.Value).ToList();
			return list.Count != 0 ? list[0] : null;
		}
		private static void UpdateBus(List<InputConsumer> bus)
		{
			bool blocked = false;
			foreach (var con in bus)
			{
				if (!blocked)
				{
					con.InputReceived = true;
					blocked = con.Blocking;
				}
				else
				{
					con.InputReceived = false;
				}
			}
		}
		public static void ToggleBlocking(InputConsumer consumer, bool status)
		{
			consumer.Blocking = status;
			UpdateBus(GetBus(consumer)!);
		}
	}
}
