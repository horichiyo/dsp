using System;
using System.Collections.Generic;

namespace dsp22
{
	public static class Dct
	{
		/// <summary>
		/// 一次元のDCT変換を行う。
		/// </summary>
		/// <returns>DCT変換後のMatrix</returns>
		/// <param name="target">変換したい配列</param>
		public static Matrix DctIdct(double[] target, Boolean mode)
		{
			int N = target.Length;
			var dctArray = new double[N, N];
			var targetMatrix = new Matrix(target);

			//DCT行列を作成
			for(int k = 0; k < N; k++)
			{
				for(int n = 0; n < N; n++)
				{
					if(k == 0)
					{
						dctArray[k, n] = 1 / Math.Sqrt(N);
					}
					else
					{
						dctArray[k, n] = (Math.Sqrt(2.0 / N) * Math.Cos(((2.0 * n + 1.0) * k * Math.PI) / (double)(2.0 * N)));
					}
				}
			}

			//DCT or IDCTを行う
			var dctMatrix = new Matrix(dctArray);
			if(!mode) dctMatrix =  dctMatrix.Transpose();
			Matrix resultMatrix = dctMatrix * targetMatrix;

			return resultMatrix;
		}

		/// <summary>
		/// 二次元DCTもしくはIDCT変換を行う。
		/// </summary>
		/// <returns>DCT変換後のMatrix</returns>
		/// <param name="target">変換したい配列</param>
		public static Matrix DctIdct(double[,] target, Boolean mode)
		{
			int N = target.GetLength(0);
			var dctArray = new double[N, N];
			var targetMatrix = new Matrix(target);
			Matrix resultMatrix;

			//DCT行列を作成
			for(int k = 0; k < N; k++)
			{
				for(int n = 0; n < N; n++)
				{
					if(k == 0)
					{
						dctArray[k, n] = 1 / Math.Sqrt(N);
					}
					else
					{
						dctArray[k, n] = (Math.Sqrt(2.0 / N) * Math.Cos(((2.0 * n + 1.0) * k * Math.PI) / (double)(2.0 * N)));
					}
				}
			}

			//DCT or IDCTを行う
			var dctMatrix = new Matrix(dctArray);
			if(mode)
			{
				return resultMatrix = dctMatrix * targetMatrix * dctMatrix.Transpose();
			}
			else
			{
				return resultMatrix = dctMatrix.Transpose() * targetMatrix * dctMatrix;
			}
		}

		/// <summary>
		/// YCCDaraに対して二次元DCTもしくはIDCT変換を行う。
		/// </summary>
		/// <returns>Bmp.YccData[,]</returns>
		/// <param name="target">Bmp.YccData[,]</param>
		/// <param name="mode">True:DCT False:IDCT</param>
		public static Bmp.YccData[,] DctIdct(Bmp.YccData[,] target, Boolean mode)
		{
			int N = target.GetLength(0);
			var dctArray = new double[N, N];
			var resultYccData = new Bmp.YccData[N, N];
			var targetY = new Matrix(target, 1);
			var targetCr = new Matrix(target, 2);
			var targetCb = new Matrix(target, 3);
			Matrix resultY;
			Matrix resultCr;
			Matrix resultCb;

			//DCT行列を作成
			for(int k = 0; k < N; k++)
			{
				for(int n = 0; n < N; n++)
				{
					if(k == 0)
					{
						dctArray[k, n] = 1 / Math.Sqrt(N);
					}
					else
					{
						dctArray[k, n] = (Math.Sqrt(2.0 / N) * Math.Cos(((2.0 * n + 1.0) * k * Math.PI) / (double)(2.0 * N)));
					}
				}
			}

			//DCT or IDCTを行う
			var dctMatrix = new Matrix(dctArray);

			if(mode)
			{
				resultY = dctMatrix * targetY * dctMatrix.Transpose();
				resultCb = dctMatrix * targetCb * dctMatrix.Transpose();
				resultCr = dctMatrix * targetCr * dctMatrix.Transpose();
				for(int i = 0; i < N; i++)
				{
					for(int j = 0; j < N; j++)
					{
						resultYccData[i, j].Y = resultY.ShowsComponentOf(i,j);
						resultYccData[i, j].Cb = resultCb.ShowsComponentOf(i, j);
						resultYccData[i, j].Cr = resultCr.ShowsComponentOf(i, j);
					}
				}
			}
			else
			{
				resultY = dctMatrix.Transpose() * targetY * dctMatrix;
				resultCb = dctMatrix.Transpose() * targetCb * dctMatrix;
				resultCr = dctMatrix.Transpose() * targetCr * dctMatrix;
				for(int i = 0; i < N; i++)
				{
					for(int j = 0; j < N; j++)
					{
						resultYccData[i, j].Y = resultY.ShowsComponentOf(i, j);
						resultYccData[i, j].Cb = resultCb.ShowsComponentOf(i, j);
						resultYccData[i, j].Cr = resultCr.ShowsComponentOf(i, j);
					}
				}
			}
			return resultYccData;
		}
	}
}
