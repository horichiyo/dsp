﻿﻿using System;
using System.Collections.Generic;

namespace dsp21
{
	public class Complex
	{
		public double re;
		public double im;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:dsp21.Complex"/> class.
		/// </summary>
		/// <param name="r">実部</param>
		/// <param name="i">虚部</param>
		public Complex (double r, double i)
		{
			re = r;
			im = i;
		}
		public Complex()
		{
		}

		/// <summary>
		/// 足し算を計算する。
		/// </summary>
		/// <param name="c1">The first <see cref="dsp21.Complex"/> to add.</param>
		/// <param name="c2">The second <see cref="dsp21.Complex"/> to add.</param>
		/// <returns>The <see cref="T:dsp21.Complex"/> that is the sum of the values of <c>c1</c> and <c>c2</c>.</returns>
		public static Complex operator+ (Complex c1, Complex c2)
		{
			return new Complex (c1.re + c2.re, c1.im + c2.im);
		}

		/// <summary>
		/// 引き算を計算する。
		/// </summary>
		/// <param name="c1">The <see cref="dsp21.Complex"/> to subtract from (the minuend).</param>
		/// <param name="c2">The <see cref="dsp21.Complex"/> to subtract (the subtrahend).</param>
		/// <returns>The <see cref="T:dsp21.Complex"/> that is the <c>c1</c> minus <c>c2</c>.</returns>
		public static Complex operator- (Complex c1, Complex c2)
		{
			return new Complex (c1.re - c2.re, c1.im - c2.im);
		}

		/// <summary>
		/// かけ算を計算する。
		/// </summary>
		/// <param name="c1">The <see cref="dsp21.Complex"/> to multiply.</param>
		/// <param name="c2">The <see cref="dsp21.Complex"/> to multiply.</param>
		/// <returns>The <see cref="T:dsp21.Complex"/> that is the <c>c1</c> * <c>c2</c>.</returns>
		public static Complex operator* (Complex c1, Complex c2)
		{
			return new Complex (c1.re * c2.re - c1.im * c2.im, c1.im * c2.re + c1.re * c2.im);
		}

		/// <summary>
		/// 割り算を計算する。
		/// </summary>
		/// <param name="c1">The <see cref="dsp21.Complex"/> to divide (the divident).</param>
		/// <param name="c2">The <see cref="dsp21.Complex"/> to divide (the divisor).</param>
		/// <returns>The <see cref="T:dsp21.Complex"/> that is the <c>c1</c> / <c>c2</c>.</returns>
		public static Complex operator/ (Complex c1, Complex c2)
		{
			return new Complex ( (c1 * c2).re / (Math.Pow(c2.re, 2) + Math.Pow(c2.im, 2)), (c1 * c2).im / (Math.Pow(c2.re, 2) + Math.Pow(c2.im, 2)) );
		}

		/// <summary>
		/// 複素共役を求める。
		/// </summary>
		public static Complex operator~ (Complex c1)
		{
			return new Complex (c1.re, - c1.im);
		}

		/// <summary>
		/// Display this instance.
		/// </summary>
		public void Display()
		{
			Console.WriteLine("Re: "+this.re + ", Im: " + this.im);
		}

		/// <summary>
		/// 絶対値を表示する。
		/// </summary>
		/// <returns>絶対値</returns>
		public double Abs()
		{
			return Math.Pow ((Math.Pow (this.re, 2) + Math.Pow (this.im, 2)), 1 / 2);
		}

	}
}
