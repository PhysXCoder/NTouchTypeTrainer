using NTouchTypeTrainer.Contracts.Common;
using NTouchTypeTrainer.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace NTouchTypeTrainer.Serialization
{
    public class MechanicalKeyboardLayoutImporter : MechanicalKeyboardLayoutBasePorter, IStringImport<MechanicalKeyboardLayout>
    {
        bool IStringImport<MechanicalKeyboardLayout>.TryImport(string exportedString, out MechanicalKeyboardLayout outputLayout)
            => TryImport(exportedString, out outputLayout);

        MechanicalKeyboardLayout IStringImport<MechanicalKeyboardLayout>.Import(string exportedString)
            => Import(exportedString);

        public static MechanicalKeyboardLayout Import(string exportString)
        {
            Import(exportString, true, out MechanicalKeyboardLayout outputKeyboard);
            return outputKeyboard;
        }

        public static bool TryImport(string exportString, out MechanicalKeyboardLayout outputLayout)
            => Import(exportString, false, out outputLayout);

        private static bool Import(string exportString, bool throwExceptions, out MechanicalKeyboardLayout outputLayout)
        {
            outputLayout = null;
            var sizeDict = new Dictionary<KeyPosition, float?>();

            exportString = exportString.Trim();
            var lines = exportString.Split(new[] { NewLine }, StringSplitOptions.RemoveEmptyEntries);

            var iRow = 0;
            foreach (var line in lines)
            {
                iRow++;
                var keyStrings = line.Trim().Split(new[] { KeySeparator }, StringSplitOptions.RemoveEmptyEntries);

                var iKey = 0;
                foreach (var keyString in keyStrings)
                {
                    iKey++;
                    var position = new KeyPosition(iRow, iKey);

                    if (keyString == RegularKey)
                    {
                        sizeDict.Add(position, null);
                    }
                    else if (keyString.StartsWith(ProportionalKey))
                    {
                        var sizeString = keyString.Replace(ProportionalKey, "");

                        if (!GetSize(throwExceptions, sizeString, out float size))
                        {
                            return false;
                        }

                        sizeDict.Add(position, size);
                    }
                    else
                    {
                        throw new FormatException($"Couldn't parse '{keyString}'! Must either be '{RegularKey}' or "
                                                  + $"'{ProportionalKey}{1.234f.ToString(FloatFormat)}'!");
                    }
                }
            }

            outputLayout = new MechanicalKeyboardLayout(sizeDict);
            return true;
        }

        private static bool GetSize(bool throwExceptions, string sizeString, out float size)
        {
            if (!float.TryParse(sizeString, NumberStyles.Number, FloatFormat, out size))
            {
                if (throwExceptions)
                {
                    throw new FormatException($"Couldn't parse '{sizeString}' to a floating point variable!");
                }
                return false;
            }
            return true;
        }
    }

#if false
    public class MechanicalKeyboardLayoutImporter : MechanicalKeyboardLayoutBasePorter, IKeyboardLayoutImporter
    {
        bool IStringImport<MechanicalKeyboardLayout>.TryImport(string exportedString, out MechanicalKeyboardLayout outputLayout) =>
            TryImport(exportedString, out outputLayout);

        MechanicalKeyboardLayout IStringImport<MechanicalKeyboardLayout>.Import(string exportedString) =>
            Import(exportedString);

        public static MechanicalKeyboardLayout Import(string exportString)
        {
            var digitsRow = new List<IKeyMapping>();
            var upperCharacterRow = new List<IKeyMapping>();
            var middleCharacterRow = new List<IKeyMapping>();
            var lowerCharacterRow = new List<IKeyMapping>();
            var controlKeyRow = new List<IKeyMapping>();

            while (exportString.Length > 0)
            {
                var modifier = GetModifier(exportString, AllModifiers);
                exportString = exportString.Remove(0, GetModifierStartToken(modifier).Length);

                ImportRow(digitsRow, modifier, HardwareKey.AccentGrave, ref exportString);
                ImportRow(upperCharacterRow, modifier, HardwareKey.Tab, ref exportString);
                ImportRow(middleCharacterRow, modifier, HardwareKey.CapsLock, ref exportString);
                ImportRow(lowerCharacterRow, modifier, HardwareKey.ShiftLeft, ref exportString);
                ImportRow(controlKeyRow, modifier, HardwareKey.ControlLeft, ref exportString);

                exportString = exportString.TrimStart();
            }

            digitsRow = SortRow(digitsRow).ToList();
            upperCharacterRow = SortRow(upperCharacterRow).ToList();
            middleCharacterRow = SortRow(middleCharacterRow).ToList();
            lowerCharacterRow = SortRow(lowerCharacterRow).ToList();
            controlKeyRow = SortRow(controlKeyRow).ToList();

            return new MechanicalKeyboardLayout(
                digitsRow,
                upperCharacterRow,
                middleCharacterRow,
                lowerCharacterRow,
                controlKeyRow);
        }

        public static bool TryImport(string exportString, out MechanicalKeyboardLayout layout)
        {
            layout = null;

            var digitsRow = new List<IKeyMapping>();
            var upperCharacterRow = new List<IKeyMapping>();
            var middleCharacterRow = new List<IKeyMapping>();
            var lowerCharacterRow = new List<IKeyMapping>();
            var controlKeyRow = new List<IKeyMapping>();

            while (exportString.Length > 0)
            {
                if (!TryGetModifier(exportString, AllModifiers, out Modifier modifier))
                {
                    return false;
                }

                exportString = exportString.Remove(0, GetModifierStartToken(modifier).Length);

                if (!TryImportForRow(digitsRow, modifier, HardwareKey.AccentGrave, ref exportString)
                    || !TryImportForRow(upperCharacterRow, modifier, HardwareKey.Tab, ref exportString)
                    || !TryImportForRow(middleCharacterRow, modifier, HardwareKey.CapsLock, ref exportString)
                    || !TryImportForRow(lowerCharacterRow, modifier, HardwareKey.ShiftLeft, ref exportString)
                    || !TryImportForRow(controlKeyRow, modifier, HardwareKey.ControlLeft, ref exportString))
                {
                    return false;
                }

                exportString = exportString.TrimStart();
            }

            digitsRow = SortRow(digitsRow).ToList();
            upperCharacterRow = SortRow(upperCharacterRow).ToList();
            middleCharacterRow = SortRow(middleCharacterRow).ToList();
            lowerCharacterRow = SortRow(lowerCharacterRow).ToList();
            controlKeyRow = SortRow(controlKeyRow).ToList();

            layout = new MechanicalKeyboardLayout(
                digitsRow,
                upperCharacterRow,
                middleCharacterRow,
                lowerCharacterRow,
                controlKeyRow);

            return true;
        }

        private static Modifier GetModifier(string exportString, IEnumerable<Modifier?> allModifiers)
        {
            var nextModifier = allModifiers
                .Where(m => m.HasValue)
                .FirstOrDefault(m => exportString.StartsWith(GetModifierStartToken(m.Value)));

            if (nextModifier == null)
            {
                throw new FormatException(
                    $"Couldn't parse '{GetShortenedString(exportString)}' to a keyboard modifier (e.g. {Modifier.Shift})");
            }

            return nextModifier.Value;
        }

        private static bool TryGetModifier(
            string exportString,
            IEnumerable<Modifier?> allModifiers,
            out Modifier modifier)
        {
            var nextModifier = allModifiers
                .Where(m => m.HasValue)
                .FirstOrDefault(m => exportString.StartsWith(GetModifierStartToken(m.Value)));

            if (nextModifier != null)
            {
                modifier = nextModifier.Value;
                return true;
            }

            modifier = Modifier.None;
            return false;
        }

        private static string GetShortenedString(string exportString)
        {
            return (exportString.Length > 20) ? exportString.Substring(0, 20) + "..." : exportString;
        }

        private static void ImportRow(
            ICollection<IKeyMapping> outputRow,
            Modifier modifier,
            HardwareKey rowStartKey,
            ref string exportString)
        {
            var mappingTargetsLine = GetMappingTargetsLine(ref exportString);

            var mappingTargets = mappingTargetsLine.Split(new[] { KeySeparator }, StringSplitOptions.RemoveEmptyEntries);

            var currentKey = rowStartKey;
            foreach (var mappingTarget in mappingTargets)
            {
                if (mappingTarget != Undefined)
                {
                    var mappingSuccess = TryImportMapping(mappingTarget, modifier, currentKey, out IKeyMapping mapping);

                    if (mappingSuccess)
                    {
                        AddToRow(mapping, outputRow);
                    }
                    else
                    {
                        throw new FormatException($"Couldn't parse mapping target '{mappingTarget}'!");
                    }
                }

                ++currentKey;
            }
        }

        private static bool TryImportForRow(
            ICollection<IKeyMapping> outputRow,
            Modifier modifier,
            HardwareKey rowStartKey,
            ref string exportString)
        {
            if (!TryGetMappingTargetLine(ref exportString, out string mappingTargetsLine))
            {
                return false;
            }

            var mappingTargets = mappingTargetsLine.Split(new[] { KeySeparator }, StringSplitOptions.RemoveEmptyEntries);

            var currentKey = rowStartKey;
            foreach (var mappingTarget in mappingTargets)
            {
                if (mappingTarget != Undefined)
                {
                    var mappingSuccess = TryImportMapping(mappingTarget, modifier, currentKey, out IKeyMapping mapping);

                    if (mappingSuccess)
                    {
                        AddToRow(mapping, outputRow);
                    }
                    else
                    {
                        return false;
                    }
                }

                ++currentKey;
            }

            return true;
        }

        private static string GetMappingTargetsLine(ref string exportString)
        {
            exportString = exportString.TrimStart();

            var iRowSep = exportString.IndexOf(RowSeparator, StringComparison.Ordinal);

            if (iRowSep < 0)
            {
                throw new FormatException(
                    $"Couldn't parse line '{GetShortenedString(exportString)}'. Row separator not found!");
            }

            var mappingTargetLine = exportString.Substring(0, iRowSep);
            exportString = exportString.Substring(iRowSep + RowSeparator.Length);

            return mappingTargetLine;
        }

        private static bool TryGetMappingTargetLine(ref string exportString, out string mappingTargetLine)
        {
            exportString = exportString.TrimStart();

            var iRowSep = exportString.IndexOf(RowSeparator, StringComparison.Ordinal);

            if (iRowSep < 0)
            {
                mappingTargetLine = null;
                return false;
            }

            mappingTargetLine = exportString.Substring(0, iRowSep);
            exportString = exportString.Substring(iRowSep + RowSeparator.Length);

            return true;
        }

        private static bool TryImportMapping(string mappingTarget,
            Modifier modifier,
            HardwareKey currentKey,
            out IKeyMapping keyMapping)
        {
            var mappingSuccess = TryImportPrintableMapping(mappingTarget, modifier, currentKey, out keyMapping);

            if (!mappingSuccess)
            {
                mappingSuccess = TryImportUnprintableMapping(mappingTarget, modifier, currentKey, out keyMapping);
            }

            return mappingSuccess;
        }

        private static bool TryImportUnprintableMapping(
            string mappingTarget,
            Modifier modifier,
            HardwareKey currentKey,
            out IKeyMapping keyMapping)
        {
            var parseSuccess = MappedUnprintable.TryImport(mappingTarget, out MappedUnprintable unprintable);

            keyMapping = parseSuccess
                ? new KeyMapping(currentKey, new Tuple<Modifier, IMappedKey>(modifier, unprintable))
                : null;
            return parseSuccess;
        }

        private static bool TryImportPrintableMapping(string mappingTarget, Modifier modifier,
            HardwareKey currentKey, out IKeyMapping keyMapping)
        {
            var parseSuccess = MappedCharacter.TryImport(mappingTarget, out MappedCharacter character);

            keyMapping = parseSuccess
                ? new KeyMapping(currentKey, new Tuple<Modifier, IMappedKey>(modifier, character))
                : null;
            return parseSuccess;
        }

        private static void AddToRow(IKeyMapping mapping, ICollection<IKeyMapping> outputRow)
        {
            var oldKeyMapping = outputRow.FirstOrDefault(m => m.PressedKey == mapping.PressedKey);

            if (oldKeyMapping != null)
            {
                outputRow.Remove(oldKeyMapping);
            }

            var newMappings = oldKeyMapping?.Mappings?.Concat(mapping.Mappings) ?? mapping.Mappings;

            outputRow.Add(new KeyMapping(mapping.PressedKey, newMappings));
        }

        private static IEnumerable<IKeyMapping> SortRow(IEnumerable<IKeyMapping> keyMappingRow)
        {
            return keyMappingRow.OrderBy(mapping => mapping.PressedKey).ToList();
        }
    }
#endif
}