using System;

namespace NTouchTypeTrainer.Common.Primitives
{
    public static class DoubleExtensions
    {
        public const double DefaultRelativePrecision = 1e-6;

        public static bool IsApproximatelyEqual(this double reference, double comparison, double relativePrecision = DefaultRelativePrecision)
        {
            var diff = Math.Abs(reference - comparison);
            var relativeDiff = diff * Math.Abs(reference);
            return relativeDiff <= Math.Abs(relativePrecision);
        }

        public static bool IsApproximatelyEqual(this double? reference, double? comparison, double relativePrecision = DefaultRelativePrecision)
        {
            var differenthasValue = reference.HasValue ^ comparison.HasValue;
            if (differenthasValue)
            {
                return false;
            }

            return !reference.HasValue
                ? true    // if both have no value, they're considered equal
                : reference.Value.IsApproximatelyEqual(comparison.Value);
        }
    }
}