using System;
using System.Collections;
using System.Collections.Generic;

namespace NTouchTypeTrainer.Common.LINQ
{
    public static class EnumerableExtensions
    {
        public static void ForEach<TElement>(this IEnumerable<TElement> enumerable, Action<TElement> action)
        {
            if (enumerable != null)
            {
                foreach (var element in enumerable)
                {
                    action(element);
                }
            }
        }

        public static void ForEach(this IEnumerable enumerable, Action<object> action)
        {
            if (enumerable != null)
            {
                foreach (var element in enumerable)
                {
                    action(element);
                }
            }
        }
    }
}