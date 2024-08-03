using Moyai.Impl.Physics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moyai.Abstract.Physics
{
	public abstract class Body
	{
		public abstract Vec3F[]? Intersection(Ray ray);
	}
}
