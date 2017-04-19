using System;
using System.Linq;
using Eto.Drawing;
using NTouchTypeTrainer.Domain;

namespace NTouchTypeTrainer.Serialization
{
    public class FingerColorsImporter : FingerColorsBasePorter
    {
        public FingerColors Import(string exportedString)
        {
            Import(exportedString, true, out FingerColors fingerColors);
            return fingerColors;
        }

        public bool TryImport(string exportedString, out FingerColors outputInstance)
        {
            return Import(exportedString, false, out outputInstance);
        }

        private bool Import(string exportedString, bool throwExceptions, out FingerColors outputInstance)
        {
            outputInstance = new FingerColors();

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
                        if (Color.TryParse(value, out Color col))
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