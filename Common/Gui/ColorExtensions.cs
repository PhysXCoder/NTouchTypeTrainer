using System.Text.RegularExpressions;
using System.Windows.Media;
using NTouchTypeTrainer.Common.RegEx;
using Color = System.Windows.Media.Color;

namespace NTouchTypeTrainer.Common.Gui
{
    public static class ColorExtensions
    {

        public static bool TryParse(string colorName, out Color color)
        {
            try
            {
                var convertedColor = ColorConverter.ConvertFromString(colorName);
                if (convertedColor is Color)
                {
                    color = (Color)convertedColor;
                    return true;
                }

                var colorRegex = new Regex(@"^#(?<Alpha>[\da-fA-F]{1,2})(?<Red>[\da-fA-F]{1,2})(?<Green>[\da-fA-F]{1,2})(?<Blue>[\da-fA-F]{1,2})$");
                var regexMatch = colorRegex.Match(colorName);

                var parseSuccess = regexMatch.TryGetGroupContent("Alpha", out string alphaString)
                    & regexMatch.TryGetGroupContent("Red", out string redString)
                    & regexMatch.TryGetGroupContent("Green", out string greenString)
                    & regexMatch.TryGetGroupContent("Blue", out string blueString);

                if (!parseSuccess)
                {
                    color = Colors.Red;
                    return false;
                }

                var alpha = byte.Parse(alphaString, System.Globalization.NumberStyles.AllowHexSpecifier);
                var red = byte.Parse(redString, System.Globalization.NumberStyles.AllowHexSpecifier);
                var green = byte.Parse(greenString, System.Globalization.NumberStyles.AllowHexSpecifier);
                var blue = byte.Parse(blueString, System.Globalization.NumberStyles.AllowHexSpecifier);

                color = Color.FromArgb(alpha, red, green, blue);

                return true;
            }
            catch
            {
                color = Colors.Red;
                return false;
            }
        }
    }
}