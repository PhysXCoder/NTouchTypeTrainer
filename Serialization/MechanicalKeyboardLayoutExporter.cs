using System;
using System.Collections.Generic;
using System.Text;
using NTouchTypeTrainer.Common.Strings;
using NTouchTypeTrainer.Contracts.Common;
using NTouchTypeTrainer.Contracts.Domain;
using NTouchTypeTrainer.Domain.Enums;

namespace NTouchTypeTrainer.Serialization
{
    public class MechanicalKeyboardLayoutExporter : KeyboardLayoutBasePorter, IStringExporter<IMechanicalKeyboardLayout>
    {
        string IStringExporter<IMechanicalKeyboardLayout>.Export(IMechanicalKeyboardLayout layout)
            => Export(layout);

        public static string Export(IMechanicalKeyboardLayout layout)
        {
            if (layout == null)
            {
                throw new ArgumentNullException(nameof(layout));
            }

            var exportedKeyboardMappings = new StringBuilder();

            foreach (var row in layout.KeyboardRows)
            {
                Export(row, exportedKeyboardMappings);
                exportedKeyboardMappings.Append(NewLine);
            }
            exportedKeyboardMappings.RemoveLast(NewLine);

            return exportedKeyboardMappings.ToString();
        }

        private static void Export(IEnumerable<HardwareKey> row, StringBuilder exportBuilder)
        {
            foreach (var hardwareKey in row)
            {
                exportBuilder
                    .Append(hardwareKey)
                    .Append(KeySeparator);
            }
            exportBuilder.RemoveLast(KeySeparator);
        }
    }

#if false
    public class MechanicalKeyboardLayoutExporter : KeyboardLayoutBasePorter, IMechanicalKeyboardLayoutExporter
    {
        string IMechanicalKeyboardLayoutExporter.Export(IMechanicalKeyboardLayout layout) => Export(layout);

        public static string Export(IMechanicalKeyboardLayout layout)
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

        private static void Export(IMechanicalKeyboardLayout layout, Modifier modifier, StringBuilder exportBuilder)
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
            var mappedKeys = rowOfKeys.OrderBy(km => (int)km.PressedKey).ToList();

            var numberOfMappedKeys = mappedKeys.Count;

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
#endif
}