using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Plugins.OdinUtils.Editor
{
	public static class EditorUtility
	{
		public static void DrawWithColor(Action drawAction, Color color)
		{
			Color originalColor = GUI.color;
			GUI.color = color;

			drawAction?.Invoke();

			GUI.color = originalColor;
		}

		public static void ResizeTableMatrix<T>
		(
			ref T[,] matrix, int newRowsCount, int newColumnsCount, IEnumerable<T> existingEntries
		)
		{
			List<T> currentEntries = existingEntries.ToList();
			matrix = new T[newRowsCount, newColumnsCount];

			for (int i = 0; i < newRowsCount; i++)
			{
				for (int j = 0; j < newColumnsCount; j++)
				{
					if (currentEntries.Count == 0)
					{
						break;
					}

					matrix[i, j] = currentEntries[0];
					currentEntries.RemoveAt(0);
				}
			}
		}
	}
}