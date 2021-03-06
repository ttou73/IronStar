﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fusion {

	public static class LinqExtensions {
		
		public static T SelectMaxOrDefault<T>(this IEnumerable<T> list, Func<T, float> selector)
		{
			if (!list.Any()) return default(T);
			return list.Aggregate((acc, next) => (selector(acc) > selector(next)) ? acc : next);
		}

		
		public static bool SelectMaxOrDefault<T>(this IEnumerable<T> list, Func<T, float> selector, out T result )
		{
			result = default(T);
			if (!list.Any()) return false;
			result = list.Aggregate((acc, next) => (selector(acc) > selector(next)) ? acc : next);
			return true;
		}


		public static T SelectMinOrDefault<T>(this IEnumerable<T> list, Func<T, float> selector)
		{
			if (!list.Any()) return default(T);
			return list.Aggregate((acc, next) => (selector(acc) < selector(next)) ? acc : next);
		}


		public static IEnumerable<TSource> DistinctBy<TSource, TKey> (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			HashSet<TKey> seenKeys = new HashSet<TKey>();
			foreach (TSource element in source)
			{
				if (seenKeys.Add(keySelector(element)))
				{
					yield return element;
				}
			}
		}

		public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int count)
		{
			return source.Skip(Math.Max(0, source.Count() - count));
		}
	}
}
