﻿﻿﻿﻿using System;
using System.Collections.Generic;

namespace dsp21
{
	public class Fft
	{
		private List<Complex> value;
		private Fft (List<Complex> target)
		{
			value = new List<Complex>(target);
		}
		//Do FFT : IFFT, true:false
		public static List<Complex> FftIfft(List<Complex> target, int dftpoint, Boolean mode)
		{
			Fft result = new Fft(target);
			int[] bit = new int[dftpoint];
			List<Complex> wnk = new List<Complex> ();
			bit = BitReverse(dftpoint);
			result.Data_replacement(bit,dftpoint);
			wnk = TwiddleFactor(dftpoint,mode);
			result.Butterfly(wnk, dftpoint, mode);
			return result.value;
		}
		private void Data_replacement(int[] bit,int n)
		{
			int i;
			Complex[] buff = new Complex[n];
			for(i = 0;i < n;i++){
				buff[bit[i]] = this.value[i];
			}
			for(i = 0;i < n;i++){
				this.value[i] = buff[i];
			}
		}
		//Make TwiddleFactor
		private static List<Complex> TwiddleFactor(int n,Boolean mode)
		{
			List<Complex> wnk = new List<Complex>();
			int j = 1;
			if (!mode) {
				j = -1;
			}
			for (int i = 0; i < n; i++) {
				Complex dummy = new Complex ();
				dummy.re = Math.Cos (2 * Math.PI * i / n);
				dummy.im = Math.Sin (-j * 2 * Math.PI * i / n);
				wnk.Add(dummy);
			}
			return wnk;
		}
		//BitReverse
		private static int[] BitReverse(int n)
		{
			int[] bit = new int[n];
			int r = (int)(Math.Log (n) / Math.Log (2.0) + 0.5);
			for(int i = 0;i < n;i++){
				for(int j = 0;j < r;j++){
					bit[i] += (i >> j & 1) << (r - j - 1);
				}
			}
			return bit;
		}
		//calculate Butterfly operation
		private void Butterfly(List<Complex> wnk,int n,Boolean mode)
		{
			int r = (int)(Math.Log (n) / Math.Log (2.0) + 0.5);
			int r_big=1, r_sma=n/2;
			int in1, in2, nk;
			Complex dummy;
			for(int i=0;i<r;i++){
				for(int j = 0; j < r_big; j++){
					for(int k = 0; k < r_sma; k++){
						in1 = r_big * 2 * k + j;
						in2 = in1 + r_big;
						nk = j * r_sma;
						dummy = this.value[in2] * wnk [nk];//complex_Multiply(in[in2], wnk[nk]);
						this.value[in2] = this.value[in1] - dummy;//complex_Sub(in[in1], dummy);
						this.value[in1] = this.value[in1] + dummy;//complex_Add(in[in1], dummy);
					}
				}
				r_big *= 2;
				r_sma /= 2;
			}
			if(!mode)
				for(int i = 0;i < n;i++){
				this.value[i].re /= n;
				this.value[i].im /= n; 
			}
		}
	}
}

