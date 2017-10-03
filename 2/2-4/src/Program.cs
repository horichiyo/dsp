using System;
using System.Collections.Generic;

namespace dsp24
{
	class MainClass
	{
		public static void Main()
		{
			Console.WriteLine("実行すると指定した配列からウェーブレット係数とスケーリング係数を算出します。逆変換も行います。");
			Console.WriteLine("またウェーブレット変換によってノイズ除去を行います。\n");
			Console.WriteLine("0:ウェーブレット・スケーリング係数の算出と逆変換");
			Console.WriteLine("1:ウェーブレット変換によるノイズ除去");

			var tag = int.Parse(Console.ReadLine());

			if(tag == 0)
			{
				var target = new List<double>() { 10, 6, 2, 4, 8, 2, 6, 4 };
				Wavelet result = new Wavelet();
				result.ConvWavelet(target);
				Console.WriteLine("逆変換を行います。");
				result.ReConvWavelet();
			}
			else if(tag == 1)
			{
				// ファイルから入力 -> リストに追加
				String filename = "wtsamp.txt";
				Wavelet result = new Wavelet();
				List<double> target = File.ReadFile(filename);


				// ウェーブレット変換
				result.ConvWavelet(target);
				// 閾値で置き換え
				// 逆変換，ファイル出力
				result.OutReConvWavelet();
			}
			else
			{
				Console.WriteLine("入力が間違っています。");
			}
		}
	}
}
