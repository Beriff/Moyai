using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moyai.Impl.Physics
{
	public struct Matrix3x3
	{
		public float[,] Values { get; set; }
		public readonly float this[int x, int y]
		{
			get { return Values[x, y]; }
			set { Values[x, y] = value; }
		}
		public Matrix3x3(
			float e00, float e10, float e20, 
			float e01, float e11, float e21, 
			float e02, float e12, float e22)
		{
			Values = new float[3, 3];
			Values[0,0] = e00; Values[1,0] = e10; Values[2,0] = e20;
			Values[0,1] = e01; Values[1,1] = e11; Values[2,1] = e21;
			Values[0,2] = e02; Values[1,2] = e12; Values[2,2] = e22;
		}
	}
}
