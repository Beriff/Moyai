
using Moyai.Impl;

using System.Runtime.InteropServices;

namespace Moyai.Abstract
{
	public class Union<A,B>
	{
		protected A _A;
		protected B _B;

		public Type ActiveValue { get; private set; }
		public A ValueA { get => _A; set { _A = value; ActiveValue = typeof(A); } }
		public B ValueB { get => _B; set { _B = value; ActiveValue = typeof(B); } }
	}
}
