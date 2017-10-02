using System;
using System.Linq;

namespace NTouchTypeTrainer.Common.LINQ
{
    public static class IntExtensions
    {
        public static void Repeat(this int numberOfRepeats, Action repeat, Action afterRepeated = null)
        {
            Enumerable.Range(0, numberOfRepeats)
                .ForEach(repeatCount =>
                {
                    repeat();
                });

            if (numberOfRepeats > 0)
            {
                afterRepeated?.Invoke();
            }
        }
    }
}