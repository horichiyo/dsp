using System;

namespace dsp23
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			double[,] target = {
				{2, 1, 3},
				{1, 2, 3},
				{3, 3, 20}};

			Eigen.Power(target);
		}
	}
}
