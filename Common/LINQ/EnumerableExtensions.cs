using System;
using System.Collections.Generic;

namespace NTouchTypeTrainer.Common.LINQ
{
    public static class EnumerableExtensions
    {
        public static void ForEach<TElement>(this IEnumerable<TElement> enumerable, Action<TElement> action)
        {
            foreach (var element in enumerable)
            {
                action(element);
            }
        }
    }
}