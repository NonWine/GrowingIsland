using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Extensions
{
	public static class ListExtension
	{
		public static List<Transform> GetNearestPoints<T>(this List<Transform> list, int count, Vector3 position)
		{
			List<Transform> nearestPoints = new();

			// сортируем список точек по расстоянию от переданной позиции
			list.Sort((a, b) => Vector3.Distance(position, a.position).CompareTo(Vector3.Distance(position, b.position)));

			// добавляем ближайшие точки в список
			for (int i = 0; i < count; i++)
			{
				if (i < list.Count)
				{
					nearestPoints.Add(list[i]);
				}
				else
				{
					break;
				}
			}

			return nearestPoints;
		}

		public static T GetRandomItem<T>(this IList<T> list)
		{
			T result = list[UnityEngine.Random.Range(0, list.Count)];
			return result;
		}
		

		public static void Shuffle<T>(this IList<T> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				list.Swap(i, UnityEngine.Random.Range(i, list.Count));
			}
		}

		public static void Swap<T>(this IList<T> list, int i, int j)
		{
			var temp = list[i];
			list[i] = list[j];
			list[j] = temp;
		}

		public static UInt64 Sum(this IEnumerable<UInt64> source)
		{
			return source.Aggregate((x, y) => x + y);
		}

		public static void ReverseMatrix<T>(this T[,] array)
		{
			int rows = array.GetLength(0);
			int columns = array.GetLength(1);
			T[,] result = new T[rows, columns];
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					result[rows - i - 1, columns - j - 1] = array[i, j];
				}
			}
			array = result;
		}


		public static T[,] ReverseMatrixR<T>(T[,] array)
		{
			int rows = array.GetLength(0);
			int columns = array.GetLength(1);
			T[,] result = new T[rows, columns];
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					result[rows - i - 1, columns - j - 1] = array[i, j];
				}
			}
			return result;
		}

		public static T[,] CreateMatrix<T>(T[] array)
		{
			int len = array.Length;

			var rows = (int)(len / Math.Sqrt(len));
			if (rows <= 0) rows = 1;

			int col = array.Length / rows;
			var matrix = new T[rows, col];

			for (int i = 0; i < len; i++)
			{
				matrix[i / rows, i % rows] = array[i];
			}
			return matrix;
		}

		public static T[,] CreateMatrix<T>(this T[] array, int row, int column)
		{
			int k = 0;
			T[,] mass = new T[row, column];
			for (int i = 0; i < row; i++)
			{
				for (int j = 0; j < column; j++)
				{
					mass[i, j] = array[k];
					k++;
				}
			}
			return mass;
		}
		
		public static List<T> GetNeighbour<T>(this T[,] arr, int row, int column)
		{
			int rows = arr.GetLength(0);
			int columns = arr.GetLength(1);

			List<T> returned = new List<T>();

			for (int j = row - 1; j <= row + 1; j++)
				for (int i = column - 1; i <= column + 1; i++)
					if (i >= 0 && j >= 0 && i < columns && j < rows && !(j == row && i == column))
						returned.Add(arr[j, i]);

			return returned;
		}

		public static Tuple<int, int> FindIndexes<T>(this T[,] matrix, T value)
		{
			int w = matrix.GetLength(0); // width
			int h = matrix.GetLength(1); // height

			for (int x = 0; x < w; ++x)
			{
				for (int y = 0; y < h; ++y)
				{
					if (matrix[x, y].Equals(value))
						return Tuple.Create(x, y);
				}
			}

			return Tuple.Create(-1, -1);
		}

		public static T MaxBy<T, U>(this IEnumerable<T> source, Func<T, U> selector)
	  where U : IComparable<U>
		{
			if (source == null) throw new ArgumentNullException("source");
			bool first = true;
			T maxObj = default(T);
			U maxKey = default(U);
			foreach (var item in source)
			{
				if (first)
				{
					maxObj = item;
					maxKey = selector(maxObj);
					first = false;
				}
				else
				{
					U currentKey = selector(item);
					if (currentKey.CompareTo(maxKey) > 0)
					{
						maxKey = currentKey;
						maxObj = item;
					}
				}
			}
			if (first) throw new InvalidOperationException("Sequence is empty.");
			return maxObj;
		}
	}
}

