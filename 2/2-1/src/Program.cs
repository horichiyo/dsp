using System;
using System.Collections.Generic;

namespace dsp21
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			var result = new List<Complex>();
			var secondData = new List<Complex>();

			Console.WriteLine("自己相関関数もしくは相互相関関数をファイルに出力します。");
			Console.Write("自己相関関数：1 相互相関関数：0\n-> ");
			var choice = int.Parse(Console.ReadLine());
			Console.Write("読み込むファイル名\n-> ");
			var readFirstFileName = Console.ReadLine();
			List<Complex> firstData = File.ReadFile(readFirstFileName);
			if (choice == 0) {
				Console.Write("読み込むファイル名\n-> ");
				var readSecondFileName = Console.ReadLine();
				secondData = File.ReadFile(readSecondFileName);
			} else {
				secondData = File.ReadFile(readFirstFileName);
			}
			result = Correlation.CorrelationFunction(firstData, secondData);
			Console.Write("書き込むファイル名\n->");
			var writeFileName = Console.ReadLine();
			File.WriteFile(result, writeFileName);
		}
	}
}
