using System;
using System.Collections.Generic;

namespace dsp22
{
	class MainClass
	{
		public static void Main()
		{
			Console.WriteLine("BMP画像のRGBに対して，YCC変換を行い，その値に対して二次元DCTを行います。");
			Console.WriteLine("Debugフォルダに処理を行いたい画像を入れて実行して下さい。");
			Console.WriteLine("afterYccYData.csvが書き出されます。");

			Console.WriteLine("0:BMP画像のRGB->YCCと二次元DCT");
			Console.WriteLine("1:一次元DCT");

			var tag = int.Parse(Console.ReadLine());

			if(tag == 0)
			{
				string fileName = "samp.bmp";
				Bmp.Load(fileName, out Bmp.BITMAPFILEHEADER bfh, out Bmp.BITMAPINFOHEADER bih, out byte[] bitData);
				var rgbData = new Bmp.RgbData[bih.biHeight, bih.biWidth];
				var yccData = new Bmp.YccData[bih.biHeight, bih.biWidth];
				var yccDctData = new Bmp.YccData[bih.biHeight, bih.biWidth];
				var result = new double[bih.biHeight, bih.biWidth];

				rgbData = Bmp.ToConvertBitmap(bitData, bih);
				yccData = Bmp.ToConvertYcc(rgbData);
				yccDctData = Dct.DctIdct(yccData, true);

				for(int i = 0; i < bih.biHeight; i++)
				{
					for(int j = 0; j < bih.biWidth; j++)
					{
						result[i, j] = yccDctData[i, j].Y;
					}
				}

				FileOpen.WriteFile(result, "afterYccYData.csv");
			}
			else if (tag == 1)
			{
				Matrix resultMat = new Matrix();
				List<double> target = FileOpen.ReadFile("a_wav_dct.txt");
				resultMat = Dct.DctIdct(target.ToArray(), false);
				//resultMat.Display();
				FileOpen.WriteFile(Matrix.ToConvertList(resultMat), "out.csv");
			}
			else
			{
				Console.WriteLine("不正な値");
			}
		}
	}
}
