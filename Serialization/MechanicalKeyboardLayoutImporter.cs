using NTouchTypeTrainer.Domain;
using NTouchTypeTrainer.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace NTouchTypeTrainer.Serialization
{
    public class MechanicalKeyboardLayoutImporter : BaseImporter, IStringImport<MechanicalKeyboardLayout>
    {
        protected const string RegularKey = "_";
        protected const string ProportionalKey = "~";
        protected static readonly NumberFormatInfo FloatFormat = CultureInfo.InvariantCulture.NumberFormat;

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
            if (!Single.TryParse(sizeString, NumberStyles.Number, FloatFormat, out size))
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
    }
#endif
}