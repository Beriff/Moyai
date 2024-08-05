namespace Moyai.Impl.Math
{
	public static class MathHelper
	{
		public static int Lerp(int a, int b, float p)
		{
			return (int)(a + (b - a) * p);
		}
		public static float Lerp(float a, float b, int p)
		{
			return a + (b-a) * p;
		}
		public static float Pairing(float a, float b)
		{
			return 0.5f * (a + b) * (a + b + 1) + b;
		}
	}
}
