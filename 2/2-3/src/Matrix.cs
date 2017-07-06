using System;

namespace dsp23
{
	public class Matrix
	{
		double[,] m;
		int numOfRow;
		int numOfColumn;

		/// <summary>
		/// Matrixの初期化を行う。
		/// </summary>
		public Matrix()
		{
			this.numOfRow = 0;
			this.numOfColumn = 0;
		}

		/// <summary>
		/// 一次元配列 input の内容で、行列（インスタンス変数）を初期化する。
		/// </summary>
		/// <param name="input">double型の一次元配列</param>
		public Matrix(double[] input)
		{
			this.numOfRow = input.Length;
			this.numOfColumn = 1;
			this.m = new double[this.GetNumOfRow(), this.GetNumOfColumn()];
			for(int i = 0; i < this.numOfRow; i++) this.m[i, 0] = input[i];
		}

		/// <summary>
		/// 二次元配列 input の内容で、行列（インスタンス変数）を初期化する。 input[0][0] -> 行列の０行０列目
		/// </summary>
		/// <param name="input">double型の二次元配列</param>
		public Matrix(double[,] input)
		{
			this.numOfRow = input.GetLength(0);
			this.numOfColumn = input.GetLength(1);
			this.m = new double[this.GetNumOfRow(), this.GetNumOfColumn()];
			this.m = input;
		}

		/// <summary>
		/// 行数を取得する。
		/// </summary>
		/// <returns>Matrixの行数</returns>
		public int GetNumOfRow()
		{
			return this.numOfRow;
		}

		/// <summary>
		/// 列数を取得する。
		/// </summary>
		/// <returns>Matrixの列数</returns>
		public int GetNumOfColumn()
		{
			return this.numOfColumn;
		}

		/// <summary>
		/// 指定された要素に対応する値を返す。
		/// </summary>
		/// <returns>Matrixの要素に対応する値</returns>
		/// <param name="rowIndex">指定したい行</param>
		/// <param name="columnIndex">指定したい列</param>
		public double ShowsComponentOf(int rowIndex, int columnIndex)
		{
			// 指定した範囲が存在しない場合
			if(rowIndex > this.GetNumOfRow() || columnIndex > this.GetNumOfColumn())
			{
				Console.WriteLine("指定する要素は存在しません。");
				Environment.Exit(0);
			}
			return this.m[rowIndex, columnIndex];
		}

		/// <summary>
		/// Matrixの内容を表示する。
		/// </summary>
		public void Display()
		{
			for(int i = 0; i < this.GetNumOfRow(); i++)
			{
				Console.Write("[");
				for(int h = 0; h < this.GetNumOfColumn(); h++)
				{
					Console.Write(" " + this.ShowsComponentOf(i, h) + " ");
				}
				Console.WriteLine("]");
			}
			Console.WriteLine("");
		}

		/// <summary>
		/// ベクトルAとBの内積 A・Bの結果を返す。
		/// </summary>
		/// <returns>内積の結果</returns>
		/// <param name="target">かけられる数</param>
		public double GetInnerProduct(Matrix target)
		{
			double num = 0.0;
			if(this.GetNumOfColumn() == 1) Environment.Exit(0);
			if(target.GetNumOfColumn() != 1 && this.GetNumOfColumn() == target.GetNumOfColumn())
			{
				for(int i = 0; i < this.GetNumOfColumn(); i++) num += this.ShowsComponentOf(0, i) * target.ShowsComponentOf(0, i);
			}
			else if(target.GetNumOfColumn() == 1 && this.GetNumOfColumn() == target.GetNumOfRow())
			{
				for(int i = 0; i < this.GetNumOfColumn(); i++) num += this.ShowsComponentOf(0, i) * target.ShowsComponentOf(i, 0);
			}
			else
			{
				Console.WriteLine("内積計算が可能な条件を満たしていません。");
				Environment.Exit(0);
			}
			return num;
		}

		/// <summary>
		/// 行列同士、もしくは行列とベクトルとの積を計算する。
		/// </summary>
		/// <param name="thisMatrix">The <see cref="dsp23.Matrix"/> to multiply.</param>
		/// <param name="target">The <see cref="dsp23.Matrix"/> to multiply.</param>
		/// <returns>The <see cref="T:dsp22.Matrix"/> that is the <c>thisMatrix</c> * <c>target</c>.</returns>
		public static Matrix operator* (Matrix thisMatrix, Matrix target)
		{
			Matrix result;
			var culc = new double[thisMatrix.GetNumOfRow(), target.GetNumOfColumn()];
			// 掛ける行列の列数と掛けられる行列の行数が等しいなら
			if(thisMatrix.GetNumOfColumn() == target.GetNumOfRow())
			{
				for(int i = 0; i < thisMatrix.GetNumOfRow(); i++)
				{
					for(int h = 0; h < target.GetNumOfColumn(); h++)
					{
						for(int j = 0; j < thisMatrix.GetNumOfColumn(); j++)
						{
							culc[i, h] += thisMatrix.ShowsComponentOf(i, j) * target.ShowsComponentOf(j, h);
						}
					}
				}
			}
			else
			{
				Console.WriteLine("要素数が計算できる組み合わせとなっていません。");
				Environment.Exit(0);
			}
			result = new Matrix(culc);
			return result;
		}

