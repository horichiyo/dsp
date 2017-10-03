using System;
using System.Collections.Generic;
using System.IO;


namespace dsp24
{
	public static class File
	{
		/// <summary>
		/// ファイルを読み込む。
		/// </summary>
		/// <returns>The file data.</returns>
		/// <param name="filename">Filename</param>
		public static List<double> ReadFile(string filename)
		{
			var value = new List<double>();
			using(StreamReader r = new StreamReader(@filename))
			{
				string line;
				while((line = r.ReadLine()) != null)
				{
					value.Add(Convert.ToDouble(line));
				}
			}
			return value;
		}



		/// <summary>
		/// ファイルに書き出す。
		/// </summary>
		/// <param name="value">書き込みたいデータ</param>
		/// <param name="filename">ファイルネーム</param>
		public static void WriteFile(List<double> value, string filename)
		{
			using(StreamWriter w = new StreamWriter(@filename))
			{
				foreach(double data in value)
				{
					w.WriteLine(data);
				}
			}
		}
	}
}
