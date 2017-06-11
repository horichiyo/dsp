using System;
using System.Collections.Generic;
using System.IO;

namespace dsp21
{
	public static class File
	{
		/// <summary>
		/// ファイルを読み込む。
		/// </summary>
		/// <returns>The file data.</returns>
		/// <param name="filename">Filename</param>
		public static List<Complex> ReadFile(string filename)
		{
			var value = new List<Complex>();
			using(StreamReader r = new StreamReader(@filename))
			{
				string line;
				while((line = r.ReadLine()) != null)
				{
					value.Add(new Complex(Convert.ToDouble(line), 0.0));
				}
			}
			return value;
		}

		/// <summary>
		/// ファイルに書き出す。
		/// </summary>
		/// <param name="value">書き込みたいデータ</param>
		/// <param name="filename">ファイルネーム</param>
		public static void WriteFile(List<Complex> value, string filename)
		{
			using(StreamWriter w = new StreamWriter(@filename))
			{
				foreach(Complex data in value)
				{
					w.WriteLine(data.re);
				}
			}
		}
	}
}
