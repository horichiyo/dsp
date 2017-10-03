using System;
using System.Collections.Generic;

namespace dsp21
{
	public class Correlation
	{
		public Correlation()
		{
		}
		//Calculate the correlation
		public static List<Complex> CorrelationFunction(List<Complex> a, List<Complex> b)
		{
			List<Complex> value1 = new List<Complex>(a);
			List<Complex> value2 = new List<Complex>(b);
			List<Complex> result = new List<Complex>();
			value1 = AddZero(value1);
			value2 = AddZero(value2);
			value1 = Fft.FftIfft(value1, value1.Count, true);
			value2 = Fft.FftIfft(value2, value2.Count, true);
			for (var i = 0; i < value1.Count; i++){
				result.Add(~value1[i] * value2[i]);
			}
			result = Fft.FftIfft(result, result.Count, false);
			foreach(Complex value in result){
				value.re /= a.Count;
				value.im /= a.Count;
			}
			return result;
		}
		// Increase the array of 0
		private static List<Complex> AddZero(List<Complex> a)
		{
			int length;
			string bin;
			List<Complex> result = new List<Complex>(a);
			bin = Convert.ToString((a.Count * 2 - 1), 2);
			length = (int)Math.Pow(2.0, (bin.Length)) - a.Count;
			for (int i = 0; i < length; i++){
				result.Add(new Complex());
			}
			return result;
		}
	}
}