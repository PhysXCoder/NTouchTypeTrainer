using System.Collections.Generic;
using System.Linq;

namespace NTouchTypeTrainer.Common.LINQ
{
    public static class ListExtensions
    {
        public static IList<TElement> RemoveLast<TElement>(this IList<TElement> list, TElement toRemove)
        {
            var shortenedList = new List<TElement>(list);

            if (shortenedList.Count > 0 && shortenedList.Last().Equals(toRemove))
            {
                shortenedList.RemoveAt(list.Count - 1);
            }

            return shortenedList;
        }
    }
}