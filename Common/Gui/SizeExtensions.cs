using NTouchTypeTrainer.Common.Primitives;
using System;
using System.Windows;

namespace NTouchTypeTrainer.Common.Gui
{
    public static class SizeExtensions
    {
        public static Size Increase(this Size size, Size enlargement)
            => new Size(size.Width + enlargement.Width, size.Height + enlargement.Height);

        public static Size Increase(this Size size, int enlargement)
            => size.Increase(new Size(enlargement, enlargement));

        public static Size Multiply(this Size size, int factor)
            => new Size(size.Width * factor, size.Height * factor);

        public static Size Multiply(this Size size, float factor)
            => size.Multiply((double)factor);

        public static Size Multiply(this Size size, double factor)
            => new Size((int)Math.Round(size.Width * factor), (int)Math.Round(size.Height * factor));

        public static bool IsApproximatelyEqual(this Size reference, Size comparison, double relativePrecision = DoubleExtensions.DefaultRelativePrecision)
            => reference.Width.IsApproximatelyEqual(comparison.Width, relativePrecision) &&
               reference.Height.IsApproximatelyEqual(comparison.Height, relativePrecision);
    }
}