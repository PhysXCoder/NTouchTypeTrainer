using NTouchTypeTrainer.Common.Gui;
using NTouchTypeTrainer.Common.Serialization;
using NTouchTypeTrainer.Domain.Keyboard;
using NTouchTypeTrainer.Interfaces.Common;
using NTouchTypeTrainer.Interfaces.Common.Gui;
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Media;

namespace NTouchTypeTrainer.Serialization
{
    public class FingerColorsImporter : BaseImporter, IStringImport<FingerColors>
    {
        protected const string NameColorSeparator = Separator + " ";

        private static readonly IThemeProvider ThemeProvider;

        static FingerColorsImporter()
        {
            ThemeProvider = new DefaultThemeProvider();
        }

        bool IStringImport<FingerColors>.TryImport(string exportedString, out FingerColors outputInstance)
            => TryImport(exportedString, out outputInstance);

        FingerColors IStringImport<FingerColors>.Import(string exportedString)
            => Import(exportedString);

        public static FingerColors Import(string exportedString)
        {
            Import(exportedString, true, out FingerColors fingerColors);
            return fingerColors;
        }

        public static bool TryImport(string exportedString, out FingerColors outputInstance)
        {
            return Import(exportedString, false, out outputInstance);
        }

        protected static PropertyInfo[] GetColorProperties() =>
            typeof(FingerColors).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.PropertyType == typeof(Color))
                .ToArray();

        private static bool Import(string exportedString, bool throwExceptions, out FingerColors outputInstance)
        {
            outputInstance = new FingerColors(ThemeProvider);

            var colorProperties = GetColorProperties();
            var lines = exportedString.Split(new[] { NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var lineParseSuccess = false;

                var tokens = line.Split(new[] { NameColorSeparator }, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length == 2)
                {
                    var name = tokens[0];
                    var value = tokens[1];

                    var colorProp = colorProperties.FirstOrDefault(prop => prop.Name == name);
                    if (colorProp != null)
                    {
                        if (ColorExtensions.TryParse(value, out Color col))
                        {
                            colorProp.SetValue(outputInstance, col);
                            lineParseSuccess = true;
                        }
                    }
                }

                if (!lineParseSuccess)
                {
                    if (throwExceptions)
                    {
                        throw new FormatException($"Couldn't parse finger color '{line}'!");
                    }

                    return false;
                }
            }

            return true;
        }
    }
}