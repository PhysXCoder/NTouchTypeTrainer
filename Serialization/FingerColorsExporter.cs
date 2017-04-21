using System.Text;
using Eto.Drawing;
using NTouchTypeTrainer.Contracts.Common;
using NTouchTypeTrainer.Contracts.Domain;

namespace NTouchTypeTrainer.Serialization
{
    public class FingerColorsExporter : FingerColorsBasePorter, IStringExporter<IFingerColors>
    {
        string IStringExporter<IFingerColors>.Export(IFingerColors fingerColors)
            => Export(fingerColors);

        public static string Export(IFingerColors fingerColors)
        {
            var builder = new StringBuilder();

            foreach (var colorProp in GetColorProperties())
            {
                Export(colorProp.Name, (Color)colorProp.GetValue(fingerColors), builder);
            }

            return builder.ToString();
        }

        private static void Export(string propertyName, Color value, StringBuilder builder)
        {
            builder
                .Append(propertyName)
                .Append(NameColorSeparator)
                .Append(value)
                .Append(NewLine);
        }
    }
}