using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTouchTypeTrainer.Contracts;
using NTouchTypeTrainer.Contracts.Enums;

namespace NTouchTypeTrainer.Serialization
{
    public class KeyboardLayoutExporter : KeyboardLayoutBasePorter, IKeyboardLayoutExporter
    {
        string IKeyboardLayoutExporter.Export(IKeyboardLayout layout) => Export(layout);

        public static string Export(IKeyboardLayout layout)
        {
            if (layout == null)
            {
                throw new ArgumentNullException(nameof(layout));
            }

            var exportedKeyboardMappings = new StringBuilder();

            foreach (var modifier in Modifier.All.GetAllCombinations())
            {
                if (ContainsMapping(layout, modifier))
                {
                    exportedKeyboardMappings.Append(GetModifierStartToken(modifier));

                    Export(layout, modifier, exportedKeyboardMappings);

                    exportedKeyboardMappings.Append(NewLine);
                }
            }

            return exportedKeyboardMappings.ToString();
        }

        private static void Export(IKeyboardLayout layout, Modifier modifier, StringBuilder exportBuilder)
        {
            ExportRow(layout.DigitsRow, modifier, exportBuilder);
            exportBuilder.Append(RowSeparator);

            ExportRow(layout.UpperCharacterRow, modifier, exportBuilder);
            exportBuilder.Append(RowSeparator);

            ExportRow(layout.MiddleCharacterRow, modifier, exportBuilder);
            exportBuilder.Append(RowSeparator);

            ExportRow(layout.LowerCharacterRow, modifier, exportBuilder);
            exportBuilder.Append(RowSeparator);

            ExportRow(layout.ControlKeyRow, modifier, exportBuilder);
            exportBuilder.Append(RowSeparator);
        }

        private static void ExportRow(IEnumerable<IKeyMapping> rowOfKeys, Modifier modifier, StringBuilder exportBuilder)
        {
            var mappedKeys = rowOfKeys.OrderBy(km => (int) km.PressedKey).ToList();

            int numberOfMappedKeys = mappedKeys.Count();

            for (int i = 0; i < numberOfMappedKeys; i++)
            {
                exportBuilder.Append(mappedKeys[i].Export(modifier));

                var isLast = (i == numberOfMappedKeys - 1);
                if (!isLast)
                {
                    exportBuilder.Append(KeySeparator);
                }
            }
        }
    }
}