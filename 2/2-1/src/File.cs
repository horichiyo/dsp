using System;
using System.Collections.Generic;
using System.IO;

namespace dsp21
{
	public class File
	{
		public File()
		{
		}
		public static List<Complex> ReadFile(string filename)
		{
			List<Complex> value = new List<Complex>();
			using (StreamReader r = new StreamReader(@filename))
			{
				string line;
				while ((line = r.ReadLine()) != null){
					value.Add(new Complex(Convert.ToDouble(line), 0.0));
				}
			}
			return value;
		}
		public static void WriteFile(List<Complex> value, string filename)
		{
			using (StreamWriter w = new StreamWriter(@filename))
			{
				foreach (Complex data in value){
					w.WriteLine(data.re);
				}
			}
		}
	}
}
