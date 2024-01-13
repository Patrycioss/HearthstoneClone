using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Utils
{
	/// <summary>
	/// Utility for manipulation of collections.
	/// </summary>
	public static class CollectionUtils
	{
		/// <summary>
		/// Randomizes a list.
		/// </summary>
		/// <param name="list">A list of elements.</param>
		/// <typeparam name="T">Type of the elements.</typeparam>
		/// <returns>A randomized version of the given list.</returns>
		[NotNull]
		public static List<T> RandomizeList<T>([NotNull] List<T> list)
		{
			List<T> newList = new List<T>();
			
			while (list.Count != 0)
			{
				int index = Random.Range(0, list.Count);
				T card = list[index];
				newList.Add(card);
				list.Remove(card);
			}

			return newList;
		}
	}
}