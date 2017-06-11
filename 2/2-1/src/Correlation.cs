using System;
using System.Collections.Generic;

namespace dsp21
{
	public static class Correlation
	{
		/// <summary>
		/// 相関係数を計算する。
		/// </summary>
		/// <returns>相関係数をComplex型のListで返す。</returns>
		/// <param name="a">相関を計算したいデータ</param>
		/// <param name="b">相関を計算したいデータ</param>
		public static List<Complex> CorrelationFunction(List<Complex> a, List<Complex> b)
		{
			var value1 = new List<Complex>(a);
			var value2 = new List<Complex>(b);
			var result = new List<Complex>();
			value1 = AddZero(value1);
			value2 = AddZero(value2);
			value1 = Fft.FftIfft(value1, value1.Count, true);
			value2 = Fft.FftIfft(value2, value2.Count, true);
			for(int i = 0; i < value1.Count; i++)
			{
				result.Add(~value1[i] * value2[i]);
			}
			result = Fft.FftIfft(result, result.Count, false);
			foreach(Complex value in result)
			{
				value.re /= a.Count;
				value.im /= a.Count;
			}
			return result;
		}

		/// <summary>
		/// ゼロを2のN乗個になるように補完する。
		/// </summary>
		/// <returns>補完した後のListを返す。</returns>
		/// <param name="a">補完した後の配列を返す。</param>
		private static List<Complex> AddZero(List<Complex> a)
		{
			var result = new List<Complex>(a);
			string bin = Convert.ToString((a.Count * 2 - 1), 2);
			int length = (int)Math.Pow(2.0, (bin.Length)) - a.Count;
			for(int i = 0; i < length; i++)
			{
				result.Add(new Complex());
			}
			return result;
		}
	}
}
