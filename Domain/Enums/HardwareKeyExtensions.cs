using System;
using System.Collections.Generic;
using System.Linq;

namespace NTouchTypeTrainer.Domain.Enums
{
    public static class HardwareKeyExtensions
    {
        public static bool TryParse(string exportedString, out HardwareKey hardwareKey, bool ignoreCase = true)
        {
            return Enum.TryParse(exportedString, ignoreCase, out hardwareKey);
        }

        public static HardwareKey Parse(string exportedString, bool ignoreCase = true)
        {
            if (!TryParse(exportedString, out HardwareKey hardwareKey, ignoreCase))
            {
                throw new FormatException($"Couldn't parse hardware key '{exportedString}'!");
            }

            return hardwareKey;
        }

        public static IEnumerable<HardwareKey> GetAllKeys()
            => Enum.GetValues(typeof(HardwareKey)).Cast<HardwareKey>();

        private static readonly Dictionary<HardwareKey, string> SpecialDefaultTexts =
            new Dictionary<HardwareKey, string>
            {
                {HardwareKey.AccentGrave, "`"},
                {HardwareKey.D1, "1"},
                {HardwareKey.D2, "2"},
                {HardwareKey.D3, "3"},
                {HardwareKey.D4, "4"},
                {HardwareKey.D5, "5"},
                {HardwareKey.D6, "6"},
                {HardwareKey.D7, "7"},
                {HardwareKey.D8, "8"},
                {HardwareKey.D9, "9"},
                {HardwareKey.D0, "0"},
                {HardwareKey.Minus, "-"},
                {HardwareKey.Equal, "="},
                {HardwareKey.Backspace, "←"},

                {HardwareKey.Tab, "⭾"},
                {HardwareKey.BracketOpen, "["},
                {HardwareKey.BracketClose, "]"},
                {HardwareKey.Enter, "↲"},

                {HardwareKey.CapsLock, "↓"},
                {HardwareKey.Semicolon, ";"},
                {HardwareKey.Apostrophe, "'"},
                {HardwareKey.NumberSign, "#"},

                {HardwareKey.ShiftLeft, "⇫"},
                {HardwareKey.Backslash, "\\"},
                {HardwareKey.Comma, ","},
                {HardwareKey.Dot, "."},
                {HardwareKey.Slash, "/"},
                {HardwareKey.ShiftRight, "⇫"},

                {HardwareKey.ControlLeft, "Ctrl"},
                {HardwareKey.SuperLeft, "❐"},
                {HardwareKey.Alt, "Alt"},
                {HardwareKey.Space, " "},
                {HardwareKey.AltGr, "AltGr"},
                {HardwareKey.SuperRight, "❐"},
                {HardwareKey.Menu, "☰"},
                {HardwareKey.ControlRight, "Ctrl"},
            };

        public static string GetDefaultText(this HardwareKey key) =>
            SpecialDefaultTexts.ContainsKey(key) ? SpecialDefaultTexts[key] : key.ToString();
    }
}