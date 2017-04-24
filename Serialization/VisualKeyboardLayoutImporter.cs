using NTouchTypeTrainer.Domain;
using NTouchTypeTrainer.Domain.Enums;
using NTouchTypeTrainer.Interfaces.Common;
using NTouchTypeTrainer.Interfaces.Domain;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using static NTouchTypeTrainer.Domain.Enums.ModifierExtensions;

namespace NTouchTypeTrainer.Serialization
{
    public class VisualKeyboardLayoutImporter : BaseImporter, IStringImport<VisualKeyboardLayout>
    {
        protected const string MappingNameSeparator = Separator + NewLine;
        protected const string NoModifier = "None";

        VisualKeyboardLayout IStringImport<VisualKeyboardLayout>.Import(string exportedString)
            => Import(exportedString);

        bool IStringImport<VisualKeyboardLayout>.TryImport(string exportedString, out VisualKeyboardLayout outputInstance)
            => TryImport(exportedString, out outputInstance);

        protected static string GetModifierStartToken(Modifier? modifier)
        {
            return (modifier?.ToString() ?? NoModifier) + MappingNameSeparator;
        }

        public static bool TryImport(string exportedString, out VisualKeyboardLayout outputInstance)
            => Import(exportedString, false, out outputInstance);

        public static VisualKeyboardLayout Import(string exportedString)
        {
            Import(exportedString, true, out VisualKeyboardLayout outputInstance);
            return outputInstance;
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private static bool Import(
            string exportedString,
            bool throwExceptions,
            out VisualKeyboardLayout outputLayout)
        {
            outputLayout = null;
            var keyMappings = new List<IPressedKeyMapping>();

            exportedString = exportedString.TrimStart();
            while (!exportedString.IsEmpty())
            {
                if (!GetAndRemoveModifier(ref exportedString, throwExceptions, out Modifier modifier))
                {
                    return false;
                }

                var iRow = 0;
                while (!StartsWithModifier(exportedString) && !exportedString.IsEmpty())
                {
                    ImportKeyRow(keyMappings, modifier, ++iRow, throwExceptions, ref exportedString);
                    exportedString = exportedString.TrimStart();
                }
            }

            outputLayout = new VisualKeyboardLayout(keyMappings);
            return true;
        }

        private static bool GetAndRemoveModifier(ref string exportedString, bool throwExceptions, out Modifier modifier)
        {
            if (throwExceptions)
            {
                modifier = ParseModifier(exportedString);
            }
            else
            {
                if (!TryParseModifier(exportedString, out modifier))
                {
                    return false;
                }
            }

            var modifierStartToken = GetModifierStartToken(modifier);
            exportedString = exportedString.Remove(0, modifierStartToken.Length);
            return true;
        }

        private static bool StartsWithModifier(string exportedstring)
            => TryParseModifier(exportedstring, out Modifier _);

        [SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
        private static bool TryParseModifier(string exportString, out Modifier modifier)
        {
            var nextModifier = AllModifiers
                .Cast<Modifier?>()
                .FirstOrDefault(m => exportString.StartsWith(GetModifierStartToken(m.Value)));

            if (nextModifier != null)
            {
                modifier = nextModifier.Value;
                return true;
            }

            modifier = Modifier.None;
            return false;
        }

        [SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
        private static Modifier ParseModifier(string exportString)
        {
            var nextModifier = AllModifiers
                .Cast<Modifier?>()
                .FirstOrDefault(m => exportString.StartsWith(GetModifierStartToken(m.Value)));

            if (nextModifier == null)
            {
                throw new FormatException(
                    $"Couldn't parse '{GetShortenedString(exportString)}' to a keyboard modifier (e.g. {Modifier.Shift})");
            }

            return nextModifier.Value;
        }

        private static string GetShortenedString(string exportString)
        {
            return (exportString.Length > 20) ? exportString.Substring(0, 20) + "..." : exportString;
        }

        private static void ImportKeyRow(
            ICollection<IPressedKeyMapping> keyMappings,
            Modifier modifier,
            int iRow,
            bool throwExceptions,
            ref string exportString)
        {
            if (!GetMappingTargetLine(ref exportString, throwExceptions, out string mappingTargetsLine))
            {
                return;
            }

            var mappingTargetStrings = mappingTargetsLine.Split(new[] { KeySeparator }, StringSplitOptions.RemoveEmptyEntries);
            if (!mappingTargetStrings.Any())
            {
                return;
            }

            var iKey = 0;
            foreach (var mappingTargetString in mappingTargetStrings)
            {
                iKey++;

                if (mappingTargetString != Undefined)
                {
                    var pressedKey = new PressedKey(new KeyPosition(iRow, iKey), modifier);

                    var mappingSuccess = TryImportMapping(mappingTargetString, pressedKey, out IPressedKeyMapping mapping);
                    if (mappingSuccess)
                    {
                        SetMapping(mapping, keyMappings);
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        private static bool GetMappingTargetLine(ref string exportString, bool throwExceptions, out string mappingTargetLine)
        {
            exportString = exportString.TrimStart();

            var iRowSep = exportString.IndexOf(RowSeparator, StringComparison.Ordinal);

            if (iRowSep < 0)
            {
                mappingTargetLine = null;
                if (throwExceptions)
                {
                    throw new FormatException($"Could not find RowSeparator token at the end of line '{exportString}'!");
                }
                return false;
            }

            mappingTargetLine = exportString.Substring(0, iRowSep);
            exportString = exportString.Substring(iRowSep + RowSeparator.Length);

            return true;
        }

        private static bool TryImportMapping(
            string mappingTarget,
            PressedKey pressedKey,
            out IPressedKeyMapping keyMapping)
        {
            var mappingSuccess = TryImportPrintableMapping(mappingTarget, pressedKey, out keyMapping);

            if (!mappingSuccess)
            {
                mappingSuccess = TryImportUnprintableMapping(mappingTarget, pressedKey, out keyMapping);
            }

            return mappingSuccess;
        }

        private static bool TryImportUnprintableMapping(
            string mappingTarget,
            PressedKey pressedKey,
            out IPressedKeyMapping keyMapping)
        {
            var parseSuccess = MappedUnprintable.TryImport(mappingTarget, out MappedUnprintable unprintable);

            keyMapping = parseSuccess ? new KeyMapping(pressedKey, unprintable) : null;
            return parseSuccess;
        }

        private static bool TryImportPrintableMapping(
            string mappingTarget,
            PressedKey pressedKey,
            out IPressedKeyMapping keyMapping)
        {
            var parseSuccess = MappedCharacter.TryImport(mappingTarget, out MappedCharacter character);

            keyMapping = parseSuccess ? new KeyMapping(pressedKey, character) : null;
            return parseSuccess;
        }

        [SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
        private static void SetMapping(IPressedKeyMapping mapping, ICollection<IPressedKeyMapping> keyMappings)
        {
            var oldKeyMapping = keyMappings.FirstOrDefault(m => m.Equals(mapping));
            if (oldKeyMapping != null)
            {
                keyMappings.Remove(oldKeyMapping);
            }

            keyMappings.Add(mapping);
        }
    }
}