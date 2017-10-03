﻿﻿using System;
using System.Collections.Generic;

namespace dsp21
{
	public class Complex
	{
		public double re;
		public double im;
		public Complex (double r,double i)
		{
			re = r;
			im = i;
		}
		public Complex()
		{
		}
		//Complex Addition
		public static Complex operator+ (Complex c1, Complex c2)
		{
			return new Complex (c1.re + c2.re, c1.im + c2.im);
		}
		//Complex Subtraction
		public static Complex operator- (Complex c1, Complex c2)
		{
			return new Complex (c1.re - c2.re, c1.im - c2.im);
		}
		//Complex multiplication
		public static Complex operator* (Complex c1, Complex c2)
		{
			return new Complex (c1.re * c2.re - c1.im * c2.im, c1.im * c2.re + c1.re * c2.im);
		}
		//Complex division
		public static Complex operator/ (Complex c1, Complex c2)
		{
			return new Complex ( (c1 * c2).re /(Math.Pow(c2.re, 2) + Math.Pow(c2.im, 2)), (c1 * c2).im / (Math.Pow(c2.re, 2) + Math.Pow(c2.im, 2)) );
		}
		//Conversion to Conjugate complex
		public static Complex operator~ (Complex c1)
		{
			return new Complex (c1.re, - c1.im);
		}
		//DDisplay Complex
		public void Display()
		{
			Console.WriteLine("Re: "+this.re + ", Im: " + this.im);
		}
		//Calculate Complex Absolute value
		public double Abs()
		{
			return Math.Pow ((Math.Pow (this.re, 2) + Math.Pow (this.im, 2)), 1 / 2);
		}

	}
}

