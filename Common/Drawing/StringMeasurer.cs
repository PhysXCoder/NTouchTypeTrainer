using NTouchTypeTrainer.Interfaces.Common.Gui;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace NTouchTypeTrainer.Common.Drawing
{
    public static class StringMeasurer
    {
        public static Size MeasureString(
            string textToMeasure,
            IFont font,
            FlowDirection flowDirection = FlowDirection.LeftToRight,
            Brush textBrush = null)
        {
            return MeasureString(textToMeasure, font.Family, font.Size, font.Style, font.Weight, font.Stretch, flowDirection, textBrush);
        }

        public static Size MeasureString(
            string textToMeasure,
            FontFamily fontFamily,
            double fontSize,
            FontStyle fontStyle,
            FontWeight fontWeight,
            FontStretch fontStretch,
            FlowDirection flowDirection = FlowDirection.LeftToRight,
            Brush textBrush = null)
        {
            textBrush = textBrush ?? new SolidColorBrush(Colors.Black);

            var formattedText = new FormattedText(
                textToMeasure,
                CultureInfo.CurrentUICulture,
                flowDirection,
                new Typeface(fontFamily, fontStyle, fontWeight, fontStretch),
                fontSize,
                textBrush);

            return new Size(formattedText.Width, formattedText.Height);
        }
    }
}