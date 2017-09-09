using System;
using System.Collections.Generic;

namespace dsp24
{
	public class Wavelet
	{
		/// <summary>
		/// Coefficient.
		/// </summary>
		public struct Coefficient
		{
			public List<double[]> resultScaling;
			public List<double[]> resultWavelet;
		}

		public Coefficient coef = new Coefficient();

		/// <summary>
		/// Wavelet変換を行い，レベルごとに表示する。
		/// </summary>
		/// <param name="target">Target.</param>
		public void ConvWavelet(List<double> target)
		{
			coef.resultScaling = new List<double[]>();
			coef.resultWavelet = new List<double[]>();

			int level = (int)Math.Log(target.Count, 2);
			var dummy = new List<double>(target);

			for(int i = 0; i < level; i++)
			{
				coef.resultScaling.Add(new double[(int)(target.Count / Math.Pow(2, i + 1))]);
				coef.resultWavelet.Add(new double[(int)(target.Count / Math.Pow(2, i + 1))]);

				for(int j = 0; j < target.Count / Math.Pow(2, i + 1); j++)
				{
					coef.resultScaling[i][j] = 1 / Math.Sqrt(2) * (dummy[j * 2] + dummy[j * 2 + 1]);
					coef.resultWavelet[i][j] = 1 / Math.Sqrt(2) * (dummy[j * 2] - dummy[j * 2 + 1]);
				}
				dummy = new List<double>(coef.resultScaling[i]);
			}
			this.ShowCoefficient(level);
		}

		/// <summary>
		/// 逆Wavelet変換を行い，Level0を表示する。
		/// </summary>
		public void ReConvWavelet()
		{
			int level = (int)Math.Log(coef.resultScaling[0].Length * 2, 2);
			var resultArray = new double[coef.resultScaling[0].Length * 2];

			for(int i = level - 1; i >= 1; i--)
			{
				for(int j = 0; j < coef.resultScaling[i].Length; j++)
				{
					coef.resultScaling[i - 1][j * 2] = 1 / Math.Sqrt(2) * (coef.resultScaling[i][j] + coef.resultWavelet[i][j]);
					coef.resultScaling[i - 1][j * 2 + 1] = 1 / Math.Sqrt(2) * (coef.resultScaling[i][j] - coef.resultWavelet[i][j]);
				}
			}

			// Level0の部分の計算
			for(int j = 0; j < coef.resultScaling[0].Length; j++)
			{
				resultArray[j * 2] = 1 / Math.Sqrt(2) * (coef.resultScaling[0][j] + coef.resultWavelet[0][j]);
				resultArray[j * 2 + 1] = 1 / Math.Sqrt(2) * (coef.resultScaling[0][j] - coef.resultWavelet[0][j]);
			}

			// 表示部
			foreach(var n in resultArray)
			{
				Console.Write(n+" ");
			}
			Console.WriteLine();

		}

		/// <summary>
		/// Shows the coefficient.
		/// </summary>
		/// <param name="level">Level.</param>
		private void ShowCoefficient(int level)
		{
			Console.WriteLine("スケーリング係数とウェーブレット係数を表示");
			for(int i = 0; i < level; i++)
			{
				for(int j = 0; j < coef.resultScaling[i].Length; j++)
				{
					Console.Write(coef.resultScaling[i][j]+" ");
				}
				for(int j = 0; j < coef.resultWavelet[i].Length; j++)
				{
					Console.Write(coef.resultWavelet[i][j] + " ");
				}
				Console.WriteLine();
			}
		}
	}
}
