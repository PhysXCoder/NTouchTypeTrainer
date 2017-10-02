using System.Collections.Generic;
using System.Linq;

namespace NTouchTypeTrainer.Common.LINQ
{
    public static class ListExtensions
    {
        public static IList<TElement> WithoutLast<TElement>(this IList<TElement> list, TElement lastElementMandatoryMatch)
        {
            var shortenedList = new List<TElement>(list);

            if (shortenedList.Count > 0 && shortenedList.Last().Equals(lastElementMandatoryMatch))
            {
                shortenedList.RemoveAt(list.Count - 1);
            }

            return shortenedList;
        }

        public static IList<TElement> WithoutLast<TElement>(this IList<TElement> list)
        {
            var shortenedList = new List<TElement>(list);

            if (shortenedList.Count > 0)
            {
                shortenedList.RemoveAt(list.Count - 1);
            }

            return shortenedList;
        }

        public static void RemoveLast<TElement>(this IList<TElement> list, TElement lastElementMandatoryMatch)
        {
            if (list.Count > 0 && list.Last().Equals(lastElementMandatoryMatch))
            {
                list.RemoveAt(list.Count - 1);
            }
        }

        public static void RemoveLast<TElement>(this IList<TElement> list)
        {
            if (list.Count > 0)
            {
                list.RemoveAt(list.Count - 1);
            }
        }
    }
}