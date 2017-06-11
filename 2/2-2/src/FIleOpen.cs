using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace dsp22
{
	public static class FileOpen
	{
		/// <summary>
		/// テキストファイルの読み込み。
		/// </summary>
		/// <returns>double型の二次元配列</returns>
		/// <param name="filename">拡張子まで含めたファイルの名前</param>
		public static double[,] LoadFile(string filename)
		{
			List<List<string>> loadFileData = new List<List<string>>();
			using(StreamReader r = new StreamReader(@filename, Encoding.GetEncoding("utf-8")))
			{
				while(!r.EndOfStream)
				{
					var addData = new List<string>();
					string line = r.ReadLine();//一行ずつ読み込む
					string[] splitData = line.Split(',');//区切りで分割したものを配列に追加
					for(int i = 0; i < splitData.Length; i++)
					{
						addData.Add(splitData[i]);//追加用のList<string>の作成
					}
					loadFileData.Add(addData);//List<List<string>>のList<string>部分の追加
				}
			}

			int rowCount = loadFileData.Count;
			int colCount = rowCount;

			var value = new double[rowCount, colCount];

			for(int y = 0; y < rowCount; y++)
			{
				var src = loadFileData[y];
				for(int x = 0; x < colCount; x++)
				{
					value[y, x] = double.Parse(src[x]);
				}
			}

			return value;

		}

		/// <summary>
		/// 二次元配列をファイルに書き出す。
		/// </summary>
		/// <param name="value">書き出したいdouble型の二次元配列のデータ</param>
		/// <param name="filename">書き出したいファイルの名前</param>
		public static void WriteFile(double[,] value, string filename)
		{
			using(StreamWriter w = new StreamWriter(@filename))
			{
				for(int i = 0; i < value.GetLength(0); i++)
				{
					for(int j = 0; j < value.GetLength(0); j++)
					{
						w.Write(value[i, j] + ",");
					}
					w.Write("\n");
				}
			}
		}
	}
}
