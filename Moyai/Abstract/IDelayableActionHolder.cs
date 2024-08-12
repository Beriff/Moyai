using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moyai.Abstract
{
	public interface IDelayableActionHolder<T>
	{
		public List<Action<T>> ActionQueue { get; }
	}
	public interface IDelayableActionHolder
	{
		public List<Action> ActionQueue { get; }
	}
}
