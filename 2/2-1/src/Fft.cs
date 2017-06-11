using System;
using System.Collections.Generic;

namespace dsp21
{
	public class Fft
	{
		private readonly List<Complex> value;

		private Fft(List<Complex> target)
		{
			value = new List<Complex>(target);
		}

		/// <summary>
		/// FFTもしくはIFFTを行う。
		/// </summary>
		/// <returns>結果を返す。</returns>
		/// <param name="target">Complex型のList</param>
		/// <param name="ftpoint">FFTの点数</param>
		/// <param name="mode">True:FFT False:IFFT</param>
		public static List<Complex> FftIfft(List<Complex> target, int ftpoint, Boolean mode)
		{
			Fft result = new Fft(target);
			var bit = new int[ftpoint];
			var wnk = new List<Complex>();
			bit = BitReverse(ftpoint);
			result.DataReplasement(bit, ftpoint);
			wnk = TwiddleFactor(ftpoint, mode);
			result.Butterfly(wnk, ftpoint, mode);
			return result.value;
		}

		/// <summary>
		/// ビット反転の位置に合わせて配列の中身を入れ替える。
		/// </summary>
		/// <param name="bit">ビット</param>
		/// <param name="n">点数</param>
		private void DataReplasement(int[] bit, int n)
		{
			var buff = new Complex[n];
			for(int i = 0; i < n; i++) buff[bit[i]] = this.value[i];
			for(int i = 0; i < n; i++) this.value[i] = buff[i];
		}

		/// <summary>
		/// 回転子を作成する。
		/// </summary>
		/// <returns>Comlex型のListとして，回転子を返す。</returns>
		/// <param name="n">点数</param>
		/// <param name="mode">True:FFT False:IFFT</param>
		private static List<Complex> TwiddleFactor(int n, Boolean mode)
		{
			var wnk = new List<Complex>();
			int j = 1;
			if(!mode)
			{
				j = -1;
			}
			for(int i = 0; i < n; i++)
			{
				var dummy = new Complex()
				{
					re = Math.Cos(2 * Math.PI * i / n),
					im = Math.Sin(-j * 2 * Math.PI * i / n)
				};
				wnk.Add(dummy);
			}
			return wnk;
		}

		/// <summary>
		/// ビット反転を行う。
		/// </summary>
		/// <returns>int型の配列を返す。</returns>
		/// <param name="n">点数</param>
		private static int[] BitReverse(int n)
		{
			var bit = new int[n];
			int r = (int)(Math.Log(n) / Math.Log(2.0) + 0.5);
			for(int i = 0; i < n; i++)
			{
				for(int j = 0; j < r; j++)
				{
					bit[i] += (i >> j & 1) << (r - j - 1);
				}
			}
			return bit;
		}

		/// <summary>
		/// バタフライ演算を行う。
		/// </summary>
		/// <param name="wnk">回転子</param>
		/// <param name="n">点数</param>
		/// <param name="mode">True:FFT False:IFFT</param>
		private void Butterfly(List<Complex> wnk, int n, Boolean mode)
		{
			int r = (int)(Math.Log(n) / Math.Log(2.0) + 0.5);
			int r_big = 1, r_sma = n / 2;
			int in1, in2, nk;
			Complex dummy;

			for(int i = 0; i < r; i++)
			{
				for(int j = 0; j < r_big; j++)
				{
					for(int k = 0; k < r_sma; k++)
					{
						in1 = r_big * 2 * k + j;
						in2 = in1 + r_big;
						nk = j * r_sma;
						dummy = this.value[in2] * wnk[nk];
						this.value[in2] = this.value[in1] - dummy;
						this.value[in1] = this.value[in1] + dummy;
					}
				}
				r_big *= 2;
				r_sma /= 2;
			}
			if(!mode)
			{
				for(int i = 0; i < n; i++)
				{
					this.value[i].re /= n;
					this.value[i].im /= n;
				}
			}
		}
	}
}