		/// <summary>
		/// 二つのmatrixの乗算処理が実行可能かどうかを判定する。
		/// </summary>
		/// <returns>乗算処理が実行可能かどうか</returns>
		/// <param name="target">判定したい行列</param>
		public Boolean Multipliable(Matrix target)
		{
			if(this.GetNumOfColumn() == target.GetNumOfRow()) return true;
			Console.WriteLine("要素数が計算できる組み合わせとなっていません。");
			return false;
		}

		public static Matrix operator -(Matrix thisMatrix, Matrix target)
		{
			Matrix result;
			var culc = new double[thisMatrix.GetNumOfRow(), target.GetNumOfColumn()];

			if(thisMatrix.GetNumOfColumn() == target.GetNumOfColumn() && thisMatrix.GetNumOfRow() == target.GetNumOfRow())
			{
				for(int i = 0; i < thisMatrix.GetNumOfRow();i++)
				{
					for(int j = 0; j < thisMatrix.GetNumOfColumn();j++)
					{
						culc[i, j] = thisMatrix.ShowsComponentOf(i, j) - target.ShowsComponentOf(i, j);
					}
				}
			}
			else
			{
				Console.WriteLine("要素数が計算できる組み合わせとなっていません。");
				Environment.Exit(0);
			}
			result = new Matrix(culc);
			return result;
		}

		/// <summary>
		/// Matrixを転置する。
		/// </summary>
		/// <returns>転置したMatrix</returns>
		public Matrix Transpose()
		{
			Matrix result;
			var trans = new double[this.GetNumOfColumn(), this.GetNumOfRow()];

			for(int i = 0; i < this.GetNumOfRow(); i++)
			{
				for(int h = 0; h < this.GetNumOfColumn(); h++)
				{
					trans[h, i] = this.ShowsComponentOf(i, h);
				}
			}
			return result = new Matrix(trans);
		}

		/// <summary>
		/// 度からラジアンに変換する。
		/// </summary>
		/// <returns>ラジアン</returns>
		/// <param name="theta">角度</param>
		public static double ConvertIntoRadian(double theta)
		{
			double rad = theta * Math.PI / 180.0;
			return rad;
		}

		/// <summary>
		/// Matrixを回転する。
		/// </summary>
		/// <returns>回転後のMatrix</returns>
		/// <param name="theta">角度</param>
		public Matrix Rotate(double theta)
		{
			double rad = ConvertIntoRadian(theta);
			double[,]
				rotat = {
					{Math.Cos(rad), -Math.Sin(rad)},
					{Math.Sin(rad), Math.Cos(rad)}};
			var rotate = new Matrix(rotat);

			if(this.GetNumOfRow() == 1) this.Transpose();
			return rotate = rotate * this;
		}

		/// <summary>
		/// ベクトルの大きさを求める。
		/// </summary>
		/// <returns>ベクトルの大きさ</returns>
		/// <param name="target">Matrix型（1列）</param>
		public static double GetNorm(Matrix target)
		{
			double sum = 0.0;
			for(int i = 0; i < target.GetNumOfRow();i++)
			{
				sum += target.ShowsComponentOf(i, 0) * target.ShowsComponentOf(i, 0);
			}
			return Math.Sqrt(sum);
		}

		/// <summary>
		/// ベクトルを正規化する。
		/// </summary>
		/// <returns>Matrix</returns>
		/// <param name="target">一列のMatrix</param>
		public static Matrix NormalizeVector(Matrix target)
		{
			var calcArray = new double[target.GetNumOfRow()];
			double N = GetNorm(target);

			for(int i = 0; i < target.GetNumOfRow();i++)
			{
				calcArray[i] = target.ShowsComponentOf(i, 0) / N;
			}
			var result = new Matrix(calcArray);
			return result;
		}

		/// <summary>
		/// Matrixを二次元配列に変換する。
		/// </summary>
		/// <returns>二次元配列</returns>
		/// <param name="target">Matrix</param>
		public static double[,] ToconvertArray(Matrix target)
		{
			var result = new double[target.GetNumOfRow(), target.GetNumOfColumn()];

			for(int i = 0; i < target.GetNumOfRow();i++)
			{
				for(int j = 0; j < target.GetNumOfColumn();j++)
				{
					result[i, j] = target.ShowsComponentOf(i, j);
				}
			}
			return result;
		}
	}
}
