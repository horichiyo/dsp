using System;

namespace dsp23
{
	public static class Eigen
	{
		const double EPSILON = 0.0001;

		/// <summary>
		/// パワー法で固有値固有ベクトルを計算し，表示する
		/// </summary>
		/// <returns>返り値なし</returns>
		/// <param name="target">計算したい正方行列</param>
		public static void Power(double[,] target)
		{
			int N = target.GetLength(0);
			double[,] num = target;
			for(int i = 0; i < N; i++)
			{
				num = CalcEigen(num, i);
			}
		}

		/// <summary>
		/// 固有値固有ベクトルを計算して表示する。
		/// </summary>
		/// <returns>更新された二次元配列</returns>
		/// <param name="target">二次元配列</param>
		/// <param name="i">繰り返し回数</param>
		static double[,] CalcEigen(double[,] target, int i)
		{
			var targetMat = new Matrix(target);
			var firstEigenVector = new double[target.GetLength(0)];
			firstEigenVector[0] = 1;
			var pastEigenVector = new Matrix(firstEigenVector);
			Matrix approEigenVector, approEigenValue, eigenVector, subtractVector;
			double norm = 0;
			double eigenValue;

			do
			{
				approEigenVector = targetMat * pastEigenVector;
				approEigenValue = approEigenVector.Transpose() * pastEigenVector;
				eigenValue = approEigenValue.ShowsComponentOf(0,0);
				eigenVector = Matrix.NormalizeVector(approEigenVector);
				subtractVector = eigenVector - pastEigenVector;
				norm = Matrix.GetNorm(subtractVector);
				pastEigenVector = eigenVector;
			} while(norm > EPSILON);

			// 表示
			Console.WriteLine((i + 1) + ":");
			Console.WriteLine("EigenValue:");
			Console.WriteLine(eigenValue);
			Console.WriteLine("EigenVector:");
			eigenVector.Display();

			var calcArray = new double[target.GetLength(0)];

			for(int j = 0; j < target.GetLength(0); j++)
			{
				calcArray[j] = eigenValue * eigenVector.ShowsComponentOf(j,0);
			}

			var calcMat = new Matrix(calcArray);
			double[,] updateTarget = Matrix.ToconvertArray(targetMat - calcMat * eigenVector.Transpose());

			return updateTarget;
		}
	}
}
