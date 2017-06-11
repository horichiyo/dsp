using System;
using System.Collections.Generic;

namespace dsp22
{
	class MainClass
	{
		public static void Main()
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

			for(int i = 0; i < bih.biHeight;i++)
			{
				for(int j = 0; j < bih.biWidth;j++)
				{
					result[i, j] = yccDctData[i, j].Y;
				}
			}

			FileOpen.WriteFile(result, "afterYccYData.csv");
		}
	}
}
