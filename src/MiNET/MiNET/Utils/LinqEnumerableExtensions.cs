using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#if NETSTANDARD

namespace MiNET.Utils
{
	public static class LinqEnumerableExtensions
	{
		public static TSource MaxBy<TSource, TProperty>(this IEnumerable<TSource> source, Func<TSource, TProperty> expr)
			=> source.OrderByDescending(expr).First();
		
		public static TSource MinBy<TSource, TProperty>(this IEnumerable<TSource> source, Func<TSource, TProperty> expr)
			=> source.OrderBy(expr).First();
	}
}

#endif
