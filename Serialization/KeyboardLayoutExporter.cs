using System;
using System.Collections.Generic;
using System.Linq;
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

            var exportedKeyboardMappings = new List<string>();

            foreach (var modifier in Modifier.All.GetAllCombinations())
            {
                if (ContainsMapping(layout, modifier))
                {
                    var exported = GetModifierStartToken(modifier) + Export(layout, modifier);
                    exportedKeyboardMappings.Add(exported);
                }
            }

            return string.Join(NewLine, exportedKeyboardMappings);
        }

        private static string Export(IKeyboardLayout layout, Modifier modifier)
        {
            return ExportRow(layout.DigitsRow, modifier) + RowSeparator
                   + ExportRow(layout.UpperCharacterRow, modifier) + RowSeparator
                   + ExportRow(layout.MiddleCharacterRow, modifier) + RowSeparator
                   + ExportRow(layout.LowerCharacterRow, modifier) + RowSeparator
                   + ExportRow(layout.ControlKeyRow, modifier) + RowSeparator;
        }

        private static string ExportRow(IEnumerable<IKeyMapping> rowOfKeys, Modifier modifier)
        {
            var mappedKeys = rowOfKeys
                .OrderBy(km => (int)km.PressedKey)
                .Select(km => km.Export(modifier));

            return string.Join(KeySeparator, mappedKeys);
        }
    }
}