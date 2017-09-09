using System;
using System.Collections.Generic;

namespace dsp24
{
	class MainClass
	{
		public static void Main()
		{
			Console.WriteLine("実行すると指定した配列からウェーブレット係数とスケーリング係数を算出します。逆変換も行います。");
			Console.WriteLine();

			var target = new List<double>() { 10, 6, 2, 4, 8, 2, 6, 4 };
			Wavelet a = new Wavelet();
			a.ConvWavelet(target);
			Console.WriteLine("逆変換を行います。");
			a.ReConvWavelet();
		}
	}
}
